using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Messenger hub responsible for taking subscriptions/publications and delivering of messages.
/// </summary>
public sealed class EventBusSystemHub : IEventBusSystemHub
{
    public EventBusSystemHub()
    {
        // default ctor
    }

    #region EventBusMessageSubscription Types
    private class WeakEventBusMessageSubscription<TMessage> : IEventBusMessageSubscription
        where TMessage : class, IEventBusMessage
    {
        private readonly WeakReference _onDeliveryCallback;
        private readonly WeakReference _messageFilter;

        public EventBusMessageSubscriptionToken SubscriptionToken { get; }

        public bool ShouldAttemptDelivery(IEventBusMessage message)
        {
            if (!(message is TMessage busMessage) || !_onDeliveryCallback.IsAlive || !_messageFilter.IsAlive)
            {
                return false;
            }

            return ((Func<TMessage, bool>)_messageFilter.Target).Invoke(busMessage);
        }

        public void Deliver(IEventBusMessage message)
        {
            if (!(message is TMessage busMessage))
            {
                throw new ArgumentException("Message is not the correct type");
            }

            if (!_onDeliveryCallback.IsAlive)
            {
                return;
            }

            ((Action<TMessage>)_onDeliveryCallback.Target).Invoke(busMessage);
        }

        /// <summary>
        /// Initializes a new instance of the WeakEventBusMessageSubscription class.
        /// </summary>
        /// <param name="subscriptionToken">SubscriptionToken</param>
        /// <param name="onDeliveryCallback">Delivery action</param>
        /// <param name="messageFilter">Filter function</param>
        public WeakEventBusMessageSubscription(EventBusMessageSubscriptionToken subscriptionToken,
            Action<TMessage> onDeliveryCallback, Func<TMessage, bool> messageFilter)
        {
            if (subscriptionToken == null)
            {
                throw new ArgumentNullException("subscriptionToken");
            }

            if (onDeliveryCallback == null)
            {
                throw new ArgumentNullException("onDeliveryCallback");
            }

            if (messageFilter == null)
            {
                throw new ArgumentNullException("messageFilter");
            }

            SubscriptionToken = subscriptionToken;
            _onDeliveryCallback = new WeakReference(onDeliveryCallback);
            _messageFilter = new WeakReference(messageFilter);
        }
    }

    private class StrongEventBusMessageSubscription<TMessage> : IEventBusMessageSubscription
        where TMessage : class, IEventBusMessage
    {
        private readonly Action<TMessage> _onDeliveryCallback;
        private readonly Func<TMessage, bool> _messageFilter;

        public EventBusMessageSubscriptionToken SubscriptionToken { get; }

        public bool ShouldAttemptDelivery(IEventBusMessage message)
        {
            if (message is TMessage busMessage)
            {
                return _messageFilter.Invoke(busMessage);
            }

            return false;
        }

        public void Deliver(IEventBusMessage message)
        {
            if (!(message is TMessage busMessage))
            {
                throw new ArgumentException("Message is not the correct type");
            }

            _onDeliveryCallback.Invoke(busMessage);
        }

        /// <summary>
        /// Initializes a new instance of the StrongEventBusMessageSubscription class.
        /// </summary>
        /// <param name="subscriptionToken">SubscriptionToken</param>
        /// <param name="onDeliveryCallback">Delivery action</param>
        /// <param name="messageFilter">Filter function</param>
        public StrongEventBusMessageSubscription(EventBusMessageSubscriptionToken subscriptionToken, Action<TMessage> onDeliveryCallback, Func<TMessage, bool> messageFilter)
        {
            if (subscriptionToken == null)
            {
                throw new ArgumentNullException("subscriptionToken");
            }

            if (onDeliveryCallback == null)
            {
                throw new ArgumentNullException("onDeliveryCallback");
            }

            if (messageFilter == null)
            {
                throw new ArgumentNullException("messageFilter");
            }

            SubscriptionToken = subscriptionToken;
            _onDeliveryCallback = onDeliveryCallback;
            _messageFilter = messageFilter;
        }
    }
    #endregion

    #region SubscriptionItems List
    private class SubscriptionItem
    {
        public IEventBusMessageSubscription Subscription { get; }

        public SubscriptionItem(IEventBusMessageSubscription subscription)
        {
            Subscription = subscription;
        }
    }
    private readonly object _subscriptionsLock = new object();
    private readonly List<SubscriptionItem> _subscriptions = new List<SubscriptionItem>();
    #endregion

    #region Public API
    /// <summary>
    /// Subscribe to a message type with the given destination and delivery action.
    /// All references are held with strong references
    /// All messages of this type will be delivered.
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    /// <param name="onDeliveryCallback">Action to invoke when message is delivered</param>
    /// <returns>EventBusMessageSubscriptionToken used for unsubscribing</returns>
    public EventBusMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> onDeliveryCallback)
        where TMessage : class, IEventBusMessage
    {
        return AddSubscriptionInternal<TMessage>(onDeliveryCallback, (m) => true, true);
    }

    /// <summary>
    /// Subscribe to a message type with the given destination and delivery action.
    /// All messages of this type will be delivered.
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    /// <param name="deliveryAction">Action to invoke when message is delivered</param>
    /// <param name="useStrongReferences">Use strong references to destination and deliveryAction </param>
    /// <returns>EventBusMessageSubscriptionToken used for unsubscribing</returns>
    public EventBusMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction,
        bool useStrongReferences) where TMessage : class, IEventBusMessage
    {
        return AddSubscriptionInternal<TMessage>(deliveryAction, (m) => true, useStrongReferences);
    }

    /// <summary>
    /// Subscribe to a message type with the given destination and delivery action with the given filter.
    /// All references are held with WeakReferences
    /// Only messages that pass the filter will be delivered.
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    /// <param name="deliveryAction">Action to invoke when message is delivered</param>
    /// <param name="messageFilter">Func to filter messages</param>
    /// <returns>EventBusMessageSubscriptionToken used for unsubscribing</returns>
    public EventBusMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction,
        Func<TMessage, bool> messageFilter) where TMessage : class, IEventBusMessage
    {
        return AddSubscriptionInternal<TMessage>(deliveryAction, messageFilter, true);
    }

    /// <summary>
    /// Subscribe to a message type with the given destination and delivery action with the given filter.
    /// All references are held with WeakReferences
    /// Only messages that pass the filter will be delivered.
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    /// <param name="deliveryAction">Action to invoke when message is delivered</param>
    /// <param name="useStrongReferences">Use strong references to destination and deliveryAction </param>
    /// <param name="messageFilter">Func to filter messages</param>
    /// <returns>EventBusMessageSubscriptionToken used for unsubscribing</returns>
    public EventBusMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction,
        Func<TMessage, bool> messageFilter, bool useStrongReferences) where TMessage : class, IEventBusMessage
    {
        return AddSubscriptionInternal<TMessage>(deliveryAction, messageFilter, useStrongReferences);
    }

    /// <summary>
    /// Unsubscribe from a particular message type.
    /// Does not throw an exception if the subscription is not found.
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    /// <param name="subscriptionToken">Subscription token received from Subscribe</param>
    public void Unsubscribe<TMessage>(EventBusMessageSubscriptionToken subscriptionToken)
        where TMessage : class, IEventBusMessage
    {
        RemoveSubscriptionInternal<TMessage>(subscriptionToken);
    }

    /// <summary>
    /// Unsubscribe from a particular message type.
    /// Does not throw an exception if the subscription is not found.
    /// </summary>
    /// <param name="subscriptionToken">Subscription token received from Subscribe</param>
    public void Unsubscribe(EventBusMessageSubscriptionToken subscriptionToken)
    {
        RemoveSubscriptionInternal<IEventBusMessage>(subscriptionToken);
    }

    /// <summary>
    /// Publish a message to any subscribers
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    /// <param name="message">Message to deliver</param>
    public void Publish<TMessage>(TMessage message) where TMessage : class, IEventBusMessage
    {
        PublishInternal<TMessage>(message);
    }
    #endregion

    #region Internal Methods
    private EventBusMessageSubscriptionToken AddSubscriptionInternal<TMessage>(Action<TMessage> deliveryAction,
        Func<TMessage, bool> messageFilter, bool strongReference) where TMessage : class, IEventBusMessage
    {
        if (deliveryAction == null)
        {
            throw new ArgumentNullException("deliveryAction");
        }

        if (messageFilter == null)
        {
            throw new ArgumentNullException("messageFilter");
        }

        lock (_subscriptionsLock)
        {
            var subscriptionToken = new EventBusMessageSubscriptionToken(this, typeof(TMessage));

            IEventBusMessageSubscription subscription;
            if (strongReference)
            {
                subscription = new StrongEventBusMessageSubscription<TMessage>(subscriptionToken, deliveryAction, messageFilter);
            }
            else
            {
                subscription = new WeakEventBusMessageSubscription<TMessage>(subscriptionToken, deliveryAction, messageFilter);
            }

            _subscriptions.Add(new SubscriptionItem(subscription));
            return subscriptionToken;
        }
    }

    private void RemoveSubscriptionInternal<TMessage>(EventBusMessageSubscriptionToken subscriptionToken)
            where TMessage : class, IEventBusMessage
    {
        if (subscriptionToken == null)
        {
            throw new ArgumentNullException("subscriptionToken");
        }

        lock (_subscriptionsLock)
        {
            var currentlySubscribed =
                _subscriptions.Where(sub => ReferenceEquals(sub.Subscription.SubscriptionToken, subscriptionToken))
                    .ToList();
            currentlySubscribed.ForEach(sub => _subscriptions.Remove(sub));
        }
    }

    private void PublishInternal<TMessage>(TMessage message)
            where TMessage : class, IEventBusMessage
    {
        if (message == null)
        {
            throw new ArgumentNullException("message");
        }

        List<SubscriptionItem> currentlySubscribed;
        lock (_subscriptionsLock)
        {
            currentlySubscribed = _subscriptions.Where(sub => sub.Subscription.ShouldAttemptDelivery(message))
                .ToList();
        }

        currentlySubscribed.ForEach(sub =>
        {
            try
            {
                sub.Subscription.Deliver(message);
            }
            catch (Exception exception)
            {
                    // Log Exception and continue
                    Debug.LogException(exception);
            }
        });
    }
    #endregion
}

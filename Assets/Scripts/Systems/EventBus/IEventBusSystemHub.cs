using System;

/// <summary>
/// Interface defining api for subscription, publication, and delivery of messages.
/// </summary>
public interface IEventBusSystemHub
{
    /// <summary>
    /// Subscribe to a message type with the given destination and delivery callback.
    /// All references are held with WeakReferences
    /// All messages of this type will be delivered.
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    /// <param name="onDeliveryCallback">Action to invoke when message is delivered</param>
    /// <returns>EventBusMessageSubscriptionToken used for unsubscribing</returns>
    EventBusMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> onDeliveryCallback) where TMessage : class, IEventBusMessage;

    /// <summary>
    /// Subscribe to a message type with the given destination, and delivery callback.
    /// All messages of this type will be delivered.
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    /// <param name="onDeliveryCallback">Action to invoke when message is delivered</param>
    /// <param name="useStrongReferences">Use strong references to destination and deliveryAction </param>
    /// <returns>EventBusMessageSubscriptionToken used for unsubscribing</returns>
    EventBusMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> onDeliveryCallback, bool useStrongReferences) where TMessage : class, IEventBusMessage;

    /// <summary>
    /// Subscribe to a message type with the given destination, delivery callback, and filter.
    /// All references are held with WeakReferences
    /// Only messages that pass the filter will be delivered.
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    /// <param name="onDeliveryCallback">Action to invoke when message is delivered</param>
    /// <returns>EventBusMessageSubscriptionToken used for unsubscribing</returns>
    EventBusMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> onDeliveryCallback, Func<TMessage, bool> messageFilter) where TMessage : class, IEventBusMessage;

    /// <summary>
    /// Subscribe to a message type with the given destination, delivery callback, and filter.
    /// All references are held with WeakReferences
    /// Only messages that pass the filter will be delivered.
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    /// <param name="onDeliveryCallback">Action to invoke when message is delivered</param>
    /// <param name="useStrongReferences">Use strong references to destination and deliveryAction </param>
    /// <returns>EventBusMessageSubscriptionToken used for unsubscribing</returns>
    EventBusMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> onDeliveryCallback, Func<TMessage, bool> messageFilter, bool useStrongReferences) where TMessage : class, IEventBusMessage;

    /// <summary>
    /// Unsubscribe from a particular message type.
    /// Does not throw an exception if the subscription is not found.
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    /// <param name="subscriptionToken">Subscription token received when subscribing</param>
    void Unsubscribe<TMessage>(EventBusMessageSubscriptionToken subscriptionToken) where TMessage : class, IEventBusMessage;

    /// <summary>
    /// Unsubscribe from a particular message type.
    /// Does not throw an exception if the subscription is not found.
    /// </summary>
    /// <param name="subscriptionToken">Subscription token received when subscribing</param>
    void Unsubscribe(EventBusMessageSubscriptionToken subscriptionToken);

    /// <summary>
    /// Publish a message to any subscribers
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    /// <param name="message">Message to deliver</param>
    void Publish<TMessage>(TMessage message) where TMessage : class, IEventBusMessage;

}

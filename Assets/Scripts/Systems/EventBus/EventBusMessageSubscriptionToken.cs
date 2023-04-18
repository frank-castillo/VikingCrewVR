using System;

/// <summary>
/// An unmanaged resource representing an EventBusMessageSubscription
/// </summary>
public sealed class EventBusMessageSubscriptionToken : IDisposable
{
    private readonly WeakReference _hub;
    private readonly Type _messageType;

    /// <summary>
    /// EventBusMessageSubscriptionToken class ctor
    /// </summary>
    public EventBusMessageSubscriptionToken(IEventBusSystemHub hub, Type messageType)
    {
        if (hub == null)
        {
            throw new ArgumentNullException("hub");
        }

        if (!typeof(IEventBusMessage).IsAssignableFrom(messageType))
        {
            throw new ArgumentOutOfRangeException("messageType");
        }


        _hub = new WeakReference(hub);
        _messageType = messageType;
    }

    public void Dispose()
    {
        if (!_hub.IsAlive) return;
        if (!(_hub.Target is IEventBusSystemHub hub)) return;

        var unsubscribeMethod = typeof(IEventBusSystemHub)
            .GetMethod("Unsubscribe", new Type[] { typeof(EventBusMessageSubscriptionToken) });
        if (unsubscribeMethod == null) return;

        unsubscribeMethod = unsubscribeMethod.MakeGenericMethod(_messageType);
        unsubscribeMethod.Invoke(hub, new object[] { this });
    }
}

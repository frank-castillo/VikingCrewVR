
/// <summary>
/// Message subscription interface
/// </summary>
public interface IEventBusMessageSubscription
{
    /// <summary>
    /// Token returned to reference this subscription
    /// </summary>
    EventBusMessageSubscriptionToken SubscriptionToken { get; }

    /// <summary>
    /// Whether delivery should be attempted.
    /// </summary>
    /// <param name="message">Message that may potentially be delivered.</param>
    /// <returns>True=> should send, False => should NOT send</returns>
    bool ShouldAttemptDelivery(IEventBusMessage message);

    /// <summary>
    /// Deliver the message
    /// </summary>
    /// <param name="message">Message to deliver</param>
    void Deliver(IEventBusMessage message);
}


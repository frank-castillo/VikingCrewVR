using System;

/// <summary>
/// An EventBusMessage to be broadcast by EventBusMessenger
/// </summary>
public interface IEventBusMessage
{
    /// <summary>
    /// The sender of the message, can be null if not supported by the implementation.
    /// </summary>
    object Sender { get; }
}

/// <summary>
/// Base class for messages that provides weak reference storage of the sender
/// </summary>
public abstract class EventBusMessageBase : IEventBusMessage
{
    /// <summary>
    /// Store a WeakReference to the sender in case the message is not disposed
    /// preventing the sender from being collected.
    /// </summary>
    private readonly WeakReference _sender;
    public object Sender => _sender?.Target;

    /// <summary>
    /// Initializes a new instance of the MessageBase class.
    /// </summary>
    /// <param name="sender">Message sender (usually "this")</param>
    public EventBusMessageBase(object sender)
    {
        if (sender == null)
        {
            throw new ArgumentNullException("sender");
        }
        _sender = new WeakReference(sender);
    }
}

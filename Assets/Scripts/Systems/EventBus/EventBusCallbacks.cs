using System;
using UnityEngine;

public class EventBusCallbacks
{
    private IEventBusSystemHub _messengerHub;
    // You can also setup events to invoke when a message is handled.
    public event Action<SimpleTextMessage> SimpleTextMessageHandled = null;
    public event Action<OnBeatInputMessage> OnBeatInputMessageHandled = null;

    public void Initialize()
    {
        Debug.Log("EventButCallbacks SystemStart");
        _messengerHub = ServiceLocator.Get<IEventBusSystemHub>();
        if (_messengerHub == null)
        {
            Debug.LogWarning("MessengerHub Not Found As Registered System");
            return;
        }

        RegisterSubscribers();
    }

    private void RegisterSubscribers()
    {
        _messengerHub.Subscribe<SimpleTextMessage>(HandleSimpleTextMessage);
        //_messengerHub.Subscribe<OnBeatInputMessage>(HandleOnBeatInputMessage);
    }

    private void HandleSimpleTextMessage(SimpleTextMessage obj)
    {
        Debug.Log($"Message Received: {obj.Message}");
        SimpleTextMessageHandled?.Invoke(obj);
    }

    //private void HandleOnBeatInputMessage(OnBeatInputMessage obj)
    //{
    //    OnBeatInputMessageHandled?.Invoke(obj);
    //}
}
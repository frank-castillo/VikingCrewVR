using UnityEngine;

public class EventBusSystem : MonoBehaviour
{
    IEventBusSystemHub _messenger = null;
    private EventBusCallbacks _eventBusCallbacks = null;

    public void Initialize()
    {
        // Hub should be created and registered first
        _messenger = new EventBusSystemHub();
        ServiceLocator.Register<IEventBusSystemHub>(_messenger);

        // Then setup callbacks
        _eventBusCallbacks = new EventBusCallbacks();
        _eventBusCallbacks.Initialize();

        // You can  register the EventBusCallbacks if you want to subscribe to any events raised when messages are handled
        ServiceLocator.Register<EventBusCallbacks>(_eventBusCallbacks);

        // Setting up message sender after the messenger hub and callbacks have been initialized and registered => Just demo
        //_sender.Initialize();
    }
}

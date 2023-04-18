using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] GameEvent _gameEvent;
    [SerializeField] UnityEvent _response;

    void Awake() => _gameEvent.Register(this);
  
    void OnDestroy() => _gameEvent.Deregister(this);

    public void RaiseEvent() => _response.Invoke();
}

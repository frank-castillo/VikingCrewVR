using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] GameEvent _gameEvent = null;
    [SerializeField] UnityEvent _response = null;

    void Awake() => _gameEvent.Register(this);
  
    void OnDestroy() => _gameEvent.Deregister(this);

    public void RaiseEvent() => _response.Invoke();
}

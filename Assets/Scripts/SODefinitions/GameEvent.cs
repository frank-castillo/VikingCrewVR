using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event", order = 52)]
public class GameEvent : ScriptableObject
{
    private HashSet<GameEventListener> _listeners = new HashSet<GameEventListener>();

    public void Invoke()
    {
        HashSet<GameEventListener>.Enumerator em = _listeners.GetEnumerator();
        while (em.MoveNext())
        {
            em.Current.RaiseEvent();
        }
    }

    public void Register(GameEventListener gameEventListener) => _listeners.Add(gameEventListener);

    public void Deregister(GameEventListener gameEventListener) => _listeners.Remove(gameEventListener);

}
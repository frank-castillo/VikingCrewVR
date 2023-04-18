using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<System.Type, object> _systemRegistry = new Dictionary<System.Type, object>();

    public static void Register<T>(object system, bool overwrite = false)
    {
        if (!_systemRegistry.ContainsKey(typeof(T)))
        {
            _systemRegistry.Add(typeof(T), system);
            return;
        }
        else // we already have one, see if we want to overwrite
        {
            if (overwrite)
            {
                _systemRegistry[typeof(T)] = system;
            }
        }
    }

    public static void Register(System.Type systemType, object system, bool overwrite = false)
    {
        if (!_systemRegistry.ContainsKey(systemType))
        {
            _systemRegistry.Add(systemType, system);
            return;
        }
        else // we already have one, see if we want to overwrite
        {
            if (overwrite)
            {
                _systemRegistry[systemType] = system;
            }
        }
    }

    static public T Get<T>()
    {
        object ret = default(T);
        _systemRegistry.TryGetValue(typeof(T), out ret);
        if (ret == null)
        {
            Debug.Log("Could not find [" + (typeof(T)) + "] as a registered system");
        }
        return (T)ret;
    }

    public static bool Contains<T>()
    {
        return _systemRegistry.ContainsKey(typeof(T));
    }
}

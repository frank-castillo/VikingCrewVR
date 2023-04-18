using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class MusicFire : MonoBehaviour
//{
//    private void Awake()
//    {
//        // Wait for Level Loader to finish initializing
//        //LevelLoader.CallOnComplete(Initialize);
//    }

//    private void OnDisable()
//    {
//        // Unsubscribe from Event Bus
//        var bus = ServiceLocator.Get<EventBusCallbacks>();
//        bus.OnMusicBeatMessageHandled -= OnBeat;
//    }

//    private void Initialize()
//    {
//        // Subscribe to Event bus
//        var bus = ServiceLocator.Get<EventBusCallbacks>();
//        bus.OnMusicBeatMessageHandled += OnBeat;
//    }

//    public void OnBeat(OnMusicBeatMessage message)
//    {
//    }
//}

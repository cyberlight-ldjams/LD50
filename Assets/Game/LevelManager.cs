using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{

    [Inject]
    private readonly SignalBus bus;

    // Start is called before the first frame update
    void Awake()
    {
        bus.Subscribe<DeathSignal>(onPlayerDeath);
        bus.Subscribe<WinSignal>(onLevelWon);
    }

    private void onLevelWon()
    {
        throw new NotImplementedException();
    }

    private void onPlayerDeath()
    {
        throw new NotImplementedException();
    }
}

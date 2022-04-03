using UnityEngine;

public struct DeathSignal : IEvent
{
    public GameObject killer;

    public string message;
}

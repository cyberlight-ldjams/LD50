using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SafeSpace : MonoBehaviour
{
    [Inject]
    private SignalBus sb;

    private bool isDead;

    void Awake()
    {
        isDead = false;
        sb.Subscribe<DeathSignal>(() => { isDead = true; });
        sb.Subscribe<WinSignal>((ws) => { Debug.Log(ws.message); });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDead)
        {
            sb.Fire<WinSignal>(new WinSignal() { message = "You escaped in " + Time.realtimeSinceStartup + " seconds!" });
        }
    }
}

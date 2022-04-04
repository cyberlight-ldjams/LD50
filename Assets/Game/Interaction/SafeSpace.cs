using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            SceneManager.LoadScene("Studio Backlot");
            
            //sb.Fire<WinSignal>(new WinSignal() { message = "You escaped in " + Time.realtimeSinceStartup + " seconds!" });
        }
    }
}

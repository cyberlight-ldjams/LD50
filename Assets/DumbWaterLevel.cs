using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DumbWaterLevel : MonoBehaviour
{
    public float speed = 0.3f;

    [Inject]
    private SignalBus sb;

    private bool won = false;

    // Update is called once per frame
    void Update()
    {
        if (!won && gameObject.transform.position.y > 27.1f)
        {
            sb.Fire<WinSignal>(new WinSignal() { message = "You escaped in " + Time.realtimeSinceStartup + " seconds!" });
            won = true;
        } else if (!won)
        {
            gameObject.transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }
}

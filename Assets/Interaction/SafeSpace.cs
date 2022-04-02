using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeSpace : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //TODO: Zenject hook in
        Debug.Log("You got to the chopper!");
    }
}

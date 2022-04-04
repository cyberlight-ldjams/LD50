using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Pushable : MonoBehaviour
{
    [Inject]
    private PlayerMovement pm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            pm.IsPushing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            pm.IsPushing = false;
        }
    }
}

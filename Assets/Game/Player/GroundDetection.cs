using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GroundDetection : MonoBehaviour
{
    [Inject]
    private PlayerMovement pm;

    private void OnTriggerEnter(Collider other)
    {
        pm.IsGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        pm.IsGrounded = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!pm.IsGrounded)
        {
            pm.IsGrounded = true;
        }
    }
}

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
        pm.IsGrounded = IsGrounded(other);
    }

    private void OnTriggerExit(Collider other)
    {
        pm.IsGrounded = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!pm.IsGrounded)
        {
            pm.IsGrounded = IsGrounded(other);
        }
    }

    private bool IsGrounded(Collider collider)
    {
        Vector3 point = collider.ClosestPointOnBounds(pm.transform.position);
        return (collider.bounds.center.y < point.y);
    }
}

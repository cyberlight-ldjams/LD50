using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        SetIdle();
    }

    public void SetLookRotation(Vector3 look)
    {
        gameObject.transform.rotation = Quaternion.LookRotation(new Vector3(look.x, 0, look.z));
    }

    private void SetAllFalse()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isClimbingLadder", false);
    }

    public void SetIdle()
    {
        SetAllFalse();
        animator.SetBool("isIdle", true);
    }

    public void SetRunning()
    {
        SetAllFalse();
        animator.SetBool("isRunning", true);
    }

    public bool IsRunning()
    {
        return animator.GetBool("isRunning");
    }

    public void SetClimbingLadder()
    {
        SetAllFalse();
        animator.SetBool("isClimbingLadder", true);
    }
}

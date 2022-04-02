using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingFence : MonoBehaviour
{
    [SerializeField]
    private FallingPole _fallingPole;

    [SerializeField]
    private GameObject _fenceRotatePoint;

    [SerializeField]
    private float _startFall = 0.6f;

    [SerializeField]
    private Vector3 _fallRotation;

    private Vector3 _startingPosition;

    private float _fallTime = 0;

    void Start()
    {
        _startingPosition = _fenceRotatePoint.transform.rotation.eulerAngles;
    }

    void Update()
    {
        if (_fallingPole.FallingTime * _fallingPole.FallingSpeed > _startFall && _fallTime < 1f) {
            _fallTime += Time.deltaTime;
            _fenceRotatePoint.transform.rotation = Quaternion.Slerp(Quaternion.Euler(_startingPosition), 
                Quaternion.Euler(_fallRotation), _fallTime);
        } else
        {
            return;
        }
    }
}

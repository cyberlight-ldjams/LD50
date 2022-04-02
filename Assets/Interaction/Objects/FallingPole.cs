using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FallingPole : MonoBehaviour
{
    [SerializeField]
    private Vector3 _startingRotation = Vector3.zero;

    [SerializeField]
    private Vector3 _endingRotation;

    [SerializeField]
    private float _fallingSpeed = 10.0f;

    [SerializeField]
    private bool _interactable;

    [SerializeField]
    private bool _interactedWith;

    // If the pole has already fallen over or not
    private bool _fallen;

    private float _fallingTime;

    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Interact.performed += ctx => OnInteraction();

        OnDisable();
    }

    // Start is called before the first frame update
    void Start()
    {
        _interactedWith = false;
        _interactable = false;
        _fallen = false;
        _fallingTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_interactedWith && !_fallen)
        {
            _fallingTime += Time.deltaTime;
            gameObject.transform.rotation = 
                Quaternion.Slerp(Quaternion.Euler(_startingRotation), 
                Quaternion.Euler(_endingRotation), _fallingTime * _fallingSpeed);
            if (gameObject.transform.rotation.Equals(Quaternion.Euler(_endingRotation)))
            {
                _fallen = true;
                InteractionZone iz = gameObject.GetComponentInChildren<InteractionZone>();
                Debug.Log(iz);
                if (iz != null)
                {
                    iz._notReady = false;
                }
            }
        }
    }

    //private static bool CloseRotation(Vector3 rotation, Vector3 check)
    //{
    //    if (rotation.x - check.x < 0.01f &&
    //        rotation.y - check.y < 0.01f &&
    //        rotation.z - check.z < 0.01f)
    //    {
    //        return true;
    //    } // else
    //    return false;
    //}

    private void OnTriggerEnter(Collider other)
    {
        _interactable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _interactable = false;
    }

    private void OnInteraction()
    {
        if (_interactable)
        {
            _interactedWith = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            _interactable = false;
        }
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}

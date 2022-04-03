using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class FallingPole : MonoBehaviour
{
    [SerializeField]
    private Vector3 _startingRotation = Vector3.zero;

    [SerializeField]
    private Vector3 _endingRotation;

    public float FallingSpeed = 0.5f;

    [SerializeField]
    private bool _interactable;

    [SerializeField]
    private bool _interactedWith;

    // If the pole has already fallen over or not
    public bool Fallen;

    public float FallingTime;

    [Inject]
    private readonly PlayerControls controls;

    void Awake()
    {
        controls.Player.Interact.performed += ctx => OnInteraction();
    }

    // Start is called before the first frame update
    void Start()
    {
        _interactedWith = false;
        _interactable = false;
        _startingRotation = gameObject.transform.rotation.eulerAngles;
        Fallen = false;
        FallingTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_interactedWith && !Fallen)
        {
            FallingTime += Time.deltaTime;
            gameObject.transform.rotation = 
                Quaternion.Slerp(Quaternion.Euler(_startingRotation), 
                Quaternion.Euler(_endingRotation), FallingTime * FallingSpeed);
            if (gameObject.transform.rotation.Equals(Quaternion.Euler(_endingRotation)))
            {
                Fallen = true;
                InteractionZone iz = gameObject.GetComponentInChildren<InteractionZone>();
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Ladder : MonoBehaviour
{
    [Inject]
    private PlayerMovement player;

    [Inject]
    private SignalBus sb;

    private PlayerControls controls;

    private bool _interactable;

    [SerializeField]
    private GameObject bottom;

    [SerializeField]
    private GameObject top;

    [SerializeField]
    private GameObject finish;

    // Start is called before the first frame update
    void Awake()
    {
        _interactable = false;

        controls = new PlayerControls();

        controls.Player.Interact.performed += ctx => OnInteraction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            sb.Fire<LadderInteractionSignal>(new LadderInteractionSignal() 
            { ladder=this.gameObject, ladderRailBottom=bottom.transform.position, 
                ladderRailTop=top.transform.position, ladderFinish=finish.transform.position });
        }
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }
}

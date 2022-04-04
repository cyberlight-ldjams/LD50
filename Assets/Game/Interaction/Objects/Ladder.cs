using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Ladder : MonoBehaviour
{ 
    [Inject]
    private SignalBus sb;

    [Inject]
    private PlayerControls controls;

    private bool _interactable;

    [SerializeField]
    private GameObject bottom;

    [SerializeField]
    private GameObject top;

    [SerializeField]
    private GameObject finish;

    [SerializeField]
    private GameObject getOff;

    // Start is called before the first frame update
    void Awake()
    {
        _interactable = false;

        controls.Player.Interact.performed += ctx => OnInteraction();
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
                ladderRailTop=top.transform.position, ladderFinish=finish.transform.position,
            getOffLadder=getOff.transform.position});
        }
    }
}

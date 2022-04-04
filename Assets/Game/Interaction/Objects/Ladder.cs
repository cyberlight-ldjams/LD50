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

    [Inject]
    private UI uiManager;

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
        if(!_interactable)
        {
            _interactable = true;
            uiManager.InputPrompt.Show(InputPrompt.Prompt.Interact);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        _interactable = false;
        uiManager.InputPrompt.Hide();
    }

    private void OnInteraction()
    {
        if (_interactable)
        {

            uiManager.InputPrompt.Hide();
            sb.Fire<LadderInteractionSignal>(new LadderInteractionSignal() 
            { ladder=this.gameObject, ladderRailBottom=bottom.transform.position, 
                ladderRailTop=top.transform.position, ladderFinish=finish.transform.position,
            getOffLadder=getOff.transform.position});
        }
    }
}

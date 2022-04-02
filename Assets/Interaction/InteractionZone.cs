using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    [SerializeField]
    private GameObject start;

    [SerializeField]
    private GameObject end;

    private bool _starting;

    private bool _ending;

    [SerializeField]
    private bool _inProgress;

    public bool _notReady;

    private TriggerPoint _startTriggerPoint;

    private TriggerPoint _endTriggerPoint;

    // Start is called before the first frame update
    void Start()
    {
        _startTriggerPoint = start.GetComponent<TriggerPoint>();
        _endTriggerPoint = end.GetComponent<TriggerPoint>();

        _starting = false;
        _ending = false;
        _inProgress = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_notReady)
        {
            return; //Do nothing
        }

        _starting = _startTriggerPoint.triggered;
        _ending = _endTriggerPoint.triggered;

        if (!_inProgress)
        {
            // If the player is in a starting zone, but has gone past/skipped the ending zone
            if (_starting && !_ending)
            {
                _inProgress = true;
            }
        } else // if (_inProgress)
        {
            // If the player is in an ending zone, but is outside of a starting zone
            if (!_starting && _ending)
            {
                _inProgress = false;
            }
        }
    }
}

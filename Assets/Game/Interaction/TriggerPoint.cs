using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TriggerPoint : MonoBehaviour
{
    public bool triggered;

    [Inject]
    private UI uiMamager;

    void Start()
    {
        triggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        triggered = true;
        if(other.gameObject.tag == "Player")
        {
            uiMamager.InputPrompt.Show(InputPrompt.Prompt.Interact);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        triggered = false;
        if (other.gameObject.tag == "Player")
        {
            uiMamager.InputPrompt.Hide();
        }
    }
}

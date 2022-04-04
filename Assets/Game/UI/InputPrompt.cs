using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InputPrompt : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 0.5f;

    [SerializeField]
    private Sprite interactionPrompt;

    private Image prompt;

    public enum Prompt
    {
        Interact
    }

    private void Start()
    {
        prompt = GetComponent<Image>();
    }

    public void Show(Prompt p)
    {
        bool success = true;
        switch(p)
        {
            case Prompt.Interact:
                prompt.sprite = interactionPrompt;
                break;
            default:
                Debug.LogWarning("No prompt available for this interaction");
                success = false;
                break;
        }

        if(success)
        {
            prompt.DOFade(1f, fadeDuration);
        }
    }

    public void Hide()
    {
        prompt.DOFade(0f, fadeDuration);
    }
}

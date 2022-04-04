using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public LevelEndScreen LevelEndScreen { get; private set; }
    public InputPrompt InputPrompt { get; private set; }

    public bool onLadder;

    private void Start()
    {
        LevelEndScreen = GetComponentInChildren<LevelEndScreen>();
        InputPrompt = GetComponentInChildren<InputPrompt>();
        onLadder = false;

    }
}

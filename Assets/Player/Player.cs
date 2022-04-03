using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement Movement { get; private set; }

    void Start()
    {
        Movement = GetComponent<PlayerMovement>();
    }
}

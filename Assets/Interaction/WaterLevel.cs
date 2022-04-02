using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevel : MonoBehaviour
{
    [SerializeField]
    private Vector3 _startingLevel;

    [SerializeField]
    private float _risingSpeed;

    //TODO: change to use Zenject
    [SerializeField]
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = _startingLevel;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(new Vector3(0, _risingSpeed * Time.deltaTime, 0));

        // If the water level is a unit above the player, they are of the dead,
        // because no can swimmy-swim
        if ((player.transform.position.y + 1.0f) < gameObject.transform.position.y)
        {
            //TODO: Fire death event with Zenject
            Debug.Log("I am of the dead! Bleh!");
        }
    }
}

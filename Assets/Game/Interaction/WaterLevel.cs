using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WaterLevel : MonoBehaviour
{
    [SerializeField]
    private Vector3 _startingLevel;

    [SerializeField]
    private float _risingSpeed;

    [Inject]
    private PlayerMovement player;

    [Inject]
    private SignalBus sb;

    private bool playerDead;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = _startingLevel;
        playerDead = false;
        sb.Subscribe<DeathSignal>(() => { playerDead = true; });
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(new Vector3(0, _risingSpeed * Time.deltaTime, 0));

        // If the water level is a unit above the player, they are of the dead,
        // because no can swimmy-swim
        if (!playerDead && (player.gameObject.transform.position.y + 1.0f) < gameObject.transform.position.y)
        {
            sb.Fire<DeathSignal>(new DeathSignal()
                { killer=this.gameObject, message="The player drowned!" });
            Debug.Log("Le dead");
        }
    }
}

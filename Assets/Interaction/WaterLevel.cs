using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevel : MonoBehaviour
{
    [SerializeField]
    private Vector3 _startingLevel;

    [SerializeField]
    private float _risingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = _startingLevel;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(new Vector3(0, _risingSpeed * Time.deltaTime, 0));
    }
}

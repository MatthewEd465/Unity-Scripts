using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingEffect : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private Vector3 startPosition;
    
    void Start()
    {
       startPosition = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}

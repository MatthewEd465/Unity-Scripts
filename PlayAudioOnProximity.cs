using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnProximity : MonoBehaviour
{

    public Transform player;
    public float triggerDistance = 10f;

    private AudioSource audioSource;

    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null || audioSource == null)
            return;

        Vector3 playerPos = player.position;
        Vector3 objectPos = transform.position;

        float distance = Vector3.Distance(playerPos, objectPos);
        
        if (distance <= triggerDistance && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        if (distance > triggerDistance && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

}

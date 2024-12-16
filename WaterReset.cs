using UnityEngine;
using System.Collections;

public class WaterReset : MonoBehaviour
{
    public Transform checkpoint;
    public PlayerController playerController;
    public AudioClip splashSound;
    private AudioSource audioSource;

    private void Start()
    {
        if (splashSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = splashSound;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.DisableCharacter();

            if (audioSource != null)
            {
                audioSource.Play();
            }

            other.transform.position = checkpoint.position;
            other.transform.rotation = checkpoint.rotation;

            StartCoroutine(ReEnablePlayer());
        }
    }

    private IEnumerator ReEnablePlayer()
    {
        yield return new WaitForSeconds(0.2f);
        playerController.EnableCharacter();
    }
}




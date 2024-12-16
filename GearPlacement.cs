using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearPlacement : MonoBehaviour
{
    public GameObject gearObject;
    public Transform gearPlacementPoint;
    public GameObject blockingRock;
    public AudioClip rockCrumbleSound;

    private bool playerInRange = false;
    private bool gearPlaced = false;
    private AudioSource audioSource;

    public Vector3 gearPlacedScale = Vector3.one;
    public Vector3 gearPlacedRotation = Vector3.zero;

    public GameObject placePromptText;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        placePromptText?.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && !gearPlaced && Input.GetKeyDown(KeyCode.E))
        {
            if (InventoryManager.Instance.HasItem("Gear"))
            {
                InventoryManager.Instance.RemoveItem("Gear");
                PlaceGear();
                PlayRockCrumbleSound();
                RemoveBlockingRock();
                gearPlaced = true;
                placePromptText?.SetActive(false);
            }
        }
    }

    private void PlaceGear()
    {
        
        if (gearObject != null && gearPlacementPoint != null)
        {
            gearObject.SetActive(true);
            gearObject.transform.position = gearPlacementPoint.position;
            gearObject.transform.rotation = Quaternion.Euler(gearPlacedRotation);
            gearObject.transform.localScale = gearPlacedScale;

            Collider gearCollider = gearObject.GetComponent<Collider>();
            if (gearCollider != null) gearCollider.enabled = false;

            LevelItem levelItem = gearObject.GetComponent<LevelItem>();
            if (levelItem != null) levelItem.enabled = false;

            
        }
        else
        {
            Debug.LogError("gearObject is null");
        }
    }

    private void PlayRockCrumbleSound()
    {
        if (audioSource == null)
        {
            
            return;
        }
        if (rockCrumbleSound == null)
        {
            
            return;
        }
        audioSource.PlayOneShot(rockCrumbleSound);
        
    }

    private void RemoveBlockingRock()
    {
        
        if (blockingRock != null)
        {
            blockingRock.SetActive(false);
            
        }
        else
        {
            Debug.LogWarning("blockingRock is null");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!gearPlaced) 
            {
                placePromptText?.SetActive(true);
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            placePromptText?.SetActive(false);
            
        }
    }
}



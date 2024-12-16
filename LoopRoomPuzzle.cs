using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoopRoomPuzzle : MonoBehaviour
{
    public GameObject[] pillars;
    public AudioClip completionSound;
    private AudioSource audioSource;

    private bool[] itemsPlaced;
    private int itemsPlacedCount = 0;
    private bool playerInRange = false;

    private string[] requiredItems = new string[] { "Tablet", "Skull", "Staff" };

    [Header("Rock Rotation Settings")]
    public Transform rockToRotate;
    public Vector3 targetRotation;
    public float rotationSpeed = 1f;
    private bool rockRotated = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        itemsPlaced = new bool[pillars.Length];
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < pillars.Length; i++)
            {
                if (!itemsPlaced[i] && InventoryManager.Instance.HasItem(requiredItems[i]))
                {
                    InventoryManager.Instance.RemoveItem(requiredItems[i]);
                    PlaceItem(i);
                    break;
                }
            }
        }
    }

    private void PlaceItem(int pillarIndex)
    {
        GameObject pillar = pillars[pillarIndex];
        Transform placementPoint = pillar.transform;

        itemsPlaced[pillarIndex] = true;
        itemsPlacedCount++;

        if (itemsPlacedCount == pillars.Length)
        {
            TriggerCompletion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void TriggerCompletion()
    {
        if (!rockRotated)
        {
            rockRotated = true;

            if (completionSound != null)
            {
                audioSource.PlayOneShot(completionSound);
            }

            StartCoroutine(RotateRock());
        }
    }

    private IEnumerator RotateRock()
    {
        Quaternion startRotation = rockToRotate.rotation;
        Quaternion endRotation = Quaternion.Euler(targetRotation);
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * rotationSpeed;
            rockToRotate.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime);
            yield return null;
        }

        rockToRotate.rotation = endRotation;
    }
}

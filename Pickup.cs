using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject pickUpText;
    public GameObject itemOnPlayer; // Optional: To enable objects on the player.

    [Header("Item Data")]
    public string itemName; // Name of the item to add to the inventory.

    private InventoryManager inventoryManager;

    void Start()
    {
        pickUpText.SetActive(false);

        // Find the InventoryManager in the scene.
        inventoryManager = FindObjectOfType<InventoryManager>();

        if (inventoryManager == null)
        {
            Debug.LogError("No InventoryManager found in the scene!");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pickUpText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E) && inventoryManager != null)
            {
                // Add the item to the inventory.
                inventoryManager.AddItem(itemName);

                // Activate item on the player if applicable.
                if (itemOnPlayer != null)
                {
                    itemOnPlayer.SetActive(true);
                }

                // Hide the pickup object and UI text.
                gameObject.SetActive(false);
                pickUpText.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pickUpText.SetActive(false);
        }
    }
}



using UnityEngine;

public class LevelItem : MonoBehaviour
{
    public int id;
    public string itemName;
    public bool isPickedUp = false;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventoryManager.AddItem(itemName);
            isPickedUp = true;
            gameObject.SetActive(false);
        }
    }
}




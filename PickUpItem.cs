using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public string itemName;
    public bool isPickedUp = false;

    public void PickUp()
    {
        if (!isPickedUp)
        {
            isPickedUp = true;
            gameObject.SetActive(false);
        }
    }

    public void ResetItem()
    {
        if (isPickedUp)
        {
            isPickedUp = false;
            gameObject.SetActive(true);
        }
    }

    public void TriggerPickUp()
    {
        PickUp();
    }
}






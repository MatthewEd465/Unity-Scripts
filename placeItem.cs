using UnityEngine;

public class PlaceGearPrompt : MonoBehaviour
{
    public GameObject aztecObject;
    public GameObject gearObject;
    public GameObject promptText;

    private bool isNearAztec = false;
    private bool hasGear = false;

    void Update()
    {
        if (isNearAztec && hasGear)
        {
            promptText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                PlaceGear();
            }
        }
        else
        {
            promptText.SetActive(false);
        }
    }

    private void PlaceGear()
    {
        promptText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == aztecObject)
        {
            isNearAztec = true;
        }

        if (gearObject != null)
        {
            hasGear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == aztecObject)
        {
            isNearAztec = false;
        }
    }
}



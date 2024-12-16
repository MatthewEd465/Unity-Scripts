using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inventoryPanel;
    public Image[] itemSlots; // Array of inventory slot images.
    public Slider fearBar; // Fear level bar.
    public Text objectiveText; // Current objective text.
    public GameObject fullInventoryMessage; // Message displayed when inventory is full.

    [Header("Player Data")]
    public List<string> collectedItems = new List<string>(); // Dynamically tracks collected items.
    public float playerFear = 0; // Player's fear level (0 to 1).
    public string currentObjective; // Current objective description.

    private bool isInventoryOpen = false;
    private AudioSource audioSource;
    public AudioClip inventoryOpenSound;
    public AudioClip inventoryCloseSound;
    public AudioClip itemPickupSound;

    [Header("Animation Settings")]
    public float fearBarAnimationSpeed = 0.5f; // Speed of fear bar animation.

    // Singleton instance
    public static InventoryManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Uncomment the following line if you need the InventoryManager to persist across scenes
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        inventoryPanel.SetActive(false);

        UpdateInventoryUI();
        UpdateFearBar(true);
        UpdateObjective();

        // Get AudioSource component.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add an AudioSource component if it doesn't exist
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);

        Time.timeScale = isInventoryOpen ? 0 : 1;
        Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isInventoryOpen;

        PlaySound(isInventoryOpen ? inventoryOpenSound : inventoryCloseSound);
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            // Check if the item slot at this index is valid before updating it
            if (itemSlots[i] != null)  // Make sure the slot is not null
            {
                if (i < collectedItems.Count)
                {
                    Sprite itemSprite = Resources.Load<Sprite>("Items/" + collectedItems[i]);
                    if (itemSprite != null)
                    {
                        itemSlots[i].sprite = itemSprite;
                        itemSlots[i].enabled = true;
                    }
                    else
                    {
                        Debug.LogWarning("Sprite not found for item: " + collectedItems[i]);
                    }
                }
                else
                {
                    itemSlots[i].sprite = null;
                    itemSlots[i].enabled = false;
                }
            }
            else
            {
                Debug.LogWarning("Item slot at index " + i + " is null or destroyed!");
            }
        }
    }

    public void UpdateFearBar(bool instant = false)
    {
        if (instant)
        {
            fearBar.value = playerFear;
        }
        else
        {
            StartCoroutine(AnimateFearBar());
        }
    }

    IEnumerator AnimateFearBar()
    {
        float initialValue = fearBar.value;
        float elapsedTime = 0;

        while (elapsedTime < fearBarAnimationSpeed)
        {
            fearBar.value = Mathf.Lerp(initialValue, playerFear, elapsedTime / fearBarAnimationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fearBar.value = playerFear;
    }

    public void UpdateObjective()
    {
        objectiveText.text = currentObjective;
    }

    public void AddItem(string itemName)
    {
        if (collectedItems.Count < itemSlots.Length)
        {
            collectedItems.Add(itemName);
            UpdateInventoryUI();
            audioSource.volume = 0.1f;
            PlaySound(itemPickupSound);
        }
        else
        {
            StartCoroutine(ShowFullInventoryMessage());
        }
    }

    public bool HasItem(string itemName)
    {
        return collectedItems.Contains(itemName);
    }

    public void RemoveItem(string itemName)
    {
        if (collectedItems.Contains(itemName))
        {
            collectedItems.Remove(itemName);
            UpdateInventoryUI();
        }
        else
        {
            Debug.LogWarning("Item " + itemName + " not found in inventory.");
        }
    }

    IEnumerator ShowFullInventoryMessage()
    {
        if (fullInventoryMessage != null)
        {
            fullInventoryMessage.SetActive(true);
            yield return new WaitForSeconds(2);
            fullInventoryMessage.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Full inventory message is not set in the inspector.");
        }
    }

    public void SetFearLevel(float fearLevel)
    {
        playerFear = Mathf.Clamp(fearLevel, 0, 1);
        UpdateFearBar();
    }

    public void SetObjective(string objective)
    {
        currentObjective = objective;
        UpdateObjective();
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}

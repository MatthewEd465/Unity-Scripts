using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public InventoryManager inventoryManager;
    public List<LevelItem> levelItems;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        levelItems = new List<LevelItem>(FindObjectsOfType<LevelItem>());
    }

    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            playerPositionX = player.transform.position.x,
            playerPositionY = player.transform.position.y,
            playerPositionZ = player.transform.position.z,
            
            collectedItems = new List<string>(inventoryManager.collectedItems),
            playerFear = inventoryManager.playerFear,
            currentObjective = inventoryManager.currentObjective
        };

        foreach (var item in levelItems)
        {
            if (item.isPickedUp)
            {
                data.pickedUpItemIDs.Add(item.id);
            }
        }

        string path = Application.persistentDataPath + "/saveData.dat";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, data);
        file.Close();
    }

    public SaveData LoadGame()
    {
        string path = Application.persistentDataPath + "/saveData.dat";
        if (!File.Exists(path)) return null;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);
        SaveData data = (SaveData)bf.Deserialize(file);
        file.Close();

        player = GameObject.FindGameObjectWithTag("Player");
        inventoryManager = FindObjectOfType<InventoryManager>();
        levelItems = new List<LevelItem>(FindObjectsOfType<LevelItem>());

        if (player == null || inventoryManager == null) return null;

        player.transform.position = new Vector3(data.playerPositionX, data.playerPositionY, data.playerPositionZ);
        inventoryManager.collectedItems = new List<string>(data.collectedItems);
        inventoryManager.UpdateInventoryUI();

        foreach (var item in levelItems)
        {
            if (data.pickedUpItemIDs.Contains(item.id))
            {
                item.gameObject.SetActive(false);
                item.isPickedUp = true;
            }
            else
            {
                item.gameObject.SetActive(true);
                item.isPickedUp = false;
            }
        }

        inventoryManager.playerFear = data.playerFear;
        inventoryManager.UpdateFearBar(true);
        inventoryManager.currentObjective = data.currentObjective;
        inventoryManager.UpdateObjective();

        return data;
    }


}









using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject mainMenu;

    private SaveData loadedData;

    public void StartNewGame()
    {
        SceneManager.LoadScene("ForestScene");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("MineScene");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MineScene")
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            loadedData = GameManager.instance.LoadGame();

            if (loadedData != null)
            {
                PlayerController player = FindObjectOfType<PlayerController>();
                InventoryManager inventory = FindObjectOfType<InventoryManager>();

                if (player != null)
                {
                    player.transform.position = new Vector3(
                        loadedData.playerPositionX,
                        loadedData.playerPositionY,
                        loadedData.playerPositionZ
                    );
                }

                if (inventory != null)
                {
                    inventory.collectedItems = new List<string>(loadedData.collectedItems);
                    inventory.UpdateInventoryUI();
                    inventory.playerFear = loadedData.playerFear;
                    inventory.UpdateFearBar(true);
                    inventory.currentObjective = loadedData.currentObjective;
                    inventory.UpdateObjective();
                }
            }
        }
    }


    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}




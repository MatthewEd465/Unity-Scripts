using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private GameObject pauseOverlay;
    [SerializeField] private MonoBehaviour cameraControlScript;

    private bool isGamePaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isGamePaused = true;
        pauseMenu.SetActive(true);
        confirmationPanel.SetActive(false);
        pauseOverlay.SetActive(true);
        if (cameraControlScript != null)
            cameraControlScript.enabled = false;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        pauseMenu.SetActive(false);
        confirmationPanel.SetActive(false);
        pauseOverlay.SetActive(false);
        if (cameraControlScript != null)
            cameraControlScript.enabled = true;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void BackToMenu()
    {
        pauseMenu.SetActive(false);
        confirmationPanel.SetActive(true);
        pauseOverlay.SetActive(true);
    }

    public void ConfirmBackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void CancelBackToMenu()
    {
        confirmationPanel.SetActive(false);
        pauseMenu.SetActive(true);
    }
}



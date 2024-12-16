using UnityEngine;

public class SavePanel : MonoBehaviour
{
    public void OnYesButtonClick()
    {
        GameManager.instance.SaveGame();
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        EnablePlayerControls();
    }

    public void OnNoButtonClick()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        EnablePlayerControls();
    }

    private void EnablePlayerControls()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}




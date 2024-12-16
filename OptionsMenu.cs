using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    public Slider volumeSlider;
    public Dropdown difficultyDropdown;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    
    void Start()
    {
        volumeSlider.value = AudioListener.volume;
        difficultyDropdown.value = PlayerPrefs.GetInt("Difficulty", 1);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetDifficulty(int difficulty)
    {
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }

    public void BackToMainMenu()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}

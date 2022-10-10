using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    GameManager GM;

    void Start()
    {
        GM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public void SaveData()
    {
        // Slider
        PlayerPrefs.SetFloat("PP_VolumeSlider", GM.volumeSlider.value);

        // Toggle
        if (GM.audioToggle.isOn)
        {
            PlayerPrefs.SetInt("PP_AudioToggle", 1);
        }
        else
        {
            PlayerPrefs.SetInt("PP_AudioToggle", 0);
        }

        if (GM.fsToggle.isOn)
        {
            PlayerPrefs.SetInt("PP_FullscreenToggle", 1);
        }
        else
        {
            PlayerPrefs.SetInt("PP_FullscreenToggle", 0);
        }

        // Dropdown
        PlayerPrefs.SetInt("PP_ResolutionDropdown", GM.resolutionDD.value);

        // Input Field
        PlayerPrefs.SetString("PP_Username", GM.usernameInput.text);
    }

    public void LoadData()
    {
        // Slider
        GM.volumeSlider.value = PlayerPrefs.GetFloat("PP_VolumeSlider", 1);

        // Toggle
        int pp_audiotoggle = PlayerPrefs.GetInt("PP_AudioToggle", 1);
        int pp_fullscreentoggle = PlayerPrefs.GetInt("PP_FullscreenToggle", 1);

        if (pp_audiotoggle == 1)
        {
            GM.audioToggle.isOn = true;
        }
        else
        {
            GM.audioToggle.isOn = false;
        }

        if (pp_fullscreentoggle == 1)
        {
            GM.fsToggle.isOn = true;
        }
        else
        {
            GM.fsToggle.isOn = false;
        }

        // Dropdown
        GM.resolutionDD.value = PlayerPrefs.GetInt("PP_ResolutionDropdown", 5);

        // Input Field
        GM.usernameInput.text = PlayerPrefs.GetString("PP_Username", "");
    }
}

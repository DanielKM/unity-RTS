using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPreferences : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider;
    private float volume;

    string userName = "traveller";
    // Start is called before the first frame update

    public void Awake() {
        LoadPlayerPreferences();
    }

    public void ApplyPlayerPreferences()
    {
        volume = volumeSlider.value;
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.SetString("username", userName);
        PlayerPrefs.Save();
    }

    public void LoadPlayerPreferences()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            volume = PlayerPrefs.GetFloat("volume");
            volumeSlider.value = PlayerPrefs.GetFloat("volume");
            userName = PlayerPrefs.GetString("username");
        }
        else
        {
            Debug.LogError("There is no save data!");
        }
    }
    
    public void ResetPlayerPreferences()
    {
        PlayerPrefs.DeleteAll();
        volume = 100.0f;
        volumeSlider.value = 100.0f;
        userName = "traveller";
    }
}

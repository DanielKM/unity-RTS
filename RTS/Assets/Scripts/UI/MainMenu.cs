using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Steamworks;

public class MainMenu : MonoBehaviour
{    
    public GameObject instructions;
    public string levelToLoad;

    public void LoadGame()
    {
        SaveLoad.load = true;
        levelToLoad = ES3.Load<string>("savedScene");
        Debug.Log("Loaded from main menu");
        SceneManager.LoadScene(levelToLoad);
    }

    public void QuitGame()
    {
        Debug.Log("Quit from main menu!");
        Application.Quit();
        Steamworks.SteamClient.Shutdown();
    }

    void OnDisable()
    {
        Debug.Log("PrintOnDisable: script was disabled");
        Steamworks.SteamClient.Shutdown();
    }
}

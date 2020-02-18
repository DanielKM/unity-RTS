using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Steamworks;

public class MainMenu : MonoBehaviour
{    
    public GameObject instructions;

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
        Steamworks.SteamClient.Shutdown();
    }

    void OnDisable()
    {
        Debug.Log("PrintOnDisable: script was disabled");
        Steamworks.SteamClient.Shutdown();
    }
}

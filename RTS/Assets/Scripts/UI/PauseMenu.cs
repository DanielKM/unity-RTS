using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool gamePaused = false;
    public GameObject pauseMenu;
    public GameObject optionsMenu;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F10) || Input.GetKeyDown(KeyCode.Escape))
        {
            if(gamePaused)
            {
                ResumeGame();
            } else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        gamePaused = true;
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        // Slow MO?
    }

    public void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        // Slow MO?
    }

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

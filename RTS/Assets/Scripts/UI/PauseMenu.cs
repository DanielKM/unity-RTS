using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool gamePaused = false;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject loadMenu;
    public GameObject saveMenu;

    public GameObject player;
    BuildingButtonController BBC;

    void Start() 
    {
        loadMenu = GameObject.Find("LoadMenu");
        saveMenu = GameObject.Find("SaveMenu");
        player = GameObject.FindGameObjectWithTag("Player");
        BBC = player.GetComponent<BuildingButtonController>();
    }

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
        loadMenu.SetActive(false);
        saveMenu.SetActive(false);
        // Slow MO?
    }

    public void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        loadMenu.SetActive(false);
        saveMenu.SetActive(false);
        // Slow MO?
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        ResumeGame();
        SceneManager.LoadScene("Main Menu");
    }

    void OnDisable()
    {
        Debug.Log("PrintOnDisable: script was disabled");
    }
}

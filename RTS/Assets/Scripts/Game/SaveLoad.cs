using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SaveLoad : MonoBehaviour
{
    GameObject GM;
    ResourceManager RM;
    GameObject faction; 
    public static bool load;
    public static string saveLocation;

    GameObject[] allGameObjects;
    List<GameObject> transferList = new List<GameObject>();
    GameObject[] savedGameObjects;
    GameObject[] loadedGameObjects;

    ES3WorkerPrefab ES3WorkerPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            ES3WorkerPrefab = GameObject.Find("ES3PrefabSaves").GetComponent<ES3WorkerPrefab>();
            GM = GameObject.Find("GameMenu");
            faction = GameObject.Find("Faction");
            RM = faction.GetComponent<ResourceManager>();

            if(load) {
                LoadGame(saveLocation);
            }
        }
    }
    
    public void ReloadGame() 
    {
        GM.GetComponent<PauseMenu>().ResumeGame();
        load = true;
        SceneManager.LoadScene("Level 1");
    }

    public void SaveGame(string saveLocation) 
    {
        string path = Application.persistentDataPath + "/" + saveLocation + ".es3";
        ES3WorkerPrefab.SaveWorkers(path);

        ES3.Save<float>("skymetal", RM.skymetal, path);
        ES3.Save<float>("iron", RM.iron, path);
        ES3.Save<float>("steel", RM.steel, path);
        ES3.Save<float>("stone", RM.stone, path);
        ES3.Save<float>("wood", RM.wood, path);
        ES3.Save<float>("food", RM.food, path);
        ES3.Save<float>("gold", RM.gold, path);

        ES3.Save<string>("savedScene", SceneManager.GetActiveScene().name, path);

        Debug.Log("Saved from in-game");
        GM.GetComponent<PauseMenu>().ResumeGame();
    }
    
    public void LoadGame(string saveLocation) 
    {   
        string path = Application.persistentDataPath + "/" + saveLocation + ".es3";
        string scene = ES3.Load<string>("savedScene", path);
        ES3WorkerPrefab.LoadWorkers(path);

        RM.skymetal = ES3.Load<float>("skymetal", path);
        RM.iron = ES3.Load<float>("iron", path);
        RM.steel = ES3.Load<float>("steel", path);
        RM.stone = ES3.Load<float>("stone", path);
        RM.wood = ES3.Load<float>("wood", path);
        RM.food = ES3.Load<float>("food", path);
        RM.gold = ES3.Load<float>("gold", path);

        Debug.Log("Loaded from in-game");
    }
}

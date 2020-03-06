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
    
    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
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
        Debug.Log(path);
        allGameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach(GameObject go in allGameObjects) 
        {
            if(go.GetComponent<UnitController>()) 
            {
                transferList.Add(go);
            } else if (go.GetComponent<BuildingController>())
            {
                transferList.Add(go);
            }
        }
        savedGameObjects = transferList.ToArray();

        ES3.Save<float>("skymetal", RM.skymetal, path);
        ES3.Save<float>("iron", RM.iron, path);
        ES3.Save<float>("steel", RM.steel, path);
        ES3.Save<float>("stone", RM.stone, path);
        ES3.Save<float>("wood", RM.wood, path);
        ES3.Save<float>("food", RM.food, path);
        ES3.Save<float>("gold", RM.gold, path);

        ES3.Save<string>("savedScene", SceneManager.GetActiveScene().name, path);
        ES3.Save<GameObject[]>("savedGameObjects", savedGameObjects, path);

        Debug.Log("Saved from in-game");
    }
    
    public void LoadGame(string saveLocation) 
    {   
        string path = Application.persistentDataPath + "/" + saveLocation + ".es3";
        RM.skymetal = ES3.Load<float>("skymetal", path);
        RM.iron = ES3.Load<float>("iron", path);
        RM.steel = ES3.Load<float>("steel", path);
        RM.stone = ES3.Load<float>("stone", path);
        RM.wood = ES3.Load<float>("wood", path);
        RM.food = ES3.Load<float>("food", path);
        RM.gold = ES3.Load<float>("gold", path);

        loadedGameObjects = ES3.Load<GameObject[]>("savedGameObjects", path);
        foreach(GameObject go in loadedGameObjects) 
        {
            if(go.GetComponent<UnitController>()) 
            {
                if(go.GetComponent<UnitController>().unitType == "Worker") 
                {
                    // Debug.Log(go.GetComponent<UnitSelection>().resources);
                    // Debug.Log(go.GetComponent<UnitSelection>().heldResource);
                    // Debug.Log(go.GetComponent<UnitSelection>().heldResourceType);
                    // Debug.Log(go.GetComponent<UnitSelection>().task);
                    // Debug.Log(go.GetComponent<UnitSelection>().targetNode);
                    // Debug.Log(go.GetComponent<NavMeshAgent>().destination);
                }
            }
        }
        // Time.timeScale = 1f;
        // GM.GetComponent<PauseMenu>().ResumeGame();
        Debug.Log("Loaded from in-game");
    }
}

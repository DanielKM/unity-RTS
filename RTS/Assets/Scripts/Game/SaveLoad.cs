using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ES3Types;

public class SaveLoad : MonoBehaviour
{
    ES3AutoSaveMgr autoSaveMgr;
    GameObject[] allGameObjects;
    List<GameObject> transferList = new List<GameObject>();


    GameObject[] savedGameObjects;

    // Start is called before the first frame update
    void Start()
    {
        autoSaveMgr = GameObject.Find("Easy Save 3 Manager").GetComponent<ES3AutoSaveMgr>();
    }

    public void SaveGame() 
    {
        allGameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach(GameObject go in allGameObjects) 
        {
            if(go.GetComponent<UnitController>() || go.GetComponent<BuildingController>()) 
            {
                transferList.Add(go);
            }
        }
        savedGameObjects = transferList.ToArray();
        ES3.Save<GameObject[]>("savedGameObjects", savedGameObjects);

        Debug.Log("Saved");
        // autoSaveMgr.Save();
    }
    
    public void LoadGame() 
    {
        // foreach(GameObject go in allGameObjects) 
        // {
        //     if(go.GetComponent<UnitController>() || go.GetComponent<BuildingController>()) 
        //     {
        //         Destroy(go);
        //     }
        // }
        ES3.Load<GameObject[]>("savedGameObjects");
        Debug.Log("Loaded");
        // autoSaveMgr.Load();
    }
}

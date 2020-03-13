using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;
/*
 * 
 * This class will manage the creation of prefabs, including loading and saving them.
 * It will also store a list of all of the prefabs we've created.
 * 
 */
public class ES3WorkerPrefab : MonoBehaviour 
{
    // The prefab we want to create.
    public GameObject workerPrefab;  
    public GameObject swordsmanPrefab; 
    public GameObject footmanPrefab; 
    public GameObject archerPrefab; 
    public GameObject outriderPrefab; 
    public GameObject knightPrefab; 
    public GameObject wizardPrefab; 

    // ENEMY PREFABS
    public GameObject banditSwordsmanPrefab; 
    public GameObject banditFootmanPrefab; 

    public NodeManager.ResourceTypes heldResourceType;
    public ActionList actions;

    // An automatically-generated unique identifier for this type of prefab.
    // We will append this to our keys when saving to identifiy different types
    // of prefab in the save file.
    public string id = System.Guid.NewGuid().ToString();
     
    // A List which we'll add the Transforms of our created prefabs to.
    private List<string> workerIDs = new List<string>();
    private List<string> swordsmanIDs = new List<string>();
    private List<string> footmanIDs = new List<string>();
    private List<string> archerIDs = new List<string>();
    private List<string> outriderIDs = new List<string>();
    private List<string> knightIDs = new List<string>();
    private List<string> wizardIDs = new List<string>();
    
    private List<string> banditSwordsmanIDs = new List<string>();
    private List<string> banditFootmanIDs = new List<string>();

    GameObject[] allGameObjects;
    List<GameObject> transferList = new List<GameObject>();
    GameObject[] savedGameObjects;
    GameObject[] loadedGameObjects;

    public Dictionary<string, bool> SavedDead = new Dictionary<string, bool>();
    public Dictionary<string, float> SavedHealth = new Dictionary<string, float>();
    public Dictionary<string, float> SavedArmour = new Dictionary<string, float>();
    public Dictionary<string, int> SavedHeldResource = new Dictionary<string, int>();
    public Dictionary<string, int> SavedKills = new Dictionary<string, int>();

    public Dictionary<string, string> SavedHeldResourceType = new Dictionary<string, string>();

    public Dictionary<string, string> SavedTask = new Dictionary<string, string>();
    public Dictionary<string, string> SavedName = new Dictionary<string, string>();
    public Dictionary<string, string> SavedRank = new Dictionary<string, string>();

    public Dictionary<string, Vector3> SavedPosition = new Dictionary<string, Vector3>();
    public Dictionary<string, Vector3> SavedDestination = new Dictionary<string, Vector3>();
    public Dictionary<string, Quaternion> SavedRotation = new Dictionary<string, Quaternion>();

    public Dictionary<string, GameObject> SavedTarget = new Dictionary<string, GameObject>();

    public Dictionary<string, GameObject> SavedFaction = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> SavedPlayer = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> SavedOwner = new Dictionary<string, GameObject>();

    public Dictionary<string, GameObject> SavedGameObject = new Dictionary<string, GameObject>();
    /*
    * This is called whenever the application is quit.
    * This is where we will save our data.
    */

    public void SaveWorkers(string saveLocation)
    {       
        SavedKills.Clear();
        SavedRank.Clear();
        SavedDead.Clear();
        SavedHealth.Clear();
        SavedArmour.Clear();
        SavedHeldResource.Clear();
        SavedHeldResourceType.Clear();
        SavedTask.Clear();
        SavedPosition.Clear();
        SavedDestination.Clear();
        SavedRotation.Clear();
        SavedTarget.Clear();
        SavedName.Clear();

        SavedFaction.Clear();
        SavedPlayer.Clear();
        SavedOwner.Clear();

        SavedGameObject.Clear();
        
        workerIDs.Clear();
        swordsmanIDs.Clear();
        footmanIDs.Clear();
        archerIDs.Clear();
        outriderIDs.Clear();
        knightIDs.Clear();
        wizardIDs.Clear();

        banditSwordsmanIDs.Clear();
        banditFootmanIDs.Clear();

        allGameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach(GameObject go in allGameObjects) 
        {
            if(go.GetComponent<UnitController>()) {
                if(go.GetComponent<UnitController>().unitType == "Worker") 
                {   
                    string unitID = go.GetComponent<UnitController>().unitID;
                    SaveInformation(unitID, go);
                    workerIDs.Add(go.GetComponent<UnitController>().unitID);
                } else if (go.GetComponent<UnitController>().unitType == "Swordsman"){
                    string unitID = go.GetComponent<UnitController>().unitID;
                    SaveInformation(unitID, go);
                    swordsmanIDs.Add(go.GetComponent<UnitController>().unitID);
                } else if (go.GetComponent<UnitController>().unitType == "Footman"){
                    string unitID = go.GetComponent<UnitController>().unitID;
                    SaveInformation(unitID, go);
                    footmanIDs.Add(go.GetComponent<UnitController>().unitID);
                } else if (go.GetComponent<UnitController>().unitType == "Archer"){
                    string unitID = go.GetComponent<UnitController>().unitID;
                    SaveInformation(unitID, go);
                    archerIDs.Add(go.GetComponent<UnitController>().unitID);
                } else if (go.GetComponent<UnitController>().unitType == "Outrider"){
                    string unitID = go.GetComponent<UnitController>().unitID;
                    SaveInformation(unitID, go);
                    outriderIDs.Add(go.GetComponent<UnitController>().unitID);
                } else if (go.GetComponent<UnitController>().unitType == "Knight"){
                    string unitID = go.GetComponent<UnitController>().unitID;
                    SaveInformation(unitID, go);
                    knightIDs.Add(go.GetComponent<UnitController>().unitID);
                } else if (go.GetComponent<UnitController>().unitType == "Wizard"){
                    string unitID = go.GetComponent<UnitController>().unitID;
                    SaveInformation(unitID, go);
                    wizardIDs.Add(go.GetComponent<UnitController>().unitID);
                } else if (go.GetComponent<UnitController>().unitType == "Bandit Swordsman"){
                    string unitID = go.GetComponent<UnitController>().unitID;
                    SaveInformation(unitID, go);
                    banditSwordsmanIDs.Add(go.GetComponent<UnitController>().unitID);
                } else if (go.GetComponent<UnitController>().unitType == "Bandit Footman"){
                    string unitID = go.GetComponent<UnitController>().unitID;
                    SaveInformation(unitID, go);
                    banditFootmanIDs.Add(go.GetComponent<UnitController>().unitID);
                }
            } else if (go.GetComponent<BuildingController>()) {
                string buildingID = go.GetComponent<BuildingController>().buildingID;
                SavedGameObject.Add(buildingID, go);
            }
        }

        ES3.Save<Dictionary<string, bool>>("DeathDictionary", SavedDead, saveLocation);
        ES3.Save<Dictionary<string, float>>("HealthDictionary", SavedHealth, saveLocation);
        ES3.Save<Dictionary<string, float>>("ArmourDictionary", SavedArmour, saveLocation);
        ES3.Save<Dictionary<string, int>>("HeldResourceDictionary", SavedHeldResource, saveLocation); 
        ES3.Save<Dictionary<string, int>>("KillsDictionary", SavedKills, saveLocation); 
        ES3.Save<Dictionary<string, string>>("HeldResourceTypeDictionary", SavedHeldResourceType, saveLocation); 
        ES3.Save<Dictionary<string, string>>("TaskDictionary", SavedTask, saveLocation);
        ES3.Save<Dictionary<string, string>>("NameDictionary", SavedName, saveLocation);
        ES3.Save<Dictionary<string, string>>("RankDictionary", SavedRank, saveLocation);
        ES3.Save<Dictionary<string, Vector3>>("PositionDictionary", SavedPosition, saveLocation);
        ES3.Save<Dictionary<string, Vector3>>("DestinationDictionary", SavedDestination, saveLocation);
        ES3.Save<Dictionary<string, Quaternion>>("RotationDictionary", SavedRotation, saveLocation);

        ES3.Save<Dictionary<string, GameObject>>("TargetDictionary", SavedTarget, saveLocation);

        ES3.Save<Dictionary<string, GameObject>>("FactionDictionary", SavedFaction, saveLocation);
        ES3.Save<Dictionary<string, GameObject>>("PlayerDictionary", SavedPlayer, saveLocation);
        ES3.Save<Dictionary<string, GameObject>>("OwnerDictionary", SavedOwner, saveLocation);

        ES3.Save<Dictionary<string, GameObject>>("GameObjectDictionary", SavedGameObject, saveLocation);

        // Save the number of created prefabs there are.
        ES3.Save<int>(id +"count", workerIDs.Count + swordsmanIDs.Count + footmanIDs.Count + archerIDs.Count + outriderIDs.Count + knightIDs.Count + wizardIDs.Count, saveLocation);
        
        // Save the references to the prefabs
        ES3.Save<List<string>>("workers", workerIDs, saveLocation);
        ES3.Save<List<string>>("swordsmen", swordsmanIDs, saveLocation);
        ES3.Save<List<string>>("footmen", footmanIDs, saveLocation);
        ES3.Save<List<string>>("archers", archerIDs, saveLocation);
        ES3.Save<List<string>>("outriders", outriderIDs, saveLocation);
        ES3.Save<List<string>>("knights", knightIDs, saveLocation);
        ES3.Save<List<string>>("wizards", wizardIDs, saveLocation);

        // Save enemy references
        ES3.Save<List<string>>("banditswordsmen", banditSwordsmanIDs, saveLocation);
        ES3.Save<List<string>>("banditfootmen", banditFootmanIDs, saveLocation);
    }

    public void SaveInformation(string unitID, GameObject go) {
        SavedRank.Add(unitID, go.GetComponent<UnitController>().unitRank);
        SavedKills.Add(unitID, go.GetComponent<UnitController>().unitKills);
        SavedDead.Add(unitID, go.GetComponent<UnitController>().isDead);
        SavedHealth.Add(unitID, go.GetComponent<UnitController>().health);
        SavedArmour.Add(unitID, go.GetComponent<UnitController>().armour);
        SavedHeldResource.Add(unitID, go.GetComponent<UnitSelection>().heldResource);
        SavedHeldResourceType.Add(unitID, go.GetComponent<UnitSelection>().heldResourceType.ToString());
        SavedTask.Add(unitID, go.GetComponent<UnitSelection>().task.ToString());
        SavedName.Add(unitID, go.GetComponent<UnitController>().unitName.ToString());
        SavedPosition.Add(unitID, go.transform.position);
        SavedDestination.Add(unitID, go.GetComponent<NavMeshAgent>().destination);
        SavedRotation.Add(unitID, go.transform.rotation);
        SavedTarget.Add(unitID, go.GetComponent<UnitSelection>().targetNode);

        SavedFaction.Add(unitID, go.GetComponent<UnitSelection>().team);
        SavedPlayer.Add(unitID, go.GetComponent<UnitSelection>().player);
        SavedOwner.Add(unitID, go.GetComponent<UnitSelection>().owner);

        SavedGameObject.Add(unitID, go);
    }

    public void LoadWorkers(string saveLocation) 
    {
        int count = ES3.Load<int>(id + "count", saveLocation);
        // If there are prefabs to load, load them.
        if(count > 0)
        {
            LoadInformation(saveLocation);
            allGameObjects = GameObject.FindObjectsOfType<GameObject>();
            for(int i = 0; i<allGameObjects.Length; i++) {
                if(allGameObjects[i].GetComponent<UnitController>()) {
                    if(allGameObjects[i].GetComponent<UnitController>().unitType == "Worker" || allGameObjects[i].GetComponent<UnitController>().unitType == "Bandit Swordsman" || allGameObjects[i].GetComponent<UnitController>().unitType == "Bandit Footman") 
                    {
                        Destroy(allGameObjects[i]);
                    } else if(allGameObjects[i].GetComponent<UnitController>().unitType == "Swordsman" || allGameObjects[i].GetComponent<UnitController>().unitType == "Footman" || allGameObjects[i].GetComponent<UnitController>().unitType == "Archer" || allGameObjects[i].GetComponent<UnitController>().unitType == "Outrider" || allGameObjects[i].GetComponent<UnitController>().unitType == "Knight" || allGameObjects[i].GetComponent<UnitController>().unitType == "Wizard"){
                        Destroy(allGameObjects[i]);
                    }
                } else if (allGameObjects[i].GetComponent<BuildingController>()) {

                }
            }

            // Save the references to the prefabs
            workerIDs = ES3.Load<List<string>>("workers", saveLocation);
            swordsmanIDs = ES3.Load<List<string>>("swordsmen", saveLocation);
            footmanIDs = ES3.Load<List<string>>("footmen", saveLocation);
            archerIDs = ES3.Load<List<string>>("archers", saveLocation);
            outriderIDs = ES3.Load<List<string>>("outriders", saveLocation);
            knightIDs = ES3.Load<List<string>>("knights", saveLocation);
            wizardIDs = ES3.Load<List<string>>("wizards", saveLocation);
            
            banditSwordsmanIDs = ES3.Load<List<string>>("banditswordsmen", saveLocation);
            banditFootmanIDs = ES3.Load<List<string>>("banditfootmen", saveLocation);

            // FRIENDLIES
            foreach(string id in workerIDs) {
                InstantiatePrefab(id, saveLocation, "Worker");
            }
            foreach(string id in swordsmanIDs) {
                InstantiatePrefab(id, saveLocation, "Swordsman");
            }
            foreach(string id in footmanIDs) {
                InstantiatePrefab(id, saveLocation, "Footman");
            }
            foreach(string id in archerIDs) {
                InstantiatePrefab(id, saveLocation, "Archer");
            }
            foreach(string id in outriderIDs) {
                InstantiatePrefab(id, saveLocation, "Outrider");
            }
            foreach(string id in knightIDs) {
                InstantiatePrefab(id, saveLocation, "Knight");
            }
            foreach(string id in wizardIDs) {
                InstantiatePrefab(id, saveLocation, "Wizard");
            }

            // ENEMIES
            foreach(string id in banditSwordsmanIDs) {
                InstantiatePrefab(id, saveLocation, "Bandit Swordsman");
            }
            foreach(string id in banditFootmanIDs) {
                InstantiatePrefab(id, saveLocation, "Bandit Footman");
            }

        }
    }

    public void LoadInformation(string saveLocation) {
        // For each prefab we want to load, instantiate a prefab.
        workerIDs.Clear();
        swordsmanIDs.Clear();
        footmanIDs.Clear();
        archerIDs.Clear();
        outriderIDs.Clear();
        knightIDs.Clear();
        wizardIDs.Clear();

        banditSwordsmanIDs.Clear();
        banditFootmanIDs.Clear();

        //Clear Dictionaries
        SavedRank.Clear();
        SavedKills.Clear();
        SavedDead.Clear();
        SavedHealth.Clear();
        SavedArmour.Clear();
        SavedHeldResource.Clear();
        SavedHeldResourceType.Clear();
        SavedTask.Clear();
        SavedName.Clear();
        SavedPosition.Clear();
        SavedDestination.Clear();
        SavedRotation.Clear();
        SavedTarget.Clear();

        SavedFaction.Clear();
        SavedPlayer.Clear();
        SavedOwner.Clear();

        SavedGameObject.Clear();

        //LOAD DICTIONARIES
        SavedRank = ES3.Load<Dictionary<string, string>>("RankDictionary", saveLocation);
        SavedKills = ES3.Load<Dictionary<string, int>>("KillsDictionary", saveLocation);
        SavedDead = ES3.Load<Dictionary<string, bool>>("DeathDictionary", saveLocation);
        SavedHealth = ES3.Load<Dictionary<string, float>>("HealthDictionary", saveLocation);
        SavedArmour = ES3.Load<Dictionary<string, float>>("ArmourDictionary", saveLocation);
        SavedHeldResource = ES3.Load<Dictionary<string, int>>("HeldResourceDictionary", saveLocation);
        SavedHeldResourceType = ES3.Load<Dictionary<string, string>>("HeldResourceTypeDictionary", saveLocation);
        SavedTask = ES3.Load<Dictionary<string, string>>("TaskDictionary", saveLocation);
        SavedName = ES3.Load<Dictionary<string, string>>("NameDictionary", saveLocation);
        SavedPosition = ES3.Load<Dictionary<string, Vector3>>("PositionDictionary", saveLocation);
        SavedDestination = ES3.Load<Dictionary<string, Vector3>>("DestinationDictionary", saveLocation);
        SavedRotation = ES3.Load<Dictionary<string, Quaternion>>("RotationDictionary", saveLocation);

        SavedTarget = ES3.Load<Dictionary<string, GameObject>>("TargetDictionary", saveLocation);

        SavedFaction = ES3.Load<Dictionary<string, GameObject>>("FactionDictionary", saveLocation);
        SavedPlayer = ES3.Load<Dictionary<string, GameObject>>("PlayerDictionary", saveLocation);
        SavedOwner = ES3.Load<Dictionary<string, GameObject>>("OwnerDictionary", saveLocation);

        SavedGameObject = ES3.Load<Dictionary<string, GameObject>>("GameObjectDictionary", saveLocation);
    }

    // INSTANTIATES PREFABS
    public GameObject InstantiatePrefab(string id, string saveLocation, string type)
    {
        Vector3 goPosition = SavedPosition[id];
        Quaternion goRotation = SavedRotation[id];

        GameObject go = workerPrefab;
        if(type == "Worker") {
            go = Instantiate(workerPrefab, goPosition, goRotation);
    
            go.GetComponent<UnitController>().unitKills = SavedKills[id];
            go.GetComponent<UnitController>().unitRank = SavedRank[id];
            go.GetComponent<UnitController>().unitID = id;
            go.GetComponent<UnitController>().isDead = SavedDead[id];
            go.GetComponent<NavMeshAgent>().destination = SavedDestination[id];
            go.GetComponent<UnitController>().health = SavedHealth[id];
            go.GetComponent<UnitController>().armour = SavedArmour[id];
            go.GetComponent<UnitSelection>().heldResource = SavedHeldResource[id];
            go.GetComponent<UnitSelection>().heldResourceType = (NodeManager.ResourceTypes) System.Enum.Parse (typeof(NodeManager.ResourceTypes), SavedHeldResourceType[id]);
            go.GetComponent<UnitSelection>().task = (ActionList) System.Enum.Parse (typeof(ActionList), SavedTask[id]);
            go.GetComponent<UnitController>().unitName =  SavedName[id];
            go.GetComponent<UnitSelection>().targetNode = SavedTarget[id];

            go.GetComponent<UnitSelection>().team = SavedFaction[id];
            go.GetComponent<UnitSelection>().player = SavedPlayer[id];
            go.GetComponent<UnitSelection>().owner = SavedOwner[id];

            return go;
        } else if(type == "Swordsman") {
            go = Instantiate(swordsmanPrefab, goPosition, goRotation);

            go.GetComponent<UnitController>().unitKills = SavedKills[id];
            go.GetComponent<UnitController>().unitRank = SavedRank[id];
            go.GetComponent<UnitController>().unitID = id;
            go.GetComponent<UnitController>().isDead = SavedDead[id];
            go.GetComponent<NavMeshAgent>().destination = SavedDestination[id];
            go.GetComponent<UnitController>().health = SavedHealth[id];
            go.GetComponent<UnitController>().armour = SavedArmour[id];
            go.GetComponent<UnitSelection>().heldResource = SavedHeldResource[id];
            go.GetComponent<UnitSelection>().heldResourceType = (NodeManager.ResourceTypes) System.Enum.Parse (typeof(NodeManager.ResourceTypes), SavedHeldResourceType[id]);
            go.GetComponent<UnitSelection>().task = (ActionList) System.Enum.Parse (typeof(ActionList), SavedTask[id]);
            go.GetComponent<UnitController>().unitName =  SavedName[id];
            go.GetComponent<UnitSelection>().targetNode = SavedTarget[id];
            go.GetComponent<UnitSelection>().team = SavedFaction[id];
            go.GetComponent<UnitSelection>().player = SavedPlayer[id];
            go.GetComponent<UnitSelection>().owner = SavedOwner[id];

        return go;
        } else if(type == "Footman") {
            go = Instantiate(footmanPrefab, goPosition, goRotation);

            go.GetComponent<UnitController>().unitKills = SavedKills[id];
            go.GetComponent<UnitController>().unitRank = SavedRank[id];
            go.GetComponent<UnitController>().unitID = id;
            go.GetComponent<UnitController>().isDead = SavedDead[id];
            go.GetComponent<NavMeshAgent>().destination = SavedDestination[id];
            go.GetComponent<UnitController>().health = SavedHealth[id];
            go.GetComponent<UnitController>().armour = SavedArmour[id];
            go.GetComponent<UnitSelection>().heldResource = SavedHeldResource[id];
            go.GetComponent<UnitSelection>().heldResourceType = (NodeManager.ResourceTypes) System.Enum.Parse (typeof(NodeManager.ResourceTypes), SavedHeldResourceType[id]);
            go.GetComponent<UnitSelection>().task = (ActionList) System.Enum.Parse (typeof(ActionList), SavedTask[id]);
            go.GetComponent<UnitController>().unitName =  SavedName[id];
            go.GetComponent<UnitSelection>().targetNode = SavedTarget[id];

            go.GetComponent<UnitSelection>().team = SavedFaction[id];
            go.GetComponent<UnitSelection>().player = SavedPlayer[id];
            go.GetComponent<UnitSelection>().owner = SavedOwner[id];

            return go;
        } else if(type == "Archer") {
            go = Instantiate(archerPrefab, goPosition, goRotation);

            go.GetComponent<UnitController>().unitKills = SavedKills[id];
            go.GetComponent<UnitController>().unitRank = SavedRank[id];
            go.GetComponent<UnitController>().unitID = id;
            go.GetComponent<UnitController>().isDead = SavedDead[id];
            go.GetComponent<NavMeshAgent>().destination = SavedDestination[id];
            go.GetComponent<UnitController>().health = SavedHealth[id];
            go.GetComponent<UnitController>().armour = SavedArmour[id];
            go.GetComponent<UnitSelection>().heldResource = SavedHeldResource[id];
            go.GetComponent<UnitSelection>().heldResourceType = (NodeManager.ResourceTypes) System.Enum.Parse (typeof(NodeManager.ResourceTypes), SavedHeldResourceType[id]);
            go.GetComponent<UnitSelection>().task = (ActionList) System.Enum.Parse (typeof(ActionList), SavedTask[id]);
            go.GetComponent<UnitController>().unitName =  SavedName[id];
            go.GetComponent<UnitSelection>().targetNode = SavedTarget[id];

            go.GetComponent<UnitSelection>().team = SavedFaction[id];
            go.GetComponent<UnitSelection>().player = SavedPlayer[id];
            go.GetComponent<UnitSelection>().owner = SavedOwner[id];

            return go;
        } else if(type == "Outrider") {
            go = Instantiate(outriderPrefab, goPosition, goRotation);

            go.GetComponent<UnitController>().unitKills = SavedKills[id];
            go.GetComponent<UnitController>().unitRank = SavedRank[id];
            go.GetComponent<UnitController>().unitID = id;
            go.GetComponent<UnitController>().isDead = SavedDead[id];
            go.GetComponent<NavMeshAgent>().destination = SavedDestination[id];
            go.GetComponent<UnitController>().health = SavedHealth[id];
            go.GetComponent<UnitController>().armour = SavedArmour[id];
            go.GetComponent<UnitSelection>().heldResource = SavedHeldResource[id];
            go.GetComponent<UnitSelection>().heldResourceType = (NodeManager.ResourceTypes) System.Enum.Parse (typeof(NodeManager.ResourceTypes), SavedHeldResourceType[id]);
            go.GetComponent<UnitSelection>().task = (ActionList) System.Enum.Parse (typeof(ActionList), SavedTask[id]);
            go.GetComponent<UnitController>().unitName =  SavedName[id];
            go.GetComponent<UnitSelection>().targetNode = SavedTarget[id];

            go.GetComponent<UnitSelection>().team = SavedFaction[id];
            go.GetComponent<UnitSelection>().player = SavedPlayer[id];
            go.GetComponent<UnitSelection>().owner = SavedOwner[id];

            return go;
        } else if(type == "Knight") {
            go = Instantiate(knightPrefab, goPosition, goRotation);

            go.GetComponent<UnitController>().unitKills = SavedKills[id];
            go.GetComponent<UnitController>().unitRank = SavedRank[id];
            go.GetComponent<UnitController>().unitID = id;
            go.GetComponent<UnitController>().isDead = SavedDead[id];
            go.GetComponent<NavMeshAgent>().destination = SavedDestination[id];
            go.GetComponent<UnitController>().health = SavedHealth[id];
            go.GetComponent<UnitController>().armour = SavedArmour[id];
            go.GetComponent<UnitSelection>().heldResource = SavedHeldResource[id];
            go.GetComponent<UnitSelection>().heldResourceType = (NodeManager.ResourceTypes) System.Enum.Parse (typeof(NodeManager.ResourceTypes), SavedHeldResourceType[id]);
            go.GetComponent<UnitSelection>().task = (ActionList) System.Enum.Parse (typeof(ActionList), SavedTask[id]);
            go.GetComponent<UnitController>().unitName =  SavedName[id];
            go.GetComponent<UnitSelection>().targetNode = SavedTarget[id];

            go.GetComponent<UnitSelection>().team = SavedFaction[id];
            go.GetComponent<UnitSelection>().player = SavedPlayer[id];
            go.GetComponent<UnitSelection>().owner = SavedOwner[id];

            return go;
        } else if(type == "Wizard") {
            go = Instantiate(wizardPrefab, goPosition, goRotation);

            go.GetComponent<UnitController>().unitKills = SavedKills[id];
            go.GetComponent<UnitController>().unitRank = SavedRank[id];
            go.GetComponent<UnitController>().unitID = id;
            go.GetComponent<UnitController>().isDead = SavedDead[id];
            go.GetComponent<NavMeshAgent>().destination = SavedDestination[id];
            go.GetComponent<UnitController>().health = SavedHealth[id];
            go.GetComponent<UnitController>().armour = SavedArmour[id];
            go.GetComponent<UnitSelection>().heldResource = SavedHeldResource[id];
            go.GetComponent<UnitSelection>().heldResourceType = (NodeManager.ResourceTypes) System.Enum.Parse (typeof(NodeManager.ResourceTypes), SavedHeldResourceType[id]);
            go.GetComponent<UnitSelection>().task = (ActionList) System.Enum.Parse (typeof(ActionList), SavedTask[id]);
            go.GetComponent<UnitController>().unitName =  SavedName[id];
            go.GetComponent<UnitSelection>().targetNode = SavedTarget[id];

            go.GetComponent<UnitSelection>().team = SavedFaction[id];
            go.GetComponent<UnitSelection>().player = SavedPlayer[id];
            go.GetComponent<UnitSelection>().owner = SavedOwner[id];

        return go;
        } else if(type == "Bandit Swordsman") {
            go = Instantiate(banditSwordsmanPrefab, goPosition, goRotation);

            go.GetComponent<UnitController>().unitKills = SavedKills[id];
            go.GetComponent<UnitController>().unitRank = SavedRank[id];
            go.GetComponent<UnitController>().unitID = id;
            go.GetComponent<UnitController>().isDead = SavedDead[id];
            go.GetComponent<NavMeshAgent>().destination = SavedDestination[id];
            go.GetComponent<UnitController>().health = SavedHealth[id];
            go.GetComponent<UnitController>().armour = SavedArmour[id];
            go.GetComponent<UnitSelection>().heldResource = SavedHeldResource[id];
            go.GetComponent<UnitSelection>().heldResourceType = (NodeManager.ResourceTypes) System.Enum.Parse (typeof(NodeManager.ResourceTypes), SavedHeldResourceType[id]);
            go.GetComponent<UnitSelection>().task = (ActionList) System.Enum.Parse (typeof(ActionList), SavedTask[id]);
            go.GetComponent<UnitController>().unitName =  SavedName[id];
            go.GetComponent<UnitSelection>().targetNode = SavedTarget[id];

            go.GetComponent<UnitSelection>().team = SavedFaction[id];
            go.GetComponent<UnitSelection>().player = SavedPlayer[id];
            go.GetComponent<UnitSelection>().owner = SavedOwner[id];

        return go;
        } else if(type == "Bandit Footman") {
            go = Instantiate(banditFootmanPrefab, goPosition, goRotation);

            go.GetComponent<UnitController>().unitKills = SavedKills[id];
            go.GetComponent<UnitController>().unitRank = SavedRank[id];
            go.GetComponent<UnitController>().unitID = id;
            go.GetComponent<UnitController>().isDead = SavedDead[id];
            go.GetComponent<NavMeshAgent>().destination = SavedDestination[id];
            go.GetComponent<UnitController>().health = SavedHealth[id];
            go.GetComponent<UnitController>().armour = SavedArmour[id];
            go.GetComponent<UnitSelection>().heldResource = SavedHeldResource[id];
            go.GetComponent<UnitSelection>().heldResourceType = (NodeManager.ResourceTypes) System.Enum.Parse (typeof(NodeManager.ResourceTypes), SavedHeldResourceType[id]);
            go.GetComponent<UnitSelection>().task = (ActionList) System.Enum.Parse (typeof(ActionList), SavedTask[id]);
            go.GetComponent<UnitController>().unitName =  SavedName[id];
            go.GetComponent<UnitSelection>().targetNode = SavedTarget[id];
            go.GetComponent<UnitSelection>().team = SavedFaction[id];
            go.GetComponent<UnitSelection>().player = SavedPlayer[id];
            go.GetComponent<UnitSelection>().owner = SavedOwner[id];

        return go;
        }
        return go;
    }
}
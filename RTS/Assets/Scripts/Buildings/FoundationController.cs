using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoundationController : MonoBehaviour
{
    public GameObject buildingPrefab;
    BuildingController buildingScript;

    public bool isBuilding;
    public int buildTime;
    public int buildPercent;
    public List<GameObject> builderList = new List<GameObject>();

    private Vector3 location;

    private CanvasGroup BuildingProgressPanel;
    private GameObject currentBuilding;

    public ResourceManager RM;
    ResearchController RC;
    
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject team = GameObject.Find("Faction");
        RM = team.GetComponent<ResourceManager>();
        RC = team.GetComponent<ResearchController>();
        BuildingProgressPanel = GameObject.Find("BuildingProgressPanel").GetComponent<CanvasGroup>();
        StartCoroutine(BuildTick());
    }

    void Update()
    {
        if (buildPercent >= 100)
        {
            buildingScript = buildingPrefab.GetComponent<BuildingController>();
            if (buildingScript.unitType == "Farm")
            {
                location = new Vector3(gameObject.transform.position.x -4.0f, gameObject.transform.position.y, gameObject.transform.position.z);
            } else
            {
                location = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            }

            Destroy(gameObject);
            //adjusted placement rotation for odd models
            currentBuilding = Instantiate(buildingPrefab, location, Quaternion.identity);
            currentBuilding.transform.rotation = gameObject.transform.rotation;

            // ADD COUNTS TO RESOURCE MANAGER
            if (buildingScript.unitType == "House") {
                RM.houseCount += 1;
            } else if(buildingScript.unitType == "Farm") {
                RM.farmCount += 1;
            } else if(buildingScript.unitType == "Town Hall") {
                RM.townHallCount += 1;
            } else if(buildingScript.unitType == "Barracks") {
                RM.barracksCount += 1;
            } else if(buildingScript.unitType == "Lumber Yard") {
                RM.lumberYardCount += 1;
            } else if(buildingScript.unitType == "Stables") {
                RM.stablesCount += 1;
            } else if(buildingScript.unitType == "Blacksmith") {
                RM.blacksmithCount += 1;
            }
            currentBuilding.layer = 11;
            isBuilding = false;
        } else {
            isBuilding = true;
        }
    }

    public void BuildStructure()
    {       
        int toolModifier;
        if(RC.artisanToolSmithing) {
            toolModifier = 3;
        } else if (RC.basicToolSmithing) {
            toolModifier = 2;
        } else {
            toolModifier = 1;
        }

        if (builderList.Count > 0)
        {
            isBuilding = true;
            buildPercent += builderList.Count * 5 * toolModifier;
        } else
        {
            isBuilding = false;
        }

        if(buildPercent >= 100)
        {
            BuildingProgressPanel.blocksRaycasts = false;
            BuildingProgressPanel.interactable = false;
            BuildingProgressPanel.alpha = 0;
        }
    }
       
    IEnumerator BuildTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(buildTime);
            BuildStructure();
        }
    }
}

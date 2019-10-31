﻿using System.Collections;
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
    public int builders;
    private Vector3 location;

    private CanvasGroup BuildingProgressPanel;
    private GameObject currentBuilding;

    public ResourceManager RM;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        RM = player.GetComponent<ResourceManager>();

        // Starts the resource tick (means its true)
        BuildingProgressPanel = GameObject.Find("BuildingProgressPanel").GetComponent<CanvasGroup>();
        StartCoroutine(BuildTick());
    }

    // Update is called once per frame
    void Update()
    {
        if (buildPercent >= 100)
        {
            buildingScript = buildingPrefab.GetComponent<BuildingController>();
            // Need to add isGathering = false
            if(buildingScript.unitType == "House")
            {
                location = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.2f, gameObject.transform.position.z);
            }
            else if (buildingScript.unitType == "Farm")
            {
                location = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            } else
            {
                location = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            }

            Destroy(gameObject);
            //adjusted placement rotation for odd models
            currentBuilding = Instantiate(buildingPrefab, location, Quaternion.identity);
             if (buildingScript.unitType == "Stables" || buildingScript.unitType == "Barracks")
            {
                currentBuilding.transform.Rotate(0, 270, 0);
            }
            //adjusted placement location
            if (buildingScript.unitType == "Stables")
            {
                Vector3 newLocation = new Vector3(currentBuilding.transform.position.x, currentBuilding.transform.position.y, currentBuilding.transform.position.z - 2.5f);
                currentBuilding.transform.position = newLocation;
            }
            if(buildingScript.unitType == "Town Hall") {
                RM.townHallCount += 1;
            }
            currentBuilding.layer = 11;
            isBuilding = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Selectable" && collision.gameObject)
        {

        }
    }

    // Ticks down while villager is gathering resource - Adjust with heldResource in GatherTick in Selection Script
    public void BuildStructure()
    {
        if (builders != 0)
        {
            isBuilding = true;
            buildPercent += builders * 5;
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

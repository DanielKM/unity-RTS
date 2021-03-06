﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UnitSelection : MonoBehaviour
{
     // Set Resource
    public NodeManager.ResourceTypes heldResourceType;
        
    public ActionList task;
    private ResourceManager RM;
    private AudioSource unitAudio;
    public AudioClip unitMoveClip;
    // Player 
    public GameObject team;
    public GameObject player;
    public GameObject owner;
    private AudioSource playerAudio;
    public int mask;

    // Grab nodemanager script and harvest speed on gameobject
    FoundationController buildScript;
    float buildSpeed;
    NodeManager harvestScript;
    InputManager IM;
    UnitController UC;
    ResearchController RC;
    UnitSelection targetScript;
    BuildingController buildingScript;
    float harvestSpeed;

    // villager target node
    public GameObject targetNode;

    // selection variables
    public bool selected = false;

    // shows if villager is gathering
    public bool isGathering;
    public bool isBuilding;
    public bool isFollowing;
    public bool isAttacking;
    public bool isMeleeing;

    private NavMeshAgent agent;

    // Number of held resources
    public int heldResource;
    public int maxHeldResource;

    public GameObject[] drops;
    public GameObject[] resources;

    public List<GameObject> formationList;
    public List<GameObject> dropList;

    public GameObject rightClickCursor;
    private GameObject currentRightClickCursor;
    PauseMenu PM;
    public GameObject game;

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            game = GameObject.Find("Game");

            if(game.GetComponent<SaveLoad>().loadedPlayer) {
                mask =~ LayerMask.GetMask("FogOfWar");
                team = GameObject.Find("Faction");
                player = game.GetComponent<SaveLoad>().loadedPlayer;
                playerAudio = player.GetComponentInChildren<AudioSource>();
                RM = team.GetComponent<ResourceManager>();
                RC = team.GetComponent<ResearchController>();
                IM = player.GetComponent<InputManager>();
                UC = GetComponent<UnitController>();
                agent = GetComponent<NavMeshAgent>();
                PM = GameObject.Find("GameMenu").GetComponent<PauseMenu>();
                rightClickCursor = IM.cursorHit;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            if(UC.unitType == "Worker" && !UC.isDead) {
                // if target node is destroyed
                if (targetNode == null && task == ActionList.Gathering)
                {
                    if (heldResource >= maxHeldResource)
                    {
                        isBuilding = false;
                        isGathering = false;
                        drops = GameObject.FindGameObjectsWithTag("Player 1");
                        drops = AdjustDrops(drops);
                        if(drops.Length > 0 && task != ActionList.Idle && task != ActionList.Moving )
                        {
                            task = ActionList.Delivering;
                            agent.destination = GetClosestDropOff(drops).transform.position;
                        } else
                        {
                            task = ActionList.Idle;
                        }
                    } else
                    {
                        resources = GameObject.FindGameObjectsWithTag("Resource");
                        targetNode = GetClosestResource(resources);
                        if(targetNode) {
                            agent.destination = targetNode.transform.position;
                        } else {
                            task = ActionList.Idle;
                        }
                    }
                } else
                {
                    if(targetNode == null && isBuilding) {
                        task = ActionList.Idle;
                        isBuilding = false;
                    }
                    
                    if (heldResource >= maxHeldResource)
                    {
                        if(targetNode.GetComponent<BuildingController>()) {
                             if(targetNode.GetComponent<BuildingController>().unitType != "Blacksmith") {
                                isGathering = false;
                                drops = GameObject.FindGameObjectsWithTag("Player 1");
                                drops = AdjustDrops(drops);
                                if(drops.Length > 0 && task != ActionList.Idle && task != ActionList.Moving ) 
                                {
                                    task = ActionList.Delivering;
                                    agent.destination = GetClosestDropOff(drops).transform.position;
                                    drops = null;
                                }
                                else
                                {
                                    task = ActionList.Idle;
                                }
                             }
                        }
                    } 
                }
            }
            if (Input.GetMouseButtonDown(1) && selected == true)
            {
                if (!EventSystem.current.IsPointerOverGameObject(-1))
                {   
                    RightClick();
                }
            }
            if (Input.GetMouseButtonUp(1) && selected == true) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 350, mask))
                {
                    StartCoroutine(ShowDestinations());
                }    
            }

            if (task == ActionList.Moving) {
                if(agent.GetComponent<UnitController>() && agent.destination.x - agent.transform.position.x < 0.4 && agent.destination.z - agent.transform.position.z < 0.4) {
                    task = ActionList.Idle;
                }
            }
        }
    }

    public GameObject[] AdjustDrops(GameObject[] newDrops) {
        dropList.Clear();
        if(heldResourceType == NodeManager.ResourceTypes.Wood) {
            foreach(GameObject drop in newDrops) {
                buildingScript = drop.GetComponent<BuildingController>();
                if(buildingScript.unitType  == "Town Hall" || buildingScript.unitType == "Lumber Yard") {
                    dropList.Add(drop);
                }
            }
        } else {
            foreach(GameObject drop in newDrops) {
                buildingScript = drop.GetComponent<BuildingController>();
                if(buildingScript.unitType  == "Town Hall") {
                    dropList.Add(drop);
                }
            }
        }
        newDrops = dropList.ToArray();
        return newDrops;
    }

    public GameObject GetClosestDropOff(GameObject[] dropOffs)
    {
        GameObject closestDrop = null;
        float closestDropDistance = Mathf.Infinity;
        Vector3 dropoffPosition = transform.position;

        foreach(GameObject targetDrop in dropOffs)
        {
            Vector3 direction = targetDrop.transform.position - dropoffPosition;
            float dropDistance = direction.sqrMagnitude;
            if(dropDistance < closestDropDistance)
            {
                closestDropDistance = dropDistance;
                closestDrop = targetDrop;
            }
        }
        return closestDrop;
    }

    GameObject GetClosestResource(GameObject[] resources)
    {
        GameObject closestResource = null;
        float closestResourceDistance = Mathf.Infinity;
        Vector3 resourcePosition = transform.position;

        foreach(GameObject targetResource in resources)
        {
            NodeManager currentNode = targetResource.GetComponent<NodeManager>();
            if(currentNode.resourceType == heldResourceType) {
                Vector3 direction = targetResource.transform.position - resourcePosition;
                float resourceDistance = direction.sqrMagnitude;
                if(resourceDistance < closestResourceDistance)
                {
                    closestResourceDistance = resourceDistance;
                    closestResource = targetResource;
                }
            }

        }
        return closestResource;
    }

    // Right click function
    public void RightClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        isFollowing = false;

        if (Physics.Raycast(ray, out hit, 350, mask))
        {   
            targetNode = hit.collider.gameObject;
            targetScript = targetNode.GetComponent<UnitSelection>();
            if(owner == team && !UC.isDead) {     
                if(UC.unitType == "Worker") {
                    gameObject.GetComponent<NavMeshAgent>().isStopped = false;

                    if (hit.collider.tag != "Player 1")
                    {
                        if(targetScript != null) {
                            agent.destination = hit.collider.gameObject.transform.position;
                            isFollowing = true;
                            StartCoroutine(Follow());
                        } else if (hit.collider.tag == "Ground")
                        {
                            isBuilding = false;
                            isGathering = false;
                            isMeleeing = false;
                            if(IM.attackMoving) {
                                task = ActionList.Attacking;
                            } else {
                                task = ActionList.Moving;
                            }
                            CreateBoxFormation(hit);
                        }
                        else if (hit.collider.tag == "Resource")
                        {
                            isBuilding = false;
                            isMeleeing = false;
                            task = ActionList.Gathering;
                            agent.destination = hit.collider.gameObject.transform.position;
                            targetNode = hit.collider.gameObject;
                        }
                        else if (hit.collider.tag == "Blacksmith")
                        {
                            targetNode = hit.collider.gameObject;
                            if(hit.collider.gameObject.GetComponent<BuildingController>().unitType == "Blacksmith") {
                                drops = GameObject.FindGameObjectsWithTag("Player 1");
                                drops = AdjustDrops(drops);
                                task = ActionList.Gathering;
                                agent.destination = GetClosestDropOff(drops).transform.position;

                                heldResourceType = NodeManager.ResourceTypes.Iron;
                                drops = null;
                            }
                        } 
                        else if (hit.collider.tag == "Foundation")
                        {
                            isGathering = false;
                            isMeleeing = false;
                            task = ActionList.Building;
                            agent.destination = hit.collider.gameObject.transform.position;
                            targetNode = hit.collider.gameObject;
                        }
                        else if (hit.collider.tag == "Doorway")
                        {
                            if(IM.attackMoving) {
                                task = ActionList.Attacking;
                            } else {
                                task = ActionList.Moving;
                            }
                        } 
                        else if (hit.collider.tag == "Enemy Unit")
                        {
                            if(IM.attackMoving) {
                                task = ActionList.Attacking;
                            } else {
                                task = ActionList.Moving;
                            }
                            if(hit.collider.gameObject.GetComponent<UnitController>().isDead) {
                                agent.destination = hit.collider.gameObject.transform.position;
                            }
                        } 
                    }
                    else if (hit.collider.tag == "Player 1")
                    {
                        isBuilding = false;
                        isGathering = false;
                        agent.destination = hit.collider.gameObject.transform.position;
                        targetNode = hit.collider.gameObject;
                        task = ActionList.Delivering;
                    } 
                    else 
                    {
                        task = ActionList.Idle;
                    }
                    playerAudio.clip = unitMoveClip;
                    playerAudio.Play();
                } else if (UC.unitType == "Footman" || UC.unitType == "Swordsman" || UC.unitType == "Archer" || UC.unitType == "Outrider" || UC.unitType == "Knight" || UC.unitType == "Wizard") {
                    isBuilding = false;
                    isGathering = false;
                    if(targetScript != null) {
                        agent.destination = hit.collider.gameObject.transform.position;
                        isFollowing = true;
                        StartCoroutine(Follow());
                    }
                    else if (hit.collider.tag == "Ground")
                    {
                        isMeleeing = false;
                        if(IM.attackMoving) {
                            task = ActionList.Attacking;
                        } else {
                            task = ActionList.Moving;
                        }
                        CreateBoxFormation(hit);
                    }
                    else if (hit.collider.tag == "Doorway")
                    {

                    } 
                    else {
                        agent.destination = hit.collider.gameObject.transform.position;
                    }
                    playerAudio.clip = unitMoveClip;
                    playerAudio.Play();
                } 
            }
        }
    }

    public void CreateBoxFormation(RaycastHit hit) {
        formationList = IM.selectedObjects;
        float row = 0.0f;
        int counter = 0;
        if(formationList.Count == 1) { 
            for(int iteration = 0; iteration < formationList.Count; iteration++) {
                formationList[iteration].GetComponent<NavMeshAgent>().destination = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }
        } else if(formationList.Count <= 4) {
            for(int iteration = 0; iteration < formationList.Count; iteration++) {
                if(iteration <= 1) {
                    if(iteration % 2 == 0) {
                        counter = 0;
                    } else {
                        counter += 1;
                    }
                } else {
                    if(iteration % 2 == 0) {
                        row += 1.5f;
                        counter = 0;
                    } else {
                        counter += 1;
                    }
                }

                if(iteration % 2 == 0) {
                    formationList[iteration].GetComponent<NavMeshAgent>().destination = new Vector3(hit.point.x + 0.8f * counter, hit.point.y, hit.point.z + row);
                } else {
                    formationList[iteration].GetComponent<NavMeshAgent>().destination = new Vector3(hit.point.x - 0.8f * counter, hit.point.y, hit.point.z + row);
                }
            }
        } else if(formationList.Count > 4 && formationList.Count <= 16) {
            for(int iteration = 0; iteration < formationList.Count; iteration++) {
                if(iteration <= 1) {
                    if(iteration % 4 == 0) {
                        counter = 0;
                    } else {
                        counter += 1;
                    }
                } else {
                    if(iteration % 4 == 0) {
                        row += 1.5f;
                        counter = 0;
                    } else {
                        counter += 1;
                    }
                }

                if(iteration % 2 == 0) {
                    formationList[iteration].GetComponent<NavMeshAgent>().destination = new Vector3(hit.point.x + 0.8f * counter, hit.point.y, hit.point.z + row);
                } else {
                    formationList[iteration].GetComponent<NavMeshAgent>().destination = new Vector3(hit.point.x - 0.8f * counter, hit.point.y, hit.point.z + row);
                }
            }
        } else if(formationList.Count >= 16) {
            for(int iteration = 0; iteration < formationList.Count; iteration++) {
                if(iteration <= 1) {
                    if(iteration % 8 == 0) {
                        counter = 0;
                    } else {
                        counter += 1;
                    }
                } else {
                    if(iteration % 8 == 0) {
                        row += 1.5f;
                        counter = 0;
                    } else {
                        counter += 1;
                    }
                }

                if(iteration % 2 == 0) {
                    formationList[iteration].GetComponent<NavMeshAgent>().destination = new Vector3(hit.point.x + 0.8f * counter, hit.point.y, hit.point.z + row);
                } else {
                    formationList[iteration].GetComponent<NavMeshAgent>().destination = new Vector3(hit.point.x - 0.8f * counter, hit.point.y, hit.point.z + row);
                }
            }
        } 		
    }

    public void DropResource() {
        //Handle drop off!
        if (RM.skymetal >= RM.maxSkymetal)
        {
            task = ActionList.Idle;
        }
        else
        {
            // if target node is destroyed
            if (targetNode == null)
            {
                isGathering = false;
                if (heldResource != 0)
                {
                    drops = GameObject.FindGameObjectsWithTag("Player 1");
                    drops = AdjustDrops(drops);
                    task = ActionList.Delivering;
                    agent.destination = GetClosestDropOff(drops).transform.position;

                    RM.skymetal += heldResource;
                    heldResource = 0;
                    drops = null;
                }
                else
                {
                    task = ActionList.Idle;
                }
            }
            else
            {
                RM.skymetal += heldResource;
                heldResource = 0;
                task = ActionList.Gathering;
                agent.destination = targetNode.transform.position;
            }
        }
    }

    public void DropSkyMetal()
    {
        //Handle drop off!
        if (RM.skymetal >= RM.maxSkymetal)
        {
            task = ActionList.Idle;
        }
        else
        {
            // if target node is destroyed
            if (targetNode == null)
            {
                isGathering = false;
                if (heldResource != 0)
                {
                    drops = GameObject.FindGameObjectsWithTag("Player 1");
                    drops = AdjustDrops(drops);
                    task = ActionList.Delivering;
                    agent.destination = GetClosestDropOff(drops).transform.position;

                    RM.skymetal += heldResource;
                    heldResource = 0;
                    drops = null;
                }
                else
                {
                    task = ActionList.Idle;
                }
            }
            else
            {
                RM.skymetal += heldResource;
                heldResource = 0;
                task = ActionList.Gathering;
                agent.destination = targetNode.transform.position;
            }
        }
    }

    public void DropWood()
    {
        //Handle drop off!
        if (RM.wood >= RM.maxWood)
        {
            task = ActionList.Idle;
        }
        else
        {
            // if target node is destroyed
            if (targetNode == null)
            {
                isGathering = false;
                if (heldResource != 0)
                {
                    drops = GameObject.FindGameObjectsWithTag("Player 1");
                    drops = AdjustDrops(drops);
                    agent.destination = GetClosestDropOff(drops).transform.position;

                    RM.wood += heldResource;
                    heldResource = 0;
                    task = ActionList.Delivering;
                    drops = null;
                }
                else
                {
                    task = ActionList.Idle;
                }
            }
            else
            {
                RM.wood += heldResource;
                heldResource = 0;
                task = ActionList.Gathering;
                agent.destination = targetNode.transform.position;
            }
        }
    }

    public void DropIron()
    {
        //Handle drop off!
        if (RM.iron >= RM.maxIron)
        {
            task = ActionList.Idle;
        }
        else
        {
            if (targetNode == null)
            {
                isGathering = false;
                if (heldResource != 0)
                {
                    drops = GameObject.FindGameObjectsWithTag("Player 1");
                    drops = AdjustDrops(drops);
                    agent.destination = GetClosestDropOff(drops).transform.position;

                    RM.iron += heldResource;
                    task = ActionList.Delivering;
                    heldResource = 0;
                    drops = null;
                }
                else
                {
                    task = ActionList.Idle;
                }
            }
            else
            {
                RM.iron += heldResource;
                heldResource = 0;
                task = ActionList.Gathering;
                agent.destination = targetNode.transform.position;
            }
        }
    }

    public void DropSteel()
    {
        //Handle drop off!
        task = ActionList.Gathering;
        RM.steel += heldResource;
        heldResource = 0;
        if(RM.iron < 100) {
            RM.iron -= RM.iron;
            heldResource = (int)RM.iron;
        } else {
            RM.iron -= 100;
            heldResource = 100;
        }
        heldResourceType = NodeManager.ResourceTypes.Iron;
        agent.destination = targetNode.transform.position;
    }

    public void DropStone()
    {
        //Handle drop off!
        if (RM.stone >= RM.maxStone)
        {
            task = ActionList.Idle;
        }
        else
        {
            // if target node is destroyed
            if (targetNode == null)
            {
                isGathering = false;
                if (heldResource != 0)
                {
                    drops = GameObject.FindGameObjectsWithTag("Player 1");
                    drops = AdjustDrops(drops);
                    agent.destination = GetClosestDropOff(drops).transform.position;

                    RM.stone += heldResource;
                    task = ActionList.Delivering;
                    heldResource = 0;
                    drops = null;
                }
                else
                {
                    task = ActionList.Idle;
                }
            }
            else
            {
                RM.stone += heldResource;
                heldResource = 0;
                task = ActionList.Gathering;
                agent.destination = targetNode.transform.position;
            }
        }
    }

    public void DropGold()
    {
        //Handle drop off!
        if (RM.gold >= RM.maxGold)
        {
            task = ActionList.Idle;
        }
        else
        {
            // if target node is destroyed
            if (targetNode == null)
            {
                isGathering = false;
                if (heldResource != 0)
                {
                    drops = GameObject.FindGameObjectsWithTag("Player 1");
                    drops = AdjustDrops(drops);
                    agent.destination = GetClosestDropOff(drops).transform.position;
                    RM.gold += heldResource;
                    task = ActionList.Delivering;
                    heldResource = 0;
                    drops = null;
                }
                else
                {
                    task = ActionList.Idle;
                }
            }
            else
            {
                RM.gold += heldResource;
                heldResource = 0;
                task = ActionList.Gathering;
                agent.destination = targetNode.transform.position;
            }
        }
    }

    public void DropFood()
    {
        //Handle drop off!
        if (RM.food >= RM.maxFood)
        {
            task = ActionList.Idle;
        }
        else
        {
            // if target node is destroyed
            if (targetNode == null)
            {
                isGathering = false;
                if (heldResource != 0)
                {
                    drops = GameObject.FindGameObjectsWithTag("Player 1");
                    drops = AdjustDrops(drops);
                    agent.destination = GetClosestDropOff(drops).transform.position;

                    RM.food += heldResource;
                    task = ActionList.Delivering;
                    heldResource = 0;
                    drops = null;
                }
                else
                {
                    task = ActionList.Idle;
                }
            }
            else
            {
                RM.food += heldResource;
                heldResource = 0;
                task = ActionList.Gathering;
                agent.destination = targetNode.transform.position;
            }
        }
    }

    // Allows collider of resources 
    public void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.gameObject;
        if (hitObject.tag == "Resource" && hitObject.gameObject == targetNode && gameObject.GetComponent<UnitController>().unitType == "Worker")
        {
            isGathering = true;
            harvestScript = targetNode.GetComponent<NodeManager>();
            harvestSpeed = harvestScript.harvestTime;
            heldResourceType = hitObject.GetComponent<NodeManager>().resourceType;
            StartCoroutine(Tick());
        } else if (hitObject.tag == "Blacksmith" && hitObject.gameObject == targetNode && gameObject.GetComponent<UnitController>().unitType == "Worker")
        {
            isGathering = true;
            harvestScript = targetNode.GetComponent<NodeManager>();
            harvestSpeed = harvestScript.harvestTime;
            StartCoroutine(Tick());
        } else if (hitObject.tag == "Foundation" && hitObject.gameObject == targetNode && gameObject.GetComponent<UnitController>().unitType == "Worker")
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            isBuilding = true;
            hitObject.GetComponent<FoundationController>().builderList.Add(gameObject);
            buildScript = targetNode.GetComponent<FoundationController>();
            buildSpeed = buildScript.buildTime;
            StartCoroutine(Tick());
        }
        else if(hitObject.tag == "Player 1" && task == ActionList.Delivering && gameObject.GetComponent<UnitController>().unitType == "Worker")
        {
            if (heldResourceType == NodeManager.ResourceTypes.Skymetal)
            {
                DropSkyMetal();
            }

            if (heldResourceType == NodeManager.ResourceTypes.Wood)
            {
                DropWood();
            }

            if (heldResourceType == NodeManager.ResourceTypes.Iron)
            {
                DropIron();
            }

            if (heldResourceType == NodeManager.ResourceTypes.Steel)
            {
                DropSteel();
            }

            if (heldResourceType == NodeManager.ResourceTypes.Stone)
            {
                DropStone();
            }

            if (heldResourceType == NodeManager.ResourceTypes.Gold)
            {
                DropGold();
            }

            if (heldResourceType == NodeManager.ResourceTypes.Food)
            {
                DropFood();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        GameObject hitObject = other.gameObject;
        if (hitObject.tag == "Resource" && hitObject.gameObject == targetNode || hitObject.tag == "Blacksmith" && hitObject.gameObject == targetNode)
        {
            isGathering = false;
        }  else if (hitObject.tag == "Foundation")
        {
            isBuilding = false;
            if(hitObject.GetComponent<FoundationController>().builderList.Contains(gameObject)) {
                hitObject.GetComponent<FoundationController>().builderList.Remove(gameObject);
            }
        } 
    }

    public IEnumerator ShowDestinations() {
        Destroy(currentRightClickCursor);
        if(!PM.gamePaused) {
            List<GameObject> createdDestinations = new List<GameObject>();
            for(int i=0; i<IM.selectedObjects.Count; i++) {
                Vector3 agentDestination = IM.selectedObjects[i].GetComponent<NavMeshAgent>().destination;
                currentRightClickCursor = Instantiate(rightClickCursor);
                currentRightClickCursor.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                currentRightClickCursor.transform.position = new Vector3 (agentDestination.x, agentDestination.y + 0.3f, agentDestination.z);
                createdDestinations.Add(currentRightClickCursor);
            }
            yield return new WaitForSeconds(1f);
            for(int i=0; i<createdDestinations.Count; i++) {
                Destroy(createdDestinations[i]);
            }
        }
    }
    
    IEnumerator Follow() {
        int counter = 0;
        if(targetScript.owner != team) {
            isAttacking = true;
            task = ActionList.Attacking;
        } else if(targetScript.owner == team) {
            isAttacking = false;
        } 

        while(isFollowing) {
            if(targetNode == null || targetNode.GetComponent<UnitController>().isDead) {
                isMeleeing = false;
                isAttacking = false;
                isFollowing = false;
                break;
            }
            float dist = Vector3.Distance(targetNode.transform.position, agent.transform.position);
            if(!isMeleeing && isAttacking && dist < UC.attackRange) {
                isMeleeing = true;
                StartCoroutine(UC.Attack(targetNode, agent.transform.rotation));
            } else {
                // isMeleeing = false;
            }
            
            if (counter < 6) {
                bool followed = targetNode.transform.GetChild(2).gameObject.activeInHierarchy;
                targetNode.transform.GetChild(2).gameObject.SetActive(!followed);
            }
            
            counter += 1;
            agent.destination = targetNode.transform.position;
            yield return new WaitForSeconds(0.2f);
        }
    }
    // Ticks down while villager is gathering - Adjust with heldResource in Tick in Selection Script
    IEnumerator Tick()
    {
        if(!isGathering && !isBuilding) {
            yield break;
        }

        while(isGathering || isBuilding)
        {
            int toolModifier;
            if(RC.artisanToolSmithing) {
                toolModifier = 3;
            } else if (RC.basicToolSmithing) {
                toolModifier = 2;
            } else {
                toolModifier = 1;
            }

            if(isBuilding) {
                // agent.destination = agent.transform.position;
                harvestSpeed = UC.attackSpeed;
            }

            yield return new WaitForSeconds(harvestSpeed);

            if(isBuilding || isGathering && heldResourceType == NodeManager.ResourceTypes.Wood || isGathering && heldResourceType == NodeManager.ResourceTypes.Food)
            {
                UC.unitAudio = agent.GetComponent<AudioSource>();
                UC.unitAudio.clip = UC.woodChop;
                UC.unitAudio.maxDistance = 55;
                UC.unitAudio.Play();
            } 
            
            if(isGathering && heldResourceType != NodeManager.ResourceTypes.Wood && heldResourceType != NodeManager.ResourceTypes.Food)
            {
                UC.unitAudio = agent.GetComponent<AudioSource>();
                UC.unitAudio.clip = UC.metalChop;
                UC.unitAudio.maxDistance = 55;
                UC.unitAudio.Play();
            } 
            
            if(isGathering && heldResourceType == NodeManager.ResourceTypes.Skymetal)
            {
                heldResource += 5 * toolModifier;
                if(heldResource > maxHeldResource) {
                    heldResource = maxHeldResource;
                }
            } 
            else if (isGathering && heldResourceType == NodeManager.ResourceTypes.Wood)
            {
                heldResource += 5 * toolModifier;
                if(heldResource > maxHeldResource) {
                    heldResource = maxHeldResource;
                }
            }
            else if (isGathering && heldResourceType == NodeManager.ResourceTypes.Iron)
            {
                heldResource += 5 * toolModifier;
                if(heldResource > maxHeldResource) {
                    heldResource = maxHeldResource;
                }
            }
            else if (isGathering && heldResourceType == NodeManager.ResourceTypes.Stone)
            {
                heldResource += 5 * toolModifier;
                if(heldResource > maxHeldResource) {
                    heldResource = maxHeldResource;
                }
            }
            else if (isGathering && heldResourceType == NodeManager.ResourceTypes.Gold)
            {
                heldResource += 5 * toolModifier;
                if(heldResource > maxHeldResource) {
                    heldResource = maxHeldResource;
                }
            }
            else if (isGathering && heldResourceType == NodeManager.ResourceTypes.Food)
            {
                heldResource += 5 * toolModifier;
                if(heldResource > maxHeldResource) {
                    heldResource = maxHeldResource;
                }
            }
            else if (isGathering)
            {
                heldResource += 5 * toolModifier;    
                if(heldResource > maxHeldResource) {
                    heldResource = maxHeldResource;
                }           
            }
        }
    }
}

using System.Collections;
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
    public GameObject player;
    public GameObject owner;
    private AudioSource playerAudio;

    // Grab nodemanager script and harvest speed on gameobject
    FoundationController buildScript;
    float buildSpeed;
    NodeManager harvestScript;
    InputManager IM;
    UnitController UC;
    ResearchController RC;
    UnitSelection targetScript;
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



    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            player = GameObject.FindGameObjectWithTag("Player");
            playerAudio = GameObject.FindGameObjectWithTag("Main Audio").GetComponent<AudioSource>();
            RM = player.GetComponent<ResourceManager>();
            RC = player.GetComponent<ResearchController>();
            IM = player.GetComponent<InputManager>();
            UC = GetComponent<UnitController>();
            agent = GetComponent<NavMeshAgent>();
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
                        if(drops.Length > 0)
                        {
                            task = ActionList.Delivering;
                            agent.destination = GetClosestDropOff(drops).transform.position;
                            drops = null;
                        } else
                        {
                            task = ActionList.Idle;
                        }
                    } else
                    {
                        resources = GameObject.FindGameObjectsWithTag("Resource");
                        targetNode = GetClosestResource(resources);
                        agent.destination = targetNode.transform.position;
                    }
                } else
                {
                    if(targetNode == null && isBuilding) {
                        task = ActionList.Idle;
                        isBuilding = false;
                    }
                    if (heldResource >= maxHeldResource)
                    {
                        isGathering = false;
                        drops = GameObject.FindGameObjectsWithTag("Player 1");
                        if (drops.Length > 0)
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
            if (Input.GetMouseButtonDown(1) && selected == true)
            {
                if (!EventSystem.current.IsPointerOverGameObject(-1))
                {   
                    RightClick();
                }
            }
        }
    }

    GameObject GetClosestDropOff(GameObject[] dropOffs)
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

        if (Physics.Raycast(ray, out hit, 350))
        {
            StartCoroutine(RightIndicator(hit));
            targetNode = hit.collider.gameObject;
            targetScript = targetNode.GetComponent<UnitSelection>();
            if(owner == player && !UC.isDead) {     
                if(UC.unitType == "Worker") {
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
                            task = ActionList.Moving;
                            agent.destination = hit.point;
                        }
                        else if (hit.collider.tag == "Resource")
                        {
                            isBuilding = false;
                            isMeleeing = false;
                            task = ActionList.Gathering;
                            agent.destination = hit.collider.gameObject.transform.position;
                            targetNode = hit.collider.gameObject;
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
                } else if (UC.unitType == "Footman" || UC.unitType == "Swordsman" || UC.unitType == "Archer" || UC.unitType == "Wizard") {
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
                        task = ActionList.Moving;
                        agent.destination = hit.point;
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

    public void OnTriggerStay(Collider other)
    {
        GameObject hitObject = other.gameObject;

        if (hitObject.tag == "Player 1" && task == ActionList.Delivering && heldResource != 0) 
        {
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
                    agent.destination = GetClosestDropOff(drops).transform.position;

                    RM.skymetal += heldResource;
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
        } else if (hitObject.tag == "Foundation" && hitObject.gameObject == targetNode && gameObject.GetComponent<UnitController>().unitType == "Worker")
        {
            isBuilding = true;
            hitObject.GetComponent<FoundationController>().builders++;
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
        if (hitObject.tag == "Resource" && hitObject.gameObject == targetNode)
        {
            isGathering = false;
        }  else if (hitObject.tag == "Foundation" && hitObject.gameObject == targetNode)
        {
            isBuilding = false;
            hitObject.GetComponent<FoundationController>().builders--;
        } 
    }

    IEnumerator Follow() {
        int counter = 0;
        if(targetScript.owner != player) {
            isAttacking = true;
        } else if(targetScript.owner == player) {
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
                agent.destination = agent.transform.position;
                
                harvestSpeed = UC.attackSpeed;
            }
            yield return new WaitForSeconds(harvestSpeed);
            if(isBuilding)
            {
                UC.unitAudio = agent.GetComponent<AudioSource>();
                UC.unitAudio.clip = UC.woodChop;
                UC.unitAudio.maxDistance = 55;
                UC.unitAudio.Play();
            } 
            else if(isGathering && heldResourceType == NodeManager.ResourceTypes.Skymetal)
            {
                heldResource += 5 * toolModifier;
                if(heldResource > maxHeldResource) {
                    heldResource = maxHeldResource;
                }
                UC.unitAudio = agent.GetComponent<AudioSource>();
                UC.unitAudio.clip = UC.metalChop;
                UC.unitAudio.maxDistance = 55;
                UC.unitAudio.Play();
            } 
            else if (isGathering && heldResourceType == NodeManager.ResourceTypes.Wood)
            {
                heldResource += 5 * toolModifier;
                if(heldResource > maxHeldResource) {
                    heldResource = maxHeldResource;
                }
                UC.unitAudio = agent.GetComponent<AudioSource>();
                UC.unitAudio.clip = UC.woodChop;
                UC.unitAudio.maxDistance = 55;
                UC.unitAudio.Play();
            }
            else if (isGathering && heldResourceType == NodeManager.ResourceTypes.Iron)
            {
                heldResource += 5 * toolModifier;
                if(heldResource > maxHeldResource) {
                    heldResource = maxHeldResource;
                }
                UC.unitAudio = agent.GetComponent<AudioSource>();
                UC.unitAudio.clip = UC.metalChop;
                UC.unitAudio.maxDistance = 55;
                UC.unitAudio.Play();
            }
            else if (isGathering && heldResourceType == NodeManager.ResourceTypes.Stone)
            {
                heldResource += 5 * toolModifier;
                if(heldResource > maxHeldResource) {
                    heldResource = maxHeldResource;
                }
                UC.unitAudio = agent.GetComponent<AudioSource>();
                UC.unitAudio.clip = UC.metalChop;
                UC.unitAudio.maxDistance = 55;
                UC.unitAudio.Play();
            }
            else if (isGathering && heldResourceType == NodeManager.ResourceTypes.Gold)
            {
                heldResource += 5 * toolModifier;
                if(heldResource > maxHeldResource) {
                    heldResource = maxHeldResource;
                }
                UC.unitAudio = agent.GetComponent<AudioSource>();
                UC.unitAudio.clip = UC.metalChop;
                UC.unitAudio.maxDistance = 55;
                UC.unitAudio.Play();
            }
            else if (isGathering && heldResourceType == NodeManager.ResourceTypes.Food)
            {
                heldResource += 5 * toolModifier;
                if(heldResource > maxHeldResource) {
                    heldResource = maxHeldResource;
                }
                UC.unitAudio = agent.GetComponent<AudioSource>();
                UC.unitAudio.clip = UC.woodChop;
                UC.unitAudio.maxDistance = 55;
                UC.unitAudio.Play();
            }
            else if (isGathering)
            {
                heldResource += 5 * toolModifier;    
                if(heldResource > maxHeldResource) {
                    heldResource = maxHeldResource;
                }           
                UC.unitAudio = agent.GetComponent<AudioSource>();
                UC.unitAudio.clip = UC.metalChop;
                UC.unitAudio.maxDistance = 55;
                UC.unitAudio.Play();
            }
        }
    }

    IEnumerator RightIndicator (RaycastHit hit) {
        yield return new WaitForSeconds(2);
    }

}

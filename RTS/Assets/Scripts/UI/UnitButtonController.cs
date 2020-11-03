using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UnitButtonController : MonoBehaviour
{
    public GameObject player;
    public GameObject team;
    ResourceManager RM;
    UIController UI;
    BuildingController BC;
    InputManager IM;

    public BuildingController building;
    public bool isPlaceable;

    //Buttons
    public Button buttonOne, buttonTwo, buttonThree, buttonFour, buttonFive, buttonSix, buttonSeven, buttonEight, basicBuildings, advancedBuildings, basicBack, advancedBack, clearDead;

    //Audio
    public AudioSource playerAudio;
    public AudioClip constructingBuilding;

    //Buildings
    public GameObject house;
    public GameObject townHall;
    public GameObject barracks;
    public GameObject fort;
    public GameObject farm;
    public GameObject blacksmith;
    public GameObject lumberMill;
    public GameObject stables;

    public List<GameObject> selectedGOs;

    private Vector3 mousePosition;
    public int mask;

    public GameObject currentPlaceableObject;
    public Material placing;

    // Mesh messing
    public MeshRenderer selectedMesh;
    public Material[] mats;
    Material[] materialArray;
    public List<Material[]> listOfMaterialArrays = new List<Material[]>();
    Material selectedMaterial;
    public Material redMaterial;
    private Vector3 currentLocation;
    private Quaternion currentRotation;

    // Dead pile
    public List<GameObject> dead = new List<GameObject>();
    private float mouseWheelRotation;

    // Canvas
    GameObject basicBuildingsPanel;
    GameObject advancedBuildingsPanel;
    GameObject workerPanel;

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            team = GameObject.Find("Faction");
            player = GameObject.Find("Game").GetComponent<SaveLoad>().loadedPlayer;
            IM = player.GetComponent<InputManager>();
            RM = team.GetComponent<ResourceManager>();
            UI = player.GetComponent<UIController>();
            basicBuildingsPanel = GameObject.Find("BasicBuildingsPanel");
            advancedBuildingsPanel = GameObject.Find("AdvancedBuildingsPanel");
            workerPanel = GameObject.Find("VillagerPanel");

            basicBack = basicBuildingsPanel.transform.GetChild(11).GetComponent<Button>();
            basicBack.onClick.AddListener(UI.WorkerSelect);

            advancedBack = advancedBuildingsPanel.transform.GetChild(11).GetComponent<Button>();
            advancedBack.onClick.AddListener(UI.WorkerSelect);

            basicBuildings = workerPanel.transform.GetChild(0).GetComponent<Button>();
            basicBuildings.onClick.AddListener(UI.VillagerBasicBuildings);
            
            advancedBuildings = workerPanel.transform.GetChild(1).GetComponent<Button>();
            advancedBuildings.onClick.AddListener(UI.VillagerAdvancedBuildings);

            clearDead = workerPanel.transform.GetChild(2).GetComponent<Button>();
            clearDead.onClick.AddListener(ClearDead);
            
            buttonTwo = basicBuildingsPanel.transform.GetChild(1).GetComponent<Button>();
            buttonTwo.onClick.AddListener(delegate{BuildStructure(house);});

            buttonThree = basicBuildingsPanel.transform.GetChild(2).GetComponent<Button>();
            buttonThree.onClick.AddListener(delegate{BuildStructure(farm);});

            buttonFour = basicBuildingsPanel.transform.GetChild(3).GetComponent<Button>();
            buttonFour.onClick.AddListener(delegate{BuildStructure(townHall);});

            buttonFive = basicBuildingsPanel.transform.GetChild(4).GetComponent<Button>();
            buttonFive.onClick.AddListener(delegate{BuildStructure(blacksmith);});

            buttonSix = basicBuildingsPanel.transform.GetChild(5).GetComponent<Button>();
            buttonSix.onClick.AddListener(delegate{BuildStructure(lumberMill);});

            buttonSeven = basicBuildingsPanel.transform.GetChild(6).GetComponent<Button>();
            buttonSeven.onClick.AddListener(delegate{BuildStructure(stables);});
            
            buttonEight = basicBuildingsPanel.transform.GetChild(7).GetComponent<Button>();
            buttonEight.onClick.AddListener(delegate{BuildStructure(barracks);});
        }
    }


    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            if(RM.townHallCount == 0) {
                buttonFive.interactable = false; 
                buttonSix.interactable = false;
                buttonSeven.interactable = false;
                buttonEight.interactable = false;
            } else if (RM.townHallCount > 0) {
                buttonFive.interactable = true; 
                buttonSix.interactable = true;
                buttonSeven.interactable = true;
                buttonEight.interactable = true;
            }
            
            if (currentPlaceableObject != null)
            {
                if(IM.rotating) {
                    UI.RotationModeText();
                } else {
                    UI.PlacementModeText();
                }
                if (!EventSystem.current.IsPointerOverGameObject(-1))
                {
                    building = currentPlaceableObject.GetComponent<BuildingController>();
                    isPlaceable = building.placeable;
                    MoveCurrentPlaceableObjectToMouse();
                    if (isPlaceable == true)
                    {
                        ReleaseIfClicked();
                    }
                }
                if(currentPlaceableObject) {
                    ChangePrefabColorIfPlaceable(currentPlaceableObject);
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Destroy(currentPlaceableObject);
                }
                if(IM.rotating) {
                    RotateFromMouseWheel();
                }
            }
        }
    }

    private void RotateFromMouseWheel() {
        mouseWheelRotation = Input.mouseScrollDelta.y;
        currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }

    void ClearDead() {
        selectedGOs = IM.selectedObjects;
        foreach(GameObject go in selectedGOs) {
            StartCoroutine(ClearingDead(go));
        }
    }

    void BuildStructure(GameObject structure) {
        BC = structure.GetComponent<BuildingController>();
        bool enoughResources = CheckResources(BC);
        if(currentPlaceableObject == null && enoughResources) {
            switch(BC.unitType) {
                case "House":
                    CreateBasicBuildingInstance(BC, structure);
                    break;
                case "Farm":
                    CreateMultiMeshBuildingInstance(BC, structure);
                    break;
                case "Town Hall":
                    CreateMultiMeshBuildingInstance(BC, structure);
                    break;
                case "Blacksmith":
                    CreateBasicBuildingInstance(BC, structure);
                    break;
                case "Barracks":
                    CreateMultiMeshBuildingInstance(BC, structure);
                    break;
                case "Lumber Yard":
                    CreateMultiMeshSphereBuildingInstance(BC, structure);
                    break;
                case "Stables":
                    CreateMultiMeshBuildingInstance(BC, structure);
                    break;
                default:
                    return;
            }
        } else {
            Destroy(currentPlaceableObject);
        }
    }

    void CreateMultiMeshBuildingInstance(BuildingController BC, GameObject selectedPrefab) {
        currentPlaceableObject = Instantiate(selectedPrefab);
        BuildingController buildingScript = currentPlaceableObject.GetComponent<BuildingController>();
        BoxCollider boxCollider = buildingScript.GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        listOfMaterialArrays.Clear();

        for (int i = 0; i < currentPlaceableObject.transform.childCount; i++) {
            Transform childTransform = currentPlaceableObject.transform.GetChild(i);
            if(childTransform.gameObject.GetComponent<MeshRenderer>()) {
                Material[] currentMaterialArray = currentPlaceableObject.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().materials;
                listOfMaterialArrays.Add(currentMaterialArray);
            } else {
                listOfMaterialArrays.Add(mats);
            }
        }
    }
    
    void CreateMultiMeshSphereBuildingInstance(BuildingController BC, GameObject selectedPrefab) {
        currentPlaceableObject = Instantiate(lumberMill);
        BuildingController buildingScript = currentPlaceableObject.GetComponent<BuildingController>();
        SphereCollider sphereCollider = buildingScript.GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        listOfMaterialArrays.Clear();

        for (int i = 0; i < currentPlaceableObject.transform.childCount; i++) {
            Transform childTransform = currentPlaceableObject.transform.GetChild(i);
            if(childTransform.gameObject.GetComponent<MeshRenderer>()) {
                Material[] currentMaterialArray = currentPlaceableObject.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().materials;
                listOfMaterialArrays.Add(currentMaterialArray);
            } else {
                listOfMaterialArrays.Add(mats);
            }
        }
    }

    void CreateBasicBuildingInstance(BuildingController BC, GameObject selectedPrefab) {
        currentPlaceableObject = Instantiate(selectedPrefab);
        selectedMesh = currentPlaceableObject.GetComponent<MeshRenderer>();
        Material[] mats = selectedMesh.materials;
        listOfMaterialArrays.Clear();

        BuildingController buildingScript = currentPlaceableObject.GetComponent<BuildingController>();
        BoxCollider boxCollider = buildingScript.GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        int iter = 0;
        int materialNum = mats.Length;
        materialArray = new Material[materialNum];

        foreach (Material mat in mats)
        {
            selectedMaterial = mat;
            materialArray[iter] = selectedMaterial;
            iter += 1;
        }
        iter = 0;
    }
    
    void ChangePrefabColorIfPlaceable(GameObject currentPlaceableObject) {
        BuildingController currentBuildingScript = currentPlaceableObject.GetComponent<BuildingController>();
        string buildingType = currentBuildingScript.unitType;
        if (isPlaceable == false)
        {
            if(buildingType == "House" || buildingType == "Blacksmith") {
                Material[] materialsArray = currentPlaceableObject.GetComponent<MeshRenderer>().materials;

                for (int i = 0; i < materialsArray.Length; i++)
                {
                    materialsArray[i] = redMaterial;
                }
                currentPlaceableObject.GetComponent<Renderer>().materials = materialsArray;
            } else {
                for (int i = 0; i < currentPlaceableObject.transform.childCount; i++) {
                    Transform childTransform = currentPlaceableObject.transform.GetChild(i);
                    if(childTransform.gameObject.GetComponent<MeshRenderer>()) {
                        MeshRenderer childRenderer = childTransform.gameObject.GetComponent<MeshRenderer>();
                        Material[] materialsArray = childRenderer.materials;
                        for (int j = 0; j < materialsArray.Length; j++)
                        {
                            materialsArray[j] = redMaterial;
                        }
                        childRenderer.materials = materialsArray;
                    }
                }
            }
        }
        else
        {
            if(buildingType == "House" || buildingType == "Blacksmith") {
                Material[] materialsArray = currentPlaceableObject.GetComponent<Renderer>().materials;
                for (int i = 0; i < materialsArray.Length; i++)
                {
                    materialsArray[i] = materialArray[i];
                }
                currentPlaceableObject.GetComponent<Renderer>().materials = materialsArray;
            } else {
                for (int i = 0; i < currentPlaceableObject.transform.childCount; i++) {
                    Transform childTransform = currentPlaceableObject.transform.GetChild(i);
                    if(childTransform.gameObject.GetComponent<MeshRenderer>()) {
                        MeshRenderer childRenderer = childTransform.gameObject.GetComponent<MeshRenderer>();
                        Material[] materialsArray = childRenderer.materials;
                        for (int j = 0; j < materialsArray.Length; j++)
                        {
                            // Not perfect
                            if(materialsArray[j]) {
                                if(listOfMaterialArrays[i][j]) {
                                    materialsArray[j] = listOfMaterialArrays[i][j];
                                }
                            }
                        }
                        childRenderer.materials = materialsArray;
                    }
                }
            }
        }
    }

    bool CheckResources(BuildingController buildingResources){
        bool enoughResources = true;
        string notEnoughResourcesText = "";
        if (RM.gold < buildingResources.gold){
            notEnoughResourcesText = "Not enough gold!";
            enoughResources = false;
        } else if (RM.wood < buildingResources.wood){
            notEnoughResourcesText = "Not enough wood!";
            enoughResources = false;
        } else if (RM.food < buildingResources.food){
            notEnoughResourcesText = "Not enough food!";
            enoughResources = false;
        } else if (RM.iron < buildingResources.iron){
            notEnoughResourcesText = "Not enough iron!";
            enoughResources = false;
        } else if (RM.steel < buildingResources.steel){
            notEnoughResourcesText = "Not enough steel!";
            enoughResources = false;
        } else if (RM.skymetal < buildingResources.skymetal){
            notEnoughResourcesText = "Not enough skymetal!";
            enoughResources = false;
        } else if (RM.stone < buildingResources.stone){
            notEnoughResourcesText = "Not enough stone!";
            enoughResources = false;
        } 

        if(!enoughResources) {
            UI.OpenNoResourcesText(notEnoughResourcesText);
            StartCoroutine(Wait());
        }
        return enoughResources;
    }


    private void MoveCurrentPlaceableObjectToMouse()
    {
        building.isPlaced = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 1000))
        {
            currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
            currentLocation = currentPlaceableObject.transform.position;
            currentRotation = currentPlaceableObject.transform.rotation;
           
            // Check if the building is placeable or not
            if (currentPlaceableObject.transform.rotation.x >= 0.05f || currentPlaceableObject.transform.rotation.x <= -0.05f || currentPlaceableObject.transform.rotation.z >= 0.05f || 
                currentPlaceableObject.transform.rotation.z <= -0.05f )
            {
                building.placeable = false;
            } else
            {
                if(building.inCollider == false)
                {
                    building.placeable = true;
                } else {
                    building.placeable = false;
                }
            }
        }
    }

    private void ReleaseIfClicked()
    {
        if(Input.GetMouseButtonDown(0)) {
            RM.gold -= BC.gold;
            RM.wood -= BC.wood;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject.transform.rotation = currentRotation;
            currentPlaceableObject = null;
            PlayBuildingSound();
            IM.rotating = false;   

        }        
    }

    private void PlayBuildingSound()
    {
        playerAudio.clip = constructingBuilding;
        playerAudio.Play();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        UI.CloseNoResourcesText();
        //my code here after 3 seconds
    }

    IEnumerator PlaceBuilding()
    {
        yield return new WaitForSeconds(3);
        //my code here after 3 seconds
        PlayBuildingSound();
    }

    public IEnumerator ClearingDead(GameObject go) 
    {        
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy Unit");
        GameObject[] allFriendlies = GameObject.FindGameObjectsWithTag("Selectable");
 
        int deadCount = 0;
        foreach(GameObject enemy in allEnemies) {
            if(enemy.GetComponent<UnitController>().isDead) {
                dead.Add(enemy);
                deadCount += 1;
            }
        }
        foreach(GameObject friend in allFriendlies) {
            if(friend.GetComponent<UnitController>()) {
                if(friend.GetComponent<UnitController>().isDead) {
                    dead.Add(friend);
                    deadCount += 1;
                }
            }
        }

        if(go.GetComponent<UnitController>().unitType == "Worker") {
            go.GetComponent<WorkerController>().clearingDead = true;
            NavMeshAgent agent = go.GetComponent<NavMeshAgent>();
            go.GetComponent<UnitSelection>().targetNode = GetClosestBody(dead, go);
            if(go.GetComponent<UnitSelection>().targetNode) {
                agent.destination = go.GetComponent<UnitSelection>().targetNode.transform.position;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
    
    public GameObject GetClosestBody(List<GameObject> dead, GameObject clearer)
    {
        GameObject closestBody = null;
        float closestBodyDistance = Mathf.Infinity;
        Vector3 bodyPosition = clearer.transform.position;

        foreach(GameObject targetBody in dead)
        {
            if(targetBody && targetBody.activeSelf) {
                Vector3 direction = targetBody.transform.position - bodyPosition;
                float dropDistance = direction.sqrMagnitude;
                if(dropDistance < closestBodyDistance)
                {
                    closestBodyDistance = dropDistance;
                    closestBody = targetBody;
                }
            }
        }
        return closestBody;
    }
}

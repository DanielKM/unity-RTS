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

    // Dead pile
    public List<GameObject> dead = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            mask =~ LayerMask.GetMask("FogOfWar");
            team = GameObject.Find("Faction");
            player = GameObject.FindGameObjectWithTag("Player");
            RM = team.GetComponent<ResourceManager>();
            UI = player.GetComponent<UIController>();
            basicBack.onClick.AddListener(UI.WorkerSelect);
            advancedBack.onClick.AddListener(UI.WorkerSelect);

            basicBuildings.onClick.AddListener(UI.VillagerBasicBuildings);
            advancedBuildings.onClick.AddListener(UI.VillagerAdvancedBuildings);
            clearDead.onClick.AddListener(ClearDead);

            buttonTwo.onClick.AddListener(BuildHouse);
            buttonThree.onClick.AddListener(BuildFarm);
            buttonFour.onClick.AddListener(BuildTownHall);

            buttonFive.onClick.AddListener(BuildBlacksmith);
            buttonSix.onClick.AddListener(BuildLumberMill);
            buttonSeven.onClick.AddListener(BuildStables);
            buttonEight.onClick.AddListener(BuildBarracks);
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
                ChangePrefabColorIfPlaceable(currentPlaceableObject);
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Destroy(currentPlaceableObject);
                }
            }
        }
    }

    void ClearDead() {
        InputManager IM = player.GetComponent<InputManager>();
        selectedGOs = IM.selectedObjects;
        foreach(GameObject go in selectedGOs) {
            StartCoroutine(ClearingDead(go));
        }
    }

    void BuildHouse()
    {
        BC = house.GetComponent<BuildingController>();
        CreateBasicBuildingInstance(BC, house);
    }

    void BuildFarm()
    {
        BC = farm.GetComponent<BuildingController>();
        CreateMultiMeshBuildingInstance(BC, farm);
    }

    void BuildTownHall()
    {
        BC = townHall.GetComponent<BuildingController>();
        CreateMultiMeshBuildingInstance(BC, townHall);
    }

    void BuildLumberMill()
    {
        BC = lumberMill.GetComponent<BuildingController>();
        CreateMultiMeshSphereBuildingInstance(BC, lumberMill);
    }

    void BuildStables()
    {
        BC = stables.GetComponent<BuildingController>();
        CreateMultiMeshBuildingInstance(BC, stables);
    }

    void BuildBarracks()
    {
        BC = barracks.GetComponent<BuildingController>();
        CreateMultiMeshBuildingInstance(BC, barracks);
    }

    void BuildFort()
    {
        BC = fort.GetComponent<BuildingController>();
        CreateMultiMeshBuildingInstance(BC, fort);
    }

    void BuildBlacksmith()
    {
        BC = blacksmith.GetComponent<BuildingController>();
        CreateBasicBuildingInstance(BC, blacksmith);
    }

    void CreateMultiMeshBuildingInstance(BuildingController BC, GameObject selectedPrefab) {
       if (currentPlaceableObject == null && RM.gold >= BC.gold && RM.wood >= BC.wood && RM.stone >= BC.stone && RM.iron >= BC.iron && RM.steel >= BC.steel && RM.skymetal >= BC.skymetal && RM.food >= BC.food)
       {
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
       else if (currentPlaceableObject == null || RM.gold < BC.gold || RM.wood < BC.wood || RM.stone < BC.stone || RM.food < BC.food  || RM.iron < BC.iron  || RM.steel < BC.steel  || RM.skymetal < BC.skymetal)
       {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
        else
        {
            Destroy(currentPlaceableObject);
        }
    }
    
    void CreateMultiMeshSphereBuildingInstance(BuildingController BC, GameObject selectedPrefab) {
        if (currentPlaceableObject == null && RM.gold >= BC.gold && RM.wood >= BC.wood && RM.stone >= BC.stone && RM.iron >= BC.iron && RM.steel >= BC.steel && RM.skymetal >= BC.skymetal && RM.food >= BC.food)
      {
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
       else if (currentPlaceableObject == null || RM.gold < BC.gold || RM.wood < BC.wood || RM.stone < BC.stone || RM.food < BC.food  || RM.iron < BC.iron  || RM.steel < BC.steel  || RM.skymetal < BC.skymetal)
       {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
        else
        {
            Destroy(currentPlaceableObject);
        }
    }

    void CreateBasicBuildingInstance(BuildingController BC, GameObject selectedPrefab) {
        if (currentPlaceableObject == null && RM.gold >= BC.gold && RM.wood >= BC.wood && RM.stone >= BC.stone && RM.iron >= BC.iron && RM.steel >= BC.steel && RM.skymetal >= BC.skymetal && RM.food >= BC.food)
        {
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
        else if (currentPlaceableObject == null || RM.gold < BC.gold || RM.wood < BC.wood || RM.stone < BC.stone || RM.food < BC.food  || RM.iron < BC.iron  || RM.steel < BC.steel  || RM.skymetal < BC.skymetal)
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
        else
        {
            Destroy(currentPlaceableObject);
        }
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

    private void MoveCurrentPlaceableObjectToMouse()
    {
        building.isPlaced = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 1000))
        {
            if (building.unitType == "House")
            {
                //HERE~!
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                currentLocation = new Vector3 (hitInfo.point.x, currentPlaceableObject.transform.position.y, hitInfo.point.z);
            }
            else if (building.unitType == "Farm")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x + 0.4f, hitInfo.point.y, hitInfo.point.z + 0.4f);
                currentLocation = currentPlaceableObject.transform.position;
            }
            else if (building.unitType == "Town Hall")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                Vector3 newLocation = new Vector3(currentPlaceableObject.transform.position.x, currentPlaceableObject.transform.position.y, currentPlaceableObject.transform.position.z);
                currentLocation = newLocation;
            }
            else if (building.unitType == "Lumber Yard")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                currentLocation = currentPlaceableObject.transform.position;
            }
            else if (building.unitType == "Stables")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                Vector3 newLocation = new Vector3(currentPlaceableObject.transform.position.x, currentPlaceableObject.transform.position.y, currentPlaceableObject.transform.position.z +2.0f);
                currentLocation = newLocation;
            }
            else if (building.unitType == "Barracks")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                Vector3 newLocation = new Vector3(currentPlaceableObject.transform.position.x, currentPlaceableObject.transform.position.y, currentPlaceableObject.transform.position.z);
                currentLocation = newLocation;
            }
            else if (building.unitType == "Fort")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                currentLocation = currentPlaceableObject.transform.position;
            }
            else if (building.unitType == "Blacksmith")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                Vector3 newLocation = new Vector3(currentPlaceableObject.transform.position.x, currentPlaceableObject.transform.position.y, currentPlaceableObject.transform.position.z);
                currentLocation = newLocation;
            }
            else if (building.unitType == "Resource")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                currentLocation = currentPlaceableObject.transform.position;
            }

            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            // Rotate the building 270 degrees
            // if(building.unitType == "Stables" || building.unitType == "Barracks" || building.unitType == "Town Hall" || building.unitType == "Blacksmith")
            // {
            //     currentPlaceableObject.transform.Rotate(0, 270, 0);
            // }

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
        if (Input.GetMouseButtonDown(0) && building.unitType == "House")
        {
            RM.gold -= BC.gold;
            RM.wood -= BC.wood;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Farm")
        {
            RM.gold -= BC.gold;
            RM.wood -= BC.wood;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            Vector3 newLocation = new Vector3(currentLocation.x, currentLocation.y, currentLocation.z);
            currentPlaceableObject.transform.position = newLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Town Hall")
        {
            RM.gold -= BC.gold;
            RM.wood -= BC.wood;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            Vector3 newLocation = new Vector3(currentLocation.x, currentLocation.y, currentLocation.z);
            currentPlaceableObject.transform.position = newLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Lumber Yard")
        {
            RM.gold -= BC.gold;
            RM.wood -= BC.wood;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Stables")
        {
            RM.gold -= BC.gold;
            RM.wood -= BC.wood;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Barracks")
        {
            RM.gold -= BC.gold;
            RM.wood -= BC.wood;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Fort")
        {
            RM.gold -= BC.gold;
            RM.wood -= BC.wood;
            RM.stone -= BC.stone;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Resource")
        {
            RM.gold -= BC.gold;
            RM.wood -= BC.wood;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Blacksmith")
        {
            RM.gold -= BC.gold;
            RM.wood -= BC.wood;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            currentPlaceableObject = null;
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
        UI.noResourcesText.SetActive(false);
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

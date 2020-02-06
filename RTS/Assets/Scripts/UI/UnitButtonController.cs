using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UnitButtonController : MonoBehaviour
{
    public GameObject player;
    ResourceManager RM;
    UIController UI;
    BuildingController BC;

    public BuildingController building;
    public bool isPlaceable;

    //Buttons
    public Button buttonOne, buttonTwo, buttonThree, buttonFour, buttonFive, buttonSix, buttonSeven, buttonEight, basicBuildings, advancedBuildings, basicBack, advancedBack;

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

    private Vector3 mousePosition;

    private GameObject currentPlaceableObject;
    public Material placing;
    public Renderer[] childColors;

    private MeshRenderer[] meshes;
    Color[] colors;
    Color color;
    private Vector3 currentLocation;

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            player = GameObject.FindGameObjectWithTag("Player");
            RM = player.GetComponent<ResourceManager>();
            UI = player.GetComponent<UIController>();
            basicBack.onClick.AddListener(UI.WorkerSelect);
            advancedBack.onClick.AddListener(UI.WorkerSelect);

            basicBuildings.onClick.AddListener(UI.VillagerBasicBuildings);
            advancedBuildings.onClick.AddListener(UI.VillagerAdvancedBuildings);

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

                if (isPlaceable == false)
                {
                    foreach (MeshRenderer mesh in meshes)
                    {
                        mesh.material.SetColor("_Color", Color.red);
                    }
                }
                else
                {
                    int colorIter = 0;
                    foreach (MeshRenderer mesh in meshes)
                    {
                        mesh.material.SetColor("_Color", colors[colorIter]);
                        colorIter += 1;
                    }
                    colorIter = 0;
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Destroy(currentPlaceableObject);
                }
            }
        }
    }

    void BuildHouse()
    {
        BC = house.GetComponent<BuildingController>();
        if (currentPlaceableObject == null && RM.gold >= BC.gold && RM.wood >= BC.wood && RM.stone >= BC.stone && RM.iron >= BC.iron && RM.steel >= BC.steel && RM.skymetal >= BC.skymetal && RM.food >= BC.food)
        {
            currentPlaceableObject = Instantiate(house);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();

            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("Base_color");
                colors[iter] = color;
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

    void BuildFarm()
    {
        BC = farm.GetComponent<BuildingController>();
        if (currentPlaceableObject == null && RM.gold >= BC.gold && RM.wood >= BC.wood && RM.stone >= BC.stone && RM.iron >= BC.iron && RM.steel >= BC.steel && RM.skymetal >= BC.skymetal && RM.food >= BC.food)
          {
            currentPlaceableObject = Instantiate(farm);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();

            BuildingController buildingScript = currentPlaceableObject.GetComponent<BuildingController>();
            BoxCollider boxCollider = buildingScript.GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("_Color");
                colors[iter] = color;
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

    void BuildTownHall()
    {
        BC = townHall.GetComponent<BuildingController>();
        //Output this to console when the Button2 is clicked
        if (currentPlaceableObject == null && RM.gold >= BC.gold && RM.wood >= BC.wood && RM.stone >= BC.stone && RM.iron >= BC.iron && RM.steel >= BC.steel && RM.skymetal >= BC.skymetal && RM.food >= BC.food)
       {
            currentPlaceableObject = Instantiate(townHall);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();

            BuildingController buildingScript = currentPlaceableObject.GetComponent<BuildingController>();
            BoxCollider boxCollider = buildingScript.GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("_Color");
                colors[iter] = color;
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

    void BuildLumberMill()
    {
        BC = lumberMill.GetComponent<BuildingController>();
        //Output this to console when the Button2 is clicked
        if (currentPlaceableObject == null && RM.gold >= BC.gold && RM.wood >= BC.wood && RM.stone >= BC.stone && RM.iron >= BC.iron && RM.steel >= BC.steel && RM.skymetal >= BC.skymetal && RM.food >= BC.food)
      {
            currentPlaceableObject = Instantiate(lumberMill);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();

            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("_Color");
                colors[iter] = color;
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

    void BuildStables()
    {
        BC = stables.GetComponent<BuildingController>();
        //Output this to console when the Button2 is clicked
        if (currentPlaceableObject == null && RM.gold >= BC.gold && RM.wood >= BC.wood && RM.stone >= BC.stone && RM.iron >= BC.iron && RM.steel >= BC.steel && RM.skymetal >= BC.skymetal && RM.food >= BC.food)
       {
            currentPlaceableObject = Instantiate(stables);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();

            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("_Color");
                colors[iter] = color;
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

    void BuildBarracks()
    {
        BC = barracks.GetComponent<BuildingController>();
        //Output this to console when the Button2 is clicked
        if (currentPlaceableObject == null && RM.gold >= BC.gold && RM.wood >= BC.wood && RM.stone >= BC.stone && RM.iron >= BC.iron && RM.steel >= BC.steel && RM.skymetal >= BC.skymetal && RM.food >= BC.food)
       {
            currentPlaceableObject = Instantiate(barracks);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();
            
            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("_Color");
                colors[iter] = color;
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

    void BuildFort()
    {
        BC = fort.GetComponent<BuildingController>();
        //Output this to console when the Button2 is clicked
        if (currentPlaceableObject == null && RM.gold >= BC.gold && RM.wood >= BC.wood && RM.stone >= BC.stone && RM.iron >= BC.iron && RM.steel >= BC.steel && RM.skymetal >= BC.skymetal && RM.food >= BC.food)
      {
            currentPlaceableObject = Instantiate(fort);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();

            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("_Color");
                colors[iter] = color;
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

    void BuildBlacksmith()
    {
        BC = blacksmith.GetComponent<BuildingController>();
        //Output this to console when the Button2 is clicked
        if (currentPlaceableObject == null && RM.gold >= BC.gold && RM.wood >= BC.wood && RM.stone >= BC.stone && RM.iron >= BC.iron && RM.steel >= BC.steel && RM.skymetal >= BC.skymetal && RM.food >= BC.food)
       {
            currentPlaceableObject = Instantiate(blacksmith);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();

            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("_Color");
                colors[iter] = color;
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
    
    private void MoveCurrentPlaceableObjectToMouse()
    {
        building.isPlaced = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (building.unitType == "House")
            {
                //HERE~!
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y - 1.0f, hitInfo.point.z);
                currentLocation = new Vector3 (hitInfo.point.x, currentPlaceableObject.transform.position.y + 0.75f, hitInfo.point.z);
            }
            else if (building.unitType == "Farm")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x + 0.4f, hitInfo.point.y, hitInfo.point.z + 0.4f);
                currentLocation = currentPlaceableObject.transform.position;
            }
            else if (building.unitType == "Town Hall")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                Vector3 newLocation = new Vector3(currentPlaceableObject.transform.position.x - 3.0f, currentPlaceableObject.transform.position.y, currentPlaceableObject.transform.position.z - 1.0f);
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
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x - 3.0f, hitInfo.point.y, hitInfo.point.z);
                Vector3 newLocation = new Vector3(currentPlaceableObject.transform.position.x + 3.0f, currentPlaceableObject.transform.position.y, currentPlaceableObject.transform.position.z - 4.0f);
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
            if(building.unitType == "Stables" || building.unitType == "Barracks" || building.unitType == "Town Hall" || building.unitType == "Blacksmith")
            {
                currentPlaceableObject.transform.Rotate(0, 270, 0);
            }

            if (currentPlaceableObject.transform.rotation.x >= 0.2f || currentPlaceableObject.transform.rotation.x <= -0.2f || currentPlaceableObject.transform.rotation.z >= 0.2f || 
                currentPlaceableObject.transform.rotation.z <= -0.2f )
            {
                building.placeable = false;
            } else
            {
                if(building.inCollider == false)
                {
                    building.placeable = true;
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
            Vector3 newLocation = new Vector3(currentLocation.x + 5.0f, currentLocation.y, currentLocation.z);
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
            Vector3 newLocation = new Vector3(currentLocation.x + 2.0f, currentLocation.y, currentLocation.z + 6.0f);
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
}

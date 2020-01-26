using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private GameObject player;

    // World Bounds
   public float xMin, xMax, yMin, yMax, zMin, zMax;

    // Reference Scripts
    UIController UI;

    UnitController unitScript;
    UnitSelection selectScript;
    BuildingController buildingScript;
    NodeManager nodeScript;
    BuildingButtonController buildingButtonScript;
    TownHallController townHallScript;
    BarracksController barracksScript;
    BlacksmithController blacksmithScript;
    FoundationController foundationScript;
    ResearchController RC;

    // UI FOR UNITS
    private AudioSource unitAudio;
    public AudioClip unitAudioClip;

    private AudioSource playerAudio;

    public Slider HB;
    public Text healthDisp;

    public Slider EB;
    public Text energyDisp;

    public Text unitName;
    public Text nameDisp;
    public Text rankDisp;
    public Text killDisp;

    public Text weaponDisp;
    public Text armourDisp;
    public Text itemDisp;

    public int resourceHeld;

    public bool isSelected;
    private bool inUnitSelectionBox;

    // UI FOR BUILDINGS
    private CanvasGroup BuildingPanel;
    private CanvasGroup BuildingActionPanel;
    private CanvasGroup AdvancedBuildingsPanel;
    private CanvasGroup BuildingProgressPanel;
    
    private GameObject BuildingProgressBar;
    public Slider BuildingProgressSlider;

    public Slider buildingHB;
    public Text buildingHealthDisp;

    public Slider buildingEB;
    public Text buildingEnergyDisp;

    public Text buildingName;
    public Text buildingNameDisp;
    public Text buildingRankDisp;
    public Text buildingKillDisp;

    public Text buildingWeaponDisp;
    public Text buildingArmourDisp;
    public Text buildingItemDisp;

    public Image progressIcon;
    public GameObject unitIcon;
    public GameObject buildingIcon;

    // TownHall variables
    public bool isTraining;
    public bool isBuilding;

    // Camera variables
    public float panSpeed;
    public float rotateSpeed;
    public float rotateAmount;

    private Quaternion rotation;
    private float minHeight = 5f;
    private float maxHeight = 150f;

    // Cursor variables
    public LayerMask clickableLayer;

    public Texture2D pointer;
    public Texture2D target;
    public Texture2D doorway;
    public Texture2D combat;
    public Texture2D sword;

    // New
    public EventVector3 OnClickEnvironment;
    public EventGameObject OnClickAttackable;
    
    // Multiselect variables
    bool isSelecting = false;
    public bool isUnit;

    Vector3 mousePosition1;
    // All units which are selected
    public GameObject[] selectedObjects;

    // All units in the game that are selectable
    private GameObject[] units;
    
    // Coordinates of all units
    private Vector3 unitPos;
    private float unitPosX;
    private float unitPosY;
    private float unitPosZ;


    // Grab the main camera
    Camera cam;
    Camera minimap;

    public UnitSelection selectedInfo;
    public GameObject selectedObj;

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            player = GameObject.FindGameObjectWithTag("Player");
            playerAudio = GameObject.FindGameObjectWithTag("Main Audio").GetComponent<AudioSource>();
            UI = player.GetComponent<UIController>();
            RC = player.GetComponent<ResearchController>();
        // progressIcon = GameObject.Find("ProgressIcon").GetComponent<Image>();
        // foundationScript = selectedObj.GetComponent<FoundationController>();
            // progressIcon.sprite = foundationScript.buildingPrefab.GetComponent<BuildingController>().icon;
            unitIcon = GameObject.Find("UnitIcon");
            BuildingProgressBar = GameObject.Find("BuildingProgressBar");
            BuildingProgressSlider = BuildingProgressBar.GetComponent<Slider>();

            rotation = Camera.main.transform.rotation;
            cam = Camera.main;
            minimap = GameObject.Find("Minimap").GetComponent<Camera>();
            // RESEARCH COROUTINES - only one working
            StartCoroutine(UpdatePanels());
        }
     }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            SelectCursor();
            MoveCamera();
            //RotateCamera();
            Multiselect();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Camera.main.transform.rotation = rotation;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject(-1))
                {
                    Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit1;

                    if (Physics.Raycast(raycast, out hit1, 350))
                    {
                        if (hit1.transform.tag == "Enemy Unit" || hit1.transform.tag == "Selectable" || hit1.transform.tag == "Player 1" ||  hit1.transform.tag == "Foundation" || hit1.transform.tag == "Ground" || hit1.transform.tag == "Yard" || hit1.transform.tag == "Barracks" || hit1.transform.tag == "House" || hit1.transform.tag == "Resource" || hit1.transform.tag == "Fort" || hit1.transform.tag == "Blacksmith" || hit1.transform.tag == "Lumber Yard" || hit1.transform.tag == "Stables")
                        {
                            LeftClick();
                        }
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.F10))
            {
                UI.OpenGameMenuPanel();
            }
            

            if(selectedObj != null)
            {
                if(buildingScript != null) {
                    if (buildingScript.unitType == "Town Hall")
                    {
                        if (townHallScript != null && townHallScript.isTraining)
                        {
                            BuildingProgressSlider.value = townHallScript.i * 10;
                        }
                    }
                    else if (buildingScript.unitType == "Barracks")
                    {
                        if (barracksScript != null && barracksScript.isTraining)
                        {
                            BuildingProgressSlider.value = barracksScript.i * 10;
                        }
                    }
                    else if (selectedObj.tag == "Foundation")
                    {
                        if (foundationScript != null && foundationScript.isBuilding)
                        {
                            BuildingProgressSlider.value = foundationScript.buildPercent;
                        }
                    }
                    else if (selectedObj.tag == "Blacksmith")
                    {
                        if (blacksmithScript.isTraining)
                        {
                            BuildingProgressSlider.value = blacksmithScript.i * 10;
                            // handle
                        }
                    }
                }
            }
        }
    }

    // Camera functions
    void MoveCamera()
    {
        float moveX = Camera.main.transform.position.x;
        float moveY = Camera.main.transform.position.y;
        float moveZ = Camera.main.transform.position.z;

        float moveMinimapX = minimap.transform.position.x;
        float moveMinimapY = minimap.transform.position.y;
        float moveMinimapZ = minimap.transform.position.z;

        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y;

        if(Input.GetKey(KeyCode.A))
        {
            moveX -= panSpeed;
            moveMinimapX -= panSpeed;
        } else if(Input.GetKey(KeyCode.D))
        {
            moveX += panSpeed;
            moveMinimapX += panSpeed;
        }

        if(Input.GetKey(KeyCode.W))
        {
            moveZ += panSpeed;
            moveMinimapZ += panSpeed;
        } else if(Input.GetKey(KeyCode.S))
        {
            moveZ -= panSpeed;
            moveMinimapZ -= panSpeed;
        }

        // Clamping main camera bounds
        moveX = Mathf.Clamp(moveX, xMin, xMax);
        moveY -= Input.GetAxis("Mouse ScrollWheel") * (panSpeed * 20);
        moveY = Mathf.Clamp(moveY, minHeight, maxHeight);
        moveZ = Mathf.Clamp(moveZ, zMin, zMax);

        // Clamping minimap bounds
        moveMinimapX = Mathf.Clamp(moveMinimapX, xMin, xMax);
        moveMinimapZ = Mathf.Clamp(moveMinimapZ, zMin, zMax);

        Vector3 newMinimapPos = new Vector3(moveMinimapX, 175f, moveMinimapZ);
        Vector3 newPos = new Vector3(moveX, moveY, moveZ);

        minimap.transform.position = newMinimapPos;
        Camera.main.transform.position = newPos;
    }

    void RotateCamera()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;

        if (Input.GetMouseButton(2))
        {
            destination.x -= Input.GetAxis("Mouse Y") * rotateAmount;
            destination.x = Mathf.Clamp(destination.x, 20f, 90f);
            destination.y += Input.GetAxis("Mouse X") * rotateAmount;
            //requires clamping -55 to 55 or so
        }

        if (destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * rotateSpeed);
        }
    }

    // Cursor functions
    void SelectCursor()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500, clickableLayer.value))
        {
            bool isAttackable = hit.collider.GetComponent(typeof(IAttackable)) != null;
                       
            if (hit.collider.tag == "Doorway")
            {
                Cursor.SetCursor(doorway, new Vector2(0, 0), CursorMode.Auto);
            }
            else if (hit.collider.tag == "Enemy Unit")
            {
                Cursor.SetCursor(combat, new Vector2(0, 0), CursorMode.Auto);
            }
            else if (hit.collider.tag == "Ground")
            {
                Cursor.SetCursor(combat, new Vector2(0, 0), CursorMode.Auto);
            }
            else if (hit.collider.tag == "Selectable" || hit.collider.gameObject.layer == 11 || hit.collider.gameObject.layer == 12  || hit.collider.tag == "Player 1")
            {
                Cursor.SetCursor(pointer, new Vector2(0, 0), CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(pointer, new Vector2(0, 0), CursorMode.Auto);
            }
        }
        else
        {
            Cursor.SetCursor(pointer, new Vector2(0, 0), CursorMode.Auto);
        }

    }

    // Multiselect box functions
    void Multiselect()
    {
        // If we press the left mouse button, save mouse location and begin UnitSelection
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            mousePosition1 = Input.mousePosition;
        }
        // If we let go of the left mouse button, end UnitSelection
        if (Input.GetMouseButtonUp(0))
            isSelecting = false;
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            GameObject[] units = GameObject.FindGameObjectsWithTag("Selectable");

            // Create a rect from both mouse positions

            var rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));

            var mouse1x = mousePosition1.x;
            var mouse1y = Screen.height - mousePosition1.y;
            var mouse1z = mousePosition1.z;
            Vector3 mouse1 = new Vector3(mouse1x, mouse1y, mouse1z);

            var mouse2x = Input.mousePosition.x;
            var mouse2y = Screen.height - Input.mousePosition.y;
            var mouse2z = Input.mousePosition.z;
            Vector3 mouse2 = new Vector3(mouse2x, mouse2y, mouse2z);

            var selectRect = Utils.GetScreenRect(mouse1, mouse2);

            inUnitSelectionBox = false;
            for (int i = 0; i < units.Length; i++)
            {
                unitPosX = units[i].transform.position.x;
                unitPosY = units[i].transform.position.y;
                unitPosZ = units[i].transform.position.z;

                unitPos = new Vector3(unitPosX, unitPosY, unitPosZ);
                Vector3 screenPos = cam.WorldToScreenPoint(unitPos);

                // Adds all units in UnitSelection box to selected
                if (selectRect.Contains(screenPos, true))
                {
                    selectedInfo = units[i].GetComponent<UnitSelection>();
                    unitScript = units[i].GetComponent<UnitController>();

                    if(!unitScript.isDead) {
                        isSelected = true;

                        selectedInfo.selected = true;
                        selectedInfo.transform.GetChild(2).gameObject.GetComponent<Projector>().material.SetColor("_Color", Color.green);
                        selectedInfo.transform.GetChild(2).gameObject.SetActive(true);

                        playerAudio.clip = unitAudioClip;
                        if(unitScript.unitType == "Worker") {
                            // unitAudio.volume = 0.5f;
                            UI.WorkerSelect();
                        } else if (unitScript.unitType == "Swordsman") {
                            UI.SwordsmanSelect();
                        } else if (unitScript.unitType == "Footman") {
                            UI.FootmanSelect();
                        }
                        inUnitSelectionBox = true;
                    }
                }
            }

            if(inUnitSelectionBox) {
                playerAudio.Play();
            }
        }
    }
      
    public void UpdateUnitPanel()
    { 
            // FFA500
        
        if (RC.artisanArmourSmithing) {
            UI.armour1.GetComponent<Image>().color = new Color32(255,165,0,255);
            UI.armour2.GetComponent<Image>().color = new Color32(255,165,0,255);
            UI.armour3.GetComponent<Image>().color = new Color32(255,165,0,255);
            UI.armour4.GetComponent<Image>().color = new Color32(255,165,0,255);
            UI.armour5.GetComponent<Image>().color = new Color32(255,165,0,255);
        } else if (RC.basicArmourSmithing) {
            UI.armour1.GetComponent<Image>().color = new Color32(114,160,193,255);
            UI.armour2.GetComponent<Image>().color = new Color32(114,160,193,255);
            UI.armour3.GetComponent<Image>().color = new Color32(114,160,193,255);
            UI.armour4.GetComponent<Image>().color = new Color32(114,160,193,255);
            UI.armour5.GetComponent<Image>().color = new Color32(114,160,193,255);
        } else {
            UI.armour1.GetComponent<Image>().color = new Color32(205,127,50,255);
            UI.armour2.GetComponent<Image>().color = new Color32(205,127,50,255);
            UI.armour3.GetComponent<Image>().color = new Color32(205,127,50,255);
            UI.armour4.GetComponent<Image>().color = new Color32(205,127,50,255);
            UI.armour5.GetComponent<Image>().color = new Color32(205,127,50,255);
        }

        if(unitScript.armour <= 0.0f) {
            UI.armour1.alpha = 0;
            UI.armour2.alpha = 0;
            UI.armour3.alpha = 0;
            UI.armour4.alpha = 0;
            UI.armour5.alpha = 0;
        } else if(unitScript.armour <= 1.0f) {
            UI.armour1.alpha = 1;
            UI.armour2.alpha = 0;
            UI.armour3.alpha = 0;
            UI.armour4.alpha = 0;
            UI.armour5.alpha = 0;
        } else if(unitScript.armour <= 2.0f) {
            UI.armour1.alpha = 1;
            UI.armour2.alpha = 1;
            UI.armour3.alpha = 0;
            UI.armour4.alpha = 0;
            UI.armour5.alpha = 0;
        } else if(unitScript.armour <= 3.0f) {
            UI.armour1.alpha = 1;
            UI.armour2.alpha = 1;
            UI.armour3.alpha = 1;
            UI.armour4.alpha = 0;
            UI.armour5.alpha = 0;
        } else if(unitScript.armour <= 4.0f) {
            UI.armour1.alpha = 1;
            UI.armour2.alpha = 1;
            UI.armour3.alpha = 1;
            UI.armour4.alpha = 1;
            UI.armour5.alpha = 0;
        } else if(unitScript.armour <= 5.0f) {
            UI.armour1.alpha = 1;
            UI.armour2.alpha = 1;
            UI.armour3.alpha = 1;
            UI.armour4.alpha = 1;
            UI.armour5.alpha = 1;
        }
        // UI Functions
        // unitScript = selectedObj.GetComponent<UnitController>();
        Image icon = unitIcon.GetComponent<Image>();
        icon.sprite = unitScript.unitIcon;

        HB.maxValue = unitScript.maxHealth;
        HB.value = unitScript.health;

        EB.maxValue = unitScript.maxEnergy;
        EB.value = unitScript.energy;

        healthDisp.text = "HEALTH: " + unitScript.health;
        energyDisp.text = "ENERGY: " + unitScript.energy;

        nameDisp.text = unitScript.unitName;
        unitName.text = unitScript.unitType;
        rankDisp.text = unitScript.unitRank;
        killDisp.text = "Kills: " + unitScript.unitKills;

        weaponDisp.text = unitScript.weapon;
        armourDisp.text = unitScript.armourType;
        NodeManager.ResourceTypes resourceType = selectedInfo.heldResourceType;
        itemDisp.text = resourceType + ": " + selectedInfo.heldResource;
    }

    public void UpdateBuildingPanel()
    {
        // UI Functions
        //ERROR - Object reference not set to an instance of an object - when you select a tree that is being harvested then switch to a villager
   
        if(selectedObj != null ) {
            buildingScript = selectedObj.GetComponent<BuildingController>();
            Image icon = buildingIcon.GetComponent<Image>();
            icon.sprite = buildingScript.icon;

            buildingHB.maxValue = buildingScript.maxHealth;
            buildingHB.value = buildingScript.health;

            buildingEB.maxValue = buildingScript.maxEnergy;
            buildingEB.value = buildingScript.energy;

            buildingHealthDisp.text = "HEALTH: " + buildingScript.health;
            buildingEnergyDisp.text = "ENERGY: " + buildingScript.energy;

            buildingNameDisp.text = buildingScript.unitName;
            buildingName.text = buildingScript.unitType;
            buildingRankDisp.text = buildingScript.unitRank;
            buildingKillDisp.text = "Kills: " + buildingScript.unitKills;

            buildingWeaponDisp.text = buildingScript.weapon;
            buildingArmourDisp.text = buildingScript.armour;

            buildingItemDisp.text = "None";
            if (selectedObj.transform.tag == "Resource")
            {
                nodeScript = selectedObj.GetComponent<NodeManager>();
                buildingItemDisp.text = nodeScript.resourceType + ": " + nodeScript.availableResource;
            }
        }
    }

    // Left click controller
    public void LeftClick()
    {
        // Check for all selectable units in the game
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 350))
        {
            if (hit.collider.tag == "Enemy Unit" && (!Input.GetKey(KeyCode.LeftShift)))
            {
                selectedObj = hit.collider.gameObject;
                selectedInfo = selectedObj.GetComponent<UnitSelection>();
                unitScript = selectedObj.GetComponent<UnitController>();
                if (selectedInfo.selected == true)
                {
                    DeselectUnits();
                }
                else 
                {
                    selectedInfo.selected = true;

                    //  OPEN ENEMY PANELS!!

                    
                    // UnitSelection indicators
                    selectedObj.transform.GetChild(2).gameObject.GetComponent<Projector>().material.SetColor("_Color", Color.red);
                    selectedObj.transform.GetChild(2).gameObject.SetActive(true);
                    // unitAudio = selectedObj.GetComponent<AudioSource>();
                    // unitAudio.clip = unitAudioClip;
                    // unitAudio.Play();
                    // isSelected = true;
                    UI.EnemySelect();
                }
            }
            else if (hit.collider.tag == "Selectable")
            {
                selectedObj = hit.collider.gameObject;
                selectedObj.transform.GetChild(2).gameObject.GetComponent<Projector>().material.SetColor("_Color", Color.green);
                selectedInfo = selectedObj.GetComponent<UnitSelection>();
                unitScript = selectedObj.GetComponent<UnitController>();
                if (selectedInfo.selected == true)
                {
                    DeselectUnits();
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    if(!unitScript.isDead) {
                        selectedInfo.selected = true;

                        // UnitSelection indicators
                        selectedObj.transform.GetChild(2).gameObject.SetActive(true);
                        playerAudio.clip = unitAudioClip;
                        playerAudio.Play();
                        if(!unitScript.isDead) {
                            isSelected = true;
                        }

                        if(unitScript.unitType == "Worker") {
                            UI.WorkerSelect();
                        } else if (unitScript.unitType == "Swordsman") {
                            UI.SwordsmanSelect();
                        } else if (unitScript.unitType == "Footman") {
                            UI.FootmanSelect();
                        }
                    }
                }
                else if (hit.collider.tag == "Ground" && (!Input.GetKey(KeyCode.LeftShift)))
                {
                    if(selectedObjects.Length >= 0)
                    {
                        DeselectUnits();
                        UI.CloseAllPanels();
                    }
                } 
                else
                {
                    if(!unitScript.isDead) {
                        GameObject[] selectedIndicators = GameObject.FindGameObjectsWithTag("SelectedIndicator");
                        for (int j = 0; j < selectedIndicators.Length; j++)
                        {
                            selectedIndicators[j].transform.gameObject.SetActive(false);
                            selectedIndicators[j].transform.parent.GetComponent<UnitSelection>().selected = false;
                        }

                        selectedInfo.selected = true;

                        // UnitSelection indicators
                        selectedObj.transform.GetChild(2).gameObject.SetActive(true);
                        playerAudio.clip = unitAudioClip;
                        playerAudio.Play();
                        if(!unitScript.isDead) {
                            isSelected = true;
                        }

                        if(unitScript.unitType == "Worker") {
                            UI.WorkerSelect();
                        } else if (unitScript.unitType == "Swordsman") {
                            UI.SwordsmanSelect();
                        } else if (unitScript.unitType == "Footman") {
                            UI.FootmanSelect();
                        }
                    }
                }
            }
            else if (hit.collider.tag == "Enemy Unit" || hit.collider.tag == "Player 1" || hit.collider.tag == "Foundation" || hit.collider.tag == "Barracks" || hit.collider.tag == "House" || hit.collider.tag == "Resource" || hit.collider.tag == "Fort" || hit.collider.tag == "Blacksmith" || hit.transform.tag == "Lumber Yard" || hit.transform.tag == "Stables" )
            {
                UI.CloseAllPanels();
                if (selectedObjects.Length >= 0)
                {
                    DeselectUnits();
                }
                selectedObj = hit.collider.gameObject;

                if(selectedObj.tag == "Player 1")
                {
                    townHallScript = selectedObj.GetComponent<TownHallController>();
                    isTraining = townHallScript.isTraining;
                    SwapProgressIcon();
                    if (isTraining)
                    {
                        UI.TownHallTraining();
                    }
                    else
                    {
                        UI.TownHallSelect();
                    }
                } else if (selectedObj.tag == "House")
                {
                    UI.HouseSelect();
                } else if (selectedObj.tag == "Resource")
                {
                    UI.ResourceSelect();
                }
                else if (selectedObj.tag == "Foundation")
                {
                    foundationScript = selectedObj.GetComponent<FoundationController>();
                    isBuilding = foundationScript.isBuilding;
                    if (isBuilding)
                    {
                        SwapProgressIcon();
                        UI.FoundationBuilding();
                    }
                    else
                    {
                        UI.FoundationSelect();
                    }
                } 
                else if (selectedObj.tag == "Blacksmith") {
                    blacksmithScript = selectedObj.GetComponent<BlacksmithController>();
                    isTraining = blacksmithScript.isTraining;
                    SwapProgressIcon();
                    if (isTraining)
                    {
                        UI.BlacksmithTraining();
                    }
                    else
                    {
                        UI.BlacksmithSelect();
                    }
                }
                else if (selectedObj.tag == "Lumber Yard") {
                    UI.LumberYardSelect();
                }
                else if (selectedObj.tag == "Barracks") {
                    barracksScript = selectedObj.GetComponent<BarracksController>();
                    isTraining = barracksScript.isTraining;
                    SwapProgressIcon();
                    if (isTraining)
                    {
                        UI.BarracksTraining();
                    }
                    else
                    {
                        UI.BarracksSelect();
                    }
                }
                else if (selectedObj.tag == "Stables") {
                    UI.StablesSelect();
                }
            }
            else if (hit.collider.tag != "Selectable" && (!Input.GetKey(KeyCode.LeftShift)))
            {
                DeselectUnits();
                UI.CloseAllPanels();
            }
        }
    }

    void SwapProgressIcon()
    {
        progressIcon = GameObject.Find("ProgressIcon").GetComponent<Image>();
        // UI Functions
        buildingScript = selectedObj.GetComponent<BuildingController>();
        if (buildingScript.unitType == "Town Hall")
        {
            progressIcon.sprite = townHallScript.villagerPrefab.GetComponent<UnitController>().unitIcon;
        } 
        else if (buildingScript.unitType == "Barracks")
        {
            if(buildingScript.GetComponent<BarracksController>().unit == "Footman") {
                progressIcon.sprite = barracksScript.footmanPrefab.GetComponent<UnitController>().unitIcon;
            } else if (buildingScript.GetComponent<BarracksController>().unit == "Swordsman") {
                progressIcon.sprite = barracksScript.swordsmanPrefab.GetComponent<UnitController>().unitIcon;
            }
        } else if (buildingScript.tag == "Foundation")
        {
            foundationScript = selectedObj.GetComponent<FoundationController>();
            progressIcon.sprite = foundationScript.buildingPrefab.GetComponent<BuildingController>().icon;
        }
        else
        {
            progressIcon.sprite = buildingScript.icon;
        }
    }

    private void DeselectUnits()
    {
        selectedObj = null;
        UI.CloseAllPanels();
        for (int i = 0; i < selectedObjects.Length; i++)
        {
            // Grabs all objects in selected array and deselects them
            selectedInfo = selectedObjects[i].GetComponent<UnitSelection>();
            selectedInfo.selected = false;
        }
        GameObject[] selectedIndicators = GameObject.FindGameObjectsWithTag("SelectedIndicator");

        // UnitSelection indicators
        for (int j = 0; j < selectedIndicators.Length; j++)
        {
            // Turns the unit UnitSelection indicator off
            selectedIndicators[j].transform.gameObject.SetActive(false);

            // Deselects the unit
            selectedIndicators[j].transform.parent.GetComponent<UnitSelection>().selected = false;
        }

        isSelected = false;
    }

    IEnumerator UpdatePanels()
    {
        while (true)
        {
            if(UI.panelOpen == 1) {
                UpdateUnitPanel();
                yield return new WaitForSeconds(0.01f);
            } else if (UI.panelOpen == 2){
                UpdateBuildingPanel();
                yield return new WaitForSeconds(0.01f);
            } else {
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}

[System.Serializable]
public class EventVector3 : UnityEvent<Vector3> { }

[System.Serializable]
public class EventGameObject : UnityEvent<GameObject> { }


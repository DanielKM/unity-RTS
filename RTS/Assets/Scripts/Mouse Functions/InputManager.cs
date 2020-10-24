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
    
     public List<Renderer> visibleRenderers = new List<Renderer>();
     //Looking for visible objects for double click

    private GameObject player;
    private GameObject team;

    // World Bounds
   public float xMin, xMax, yMin, yMax, zMin, zMax;

    // Reference Scripts

    UnitList unitList;
    UnitController unitScript;
    UnitSelection selectScript;
    BuildingController buildingScript;
    NodeManager nodeScript;
    BuildingButtonController buildingButtonScript;
    TownHallController townHallScript;
    BarracksController barracksScript;
    BlacksmithController blacksmithScript;
    FoundationController foundationScript;

    UnitButtonController unitButtonController;

    ResearchController RC;
    UIController UI;
    PauseMenu PM;

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
    public Text taskDisp;

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

    // MODES
    public bool rotating = false;
    public bool attackMoving = false;
    public bool patrolling = false;

    // Cursor variables
    public LayerMask clickableLayer;
    public int mask;

    public Texture2D pointer;
    public Texture2D finger;
    public Texture2D move;

    public Texture2D doorway;
    public Texture2D sword;
    public Texture2D mine;
    
    public GameObject cursorHit;
    private GameObject currentCursorHit;

    // New
    public EventVector3 OnClickEnvironment;
    public EventGameObject OnClickAttackable;
    
    // Multiselect variables
    bool isSelecting = false;
    bool selectionBoxOpen = false;
    public bool isUnit;

    Vector3 mousePosition1;
    // All units which are selected
    public List<GameObject> selectedObjects;

    // Control groups
    public List<GameObject> controlGroup1;
    public List<GameObject> controlGroup2;
    public List<GameObject> controlGroup3;
    public List<GameObject> controlGroup4;
    public List<GameObject> controlGroup5;
    public List<GameObject> controlGroup6;
    public List<GameObject> controlGroup7;
    public List<GameObject> controlGroup8;
    public List<GameObject> controlGroup9;
    public List<GameObject> controlGroup0;

    // All units in the game that are selectable
    private GameObject[] units;
    GameObject closestSelectableUnit;

    // Coordinates of all units
    private Vector3 unitPos;
    private float unitPosX;
    private float unitPosY;
    private float unitPosZ;

    // Last Click Time
    private float lastclickTime;
    private const float DOUBLE_CLICK_TIME = .2f;

    // Grab the main camera
    Camera cam;
    Camera minimap;

    public UnitSelection selectedInfo;
    public GameObject selectedObj;

    void Awake() {
        unitList = GameObject.Find("Game").GetComponent<UnitList>();
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            team = GameObject.Find("Faction");
            player = GameObject.FindGameObjectWithTag("Player");

            unitButtonController = player.GetComponent<UnitButtonController>();
            playerAudio = GameObject.FindGameObjectWithTag("Main Audio").GetComponent<AudioSource>();
            UI = player.GetComponent<UIController>();
            RC = team.GetComponent<ResearchController>();
            PM = GameObject.Find("GameMenu").GetComponent<PauseMenu>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SelectCursor();
        if(currentScene.name != "Main Menu") {
            mask =~ LayerMask.GetMask("FogOfWar");
            team = GameObject.Find("Faction");
            player = GameObject.FindGameObjectWithTag("Player");

            unitButtonController = player.GetComponent<UnitButtonController>();
            playerAudio = GameObject.FindGameObjectWithTag("Main Audio").GetComponent<AudioSource>();
            UI = player.GetComponent<UIController>();
            RC = team.GetComponent<ResearchController>();
            PM = GameObject.Find("GameMenu").GetComponent<PauseMenu>();

            unitIcon = GameObject.Find("UnitIcon");
            BuildingProgressBar = GameObject.Find("BuildingProgressBar");
            BuildingProgressSlider = BuildingProgressBar.GetComponent<Slider>();

            rotation = Camera.main.transform.rotation;
            cam = Camera.main;
            minimap = GameObject.Find("Minimap").GetComponent<Camera>();
            StartCoroutine(UpdatePanels());
        }
     }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SelectCursor();
        if(currentScene.name != "Main Menu") {
            MoveCamera();
            //RotateCamera();
            Multiselect();
            ActivateControlGroups();

            // Switch to building rotation mode
            if(Input.GetKeyDown("r") && unitButtonController.currentPlaceableObject) {
                rotating = !rotating;
                if(rotating) {
                    UI.NoModeText();
                    UI.RotationModeText();
                } else {
                    UI.NoModeText();
                    UI.PlacementModeText();
                }
            }

            // Switch to attack mode
            if(Input.GetKeyDown("m") && selectedObjects.Count > 0) {
                attackMoving = !attackMoving;
                if(attackMoving) {
                    UI.NoModeText();
                    UI.AttackMovementText();
                } else {
                    UI.NoModeText();
                    UI.StandardMovementText();
                }
            }

            // Reset camera angle
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Camera.main.transform.rotation = rotation;
            }

            // Handle left clicking
            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject(-1))
                {
                    float timeSinceLastClick = Time.time - lastclickTime;
                    string clickType = "single";
                    if(timeSinceLastClick <= DOUBLE_CLICK_TIME) {
                        clickType = "double";
                        LeftClick(clickType);
                    } else {
                        clickType = "single";
                        LeftClick(clickType);
                    }
                    lastclickTime = Time.time;
                }
            }

            if(Input.GetMouseButtonUp(0)) {
                if(!inUnitSelectionBox && selectedObjects.Count == 0 && !selectedObj) {
                    UI.CloseAllPanels();
                }
            }

            // Open game menu
            if(Input.GetKeyDown(KeyCode.F10))
            {
                UI.OpenGameMenuPanel();
            }

            // If a building isnt being placed, open game menu
            if(!unitButtonController.currentPlaceableObject) {
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    UI.OpenGameMenuPanel();
                } 
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

    void ActivateControlGroups() {
        if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
            if(Input.GetKeyDown(KeyCode.Alpha1)) {
                SaveControlGroup(1);
            } else if(Input.GetKeyDown(KeyCode.Alpha2)) {
                SaveControlGroup(2);
            } else if(Input.GetKeyDown(KeyCode.Alpha3)) {
                SaveControlGroup(3);
            } else if(Input.GetKeyDown(KeyCode.Alpha4)) {
                SaveControlGroup(4);
            } else if(Input.GetKeyDown(KeyCode.Alpha5)) {
                SaveControlGroup(5);
            } else if(Input.GetKeyDown(KeyCode.Alpha6)) {
                SaveControlGroup(6);
            } else if(Input.GetKeyDown(KeyCode.Alpha7)) {
                SaveControlGroup(7);
            } else if(Input.GetKeyDown(KeyCode.Alpha8)) {
                SaveControlGroup(8);
            } else if(Input.GetKeyDown(KeyCode.Alpha9)) {
                SaveControlGroup(9);
            }
        } else {
            if(Input.GetKeyDown(KeyCode.Alpha1) ) {
                LoadControlGroup(1);
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                LoadControlGroup(2);
            } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                LoadControlGroup(3);
            } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                LoadControlGroup(4);
            } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
                LoadControlGroup(5);
            } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
                LoadControlGroup(6);
            } else if (Input.GetKeyDown(KeyCode.Alpha7)) {
                LoadControlGroup(7);
            } else if (Input.GetKeyDown(KeyCode.Alpha8)) {
                LoadControlGroup(8);
            } else if (Input.GetKeyDown(KeyCode.Alpha9)) {
                LoadControlGroup(9);
            } else if (Input.GetKeyDown(KeyCode.Alpha0)) {
                LoadControlGroup(0);
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
        if(rotating) { 
            moveY -= 0;
        } else {
            moveY -= Input.GetAxis("Mouse ScrollWheel") * (panSpeed * 20);
        }
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
        Vector3 mousePos = new Vector3(Input.mousePosition.x + 16.0f, Input.mousePosition.y, Input.mousePosition.z);
        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 500, clickableLayer.value))
        {      
            if(attackMoving) {
                Cursor.SetCursor(sword, new Vector2(0, 0), CursorMode.Auto);
            } else if (hit.collider.tag == "Doorway")
            {
                Cursor.SetCursor(doorway, new Vector2(0, 0), CursorMode.Auto);
            }
            else if (hit.collider.tag == "Enemy Unit")
            {
                Cursor.SetCursor(sword, new Vector2(0, 0), CursorMode.Auto);
            }
            else if (hit.collider.tag == "Ground")
            {
                Cursor.SetCursor(pointer, new Vector2(0, 0), CursorMode.Auto);
            }
            else if (hit.collider.tag == "Resource" || hit.collider.tag == "Foundation")
            {
                Cursor.SetCursor(mine, new Vector2(0, 0), CursorMode.Auto);
            }
            else if (hit.collider.tag == "Selectable" || hit.collider.gameObject.layer == 11 || hit.collider.tag == "Player 1")
            {
                Cursor.SetCursor(finger, new Vector2(0, 0), CursorMode.Auto);
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

    void Multiselect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            mousePosition1 = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        { 
            isSelecting = false;       
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 350, mask))
            {
                if(!selectionBoxOpen) {
                    StartCoroutine(ClickCursorHit(hit));
                }
            }    
        }
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            // if(currentlySelecting) {

            // }
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
            if(selectRect.size.x * selectRect.size.y > 150) {  
                if(!selectionBoxOpen) {
                    units = GameObject.FindGameObjectsWithTag("Selectable");
                    if(selectedObj) {
                        selectedObj.transform.GetChild(0).gameObject.SetActive(false);
                        selectedObj = null;
                    }
                }
                
                selectionBoxOpen = true;
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
                        if(!selectedObjects.Contains(units[i])) {
                            SelectMultipleUnits (units[i]);
                        }
                    } else {
                        if(!Input.GetKey(KeyCode.LeftShift)) {
                            if(selectedObjects.Contains(units[i])) {
                                DeselectSingleUnit(units[i]);
                            }
                        }
                    }
                }
            } else {
                selectionBoxOpen = false;
            }
        }
    }
      

    void SelectMultipleUnits (GameObject unit) {
        selectedInfo = unit.GetComponent<UnitSelection>();
        unitScript = unit.GetComponent<UnitController>();

        if(!unitScript.isDead) {
            UI.DisplaySelectedObjects(unit);
            if(!selectedObjects.Contains(unit)) {
                selectedObjects.Add(unit);
            }
            selectedInfo.selected = true;
            selectedInfo.transform.GetChild(2).gameObject.GetComponent<Projector>().material.SetColor("_Color", Color.green);
            selectedInfo.transform.GetChild(2).gameObject.SetActive(true);

            playerAudio.clip = unitAudioClip;
            if(unitScript.unitType == "Worker") {
                UI.WorkerSelect();
            } else if (unitScript.unitType == "Swordsman") {
                UI.SwordsmanSelect();
            } else if (unitScript.unitType == "Footman") {
                UI.FootmanSelect();
            } else if (unitScript.unitType == "Archer") {
                UI.ArcherSelect();
            } else if (unitScript.unitType == "Outrider") {
                UI.OutriderSelect();
            } else if (unitScript.unitType == "Knight") {
                UI.KnightSelect();
            } else if (unitScript.unitType == "Wizard") {
                UI.WizardSelect();
            }
            UI.StandardMovementText();
            inUnitSelectionBox = true;
        } else {
            if(selectedObjects.Contains(unit)) {
                selectedObjects.Remove(unit);
            }
        }
    }

    public void UpdateUnitPanel()
    { 
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

        string currentTask = "";
        if(selectedInfo.task == ActionList.Attacking) {
            currentTask = "Attacking";
        } else if(selectedInfo.task == ActionList.Gathering) {
            currentTask = "Gathering";
        } else if(selectedInfo.task == ActionList.Moving) {
            currentTask = "Moving";
        } else if(selectedInfo.task == ActionList.Idle) {
            currentTask = "Idle";
        } else if(selectedInfo.task == ActionList.Building) {
            currentTask = "Building";
        } else if(selectedInfo.task == ActionList.Delivering) {
            currentTask = "Delivering";
        }
        taskDisp.text = currentTask;

    }

    public void UpdateBuildingPanel()
    {
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

    public void LeftClick(string clickType)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 350, mask))
        {
            closestSelectableUnit = CheckClosestUnitFromRaycast(hit);
            if (hit.collider.tag == "Ground" && (!Input.GetKey(KeyCode.LeftShift)) && !closestSelectableUnit)
            {
                if(!Input.GetKey(KeyCode.LeftShift)) {
                    DeselectUnits();
                }
                UI.CloseAllPanels();
            } 
            else if (hit.collider.tag == "Enemy Unit" && (!Input.GetKey(KeyCode.LeftShift)))
            {
                if(!Input.GetKey(KeyCode.LeftShift)) {
                    DeselectUnits();
                }
                selectedObj = hit.collider.gameObject;
                selectedInfo = selectedObj.GetComponent<UnitSelection>();
                unitScript = selectedObj.GetComponent<UnitController>();
            
                selectedInfo.selected = true;
                //  OPEN ENEMY PANELS!!
                selectedObj.transform.GetChild(2).gameObject.GetComponent<Projector>().material.SetColor("_Color", Color.red);
                selectedObj.transform.GetChild(2).gameObject.SetActive(true);
                UI.EnemySelect();
            }
            else if (hit.collider.tag == "Selectable")
            {
                if(clickType == "double") {
                    Renderer[] sceneRenderers = FindObjectsOfType<Renderer>();
                    visibleRenderers.Clear();
                    for(int i = 0; i < sceneRenderers.Length; i++) {
                        if(IsVisible(sceneRenderers[i])) {
                            if (sceneRenderers[i].transform.parent) {
                                if (sceneRenderers[i].transform.parent.gameObject) {
                                    if(sceneRenderers[i].transform.parent.gameObject.GetComponent<UnitController>()) {
                                        if(sceneRenderers[i].transform.parent.gameObject.GetComponent<UnitController>().unitType == hit.collider.gameObject.GetComponent<UnitController>().unitType) {
                                            visibleRenderers.Add(sceneRenderers[i]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach( Renderer renderer in visibleRenderers) {
                        GameObject doubleClickSelection = renderer.transform.parent.gameObject;
                        if (doubleClickSelection.GetComponent<UnitController>()) {
                            if(hit.collider.gameObject.GetComponent<UnitController>().unitType == doubleClickSelection.GetComponent<UnitController>().unitType) {
                                SelectMultipleUnits(doubleClickSelection);
                            }
                        }
                    }
                } else {
                    if(selectedObjects.Contains(hit.collider.gameObject)) {
                        DeselectSingleUnit(hit.collider.gameObject);
                    } else {
                        SelectMultipleUnits(hit.collider.gameObject);
                    }
                }
            }
            else if (hit.collider.tag == "Player 1" || hit.collider.tag == "Foundation" || hit.collider.tag == "Barracks" || hit.collider.tag == "House" || hit.collider.tag == "Resource" || hit.collider.tag == "Fort" || hit.collider.tag == "Blacksmith" || hit.transform.tag == "Stables" )
            {
                // Select the building or unit
                selectedObj = hit.collider.gameObject;
                SelectBuilding(selectedObj);
            }

            else if (hit.collider.tag != "Selectable" && (!Input.GetKey(KeyCode.LeftShift)) && !closestSelectableUnit)
            {
                if(!Input.GetKey(KeyCode.LeftShift)) {
                    DeselectUnits();
                }
                UI.CloseAllPanels();
            } else if (closestSelectableUnit) {
                if(!Input.GetKey(KeyCode.LeftShift)) {
                    DeselectUnits();
                }
                if(selectedObjects.Contains(closestSelectableUnit)) {
                    DeselectSingleUnit(closestSelectableUnit);
                } else {
                    SelectMultipleUnits(closestSelectableUnit);
                }
            }
        }
    }

    void SelectBuilding(GameObject building) {
        // Close all panels
        UI.CloseAllPanels();

        // Deselect all units
        if (selectedObjects.Count >= 0)
        {
            DeselectUnits();
        }
        selectedObj = building;
        // Turn on selection circle
        selectedObj.transform.GetChild(0).gameObject.SetActive(true);

        if(selectedObj.tag == "Player 1")
        {
            buildingScript = selectedObj.GetComponent<BuildingController>();
            if(buildingScript.unitType == "Lumber Yard") {
                UI.LumberYardSelect();
            } else if (buildingScript.unitType == "Town Hall") {
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
            }
        } else if (selectedObj.tag == "House")
        {
            UI.HouseSelect();
        } else if (selectedObj.tag == "Resource")
        {
            UI.ResourceSelect();
        } else if (selectedObj.tag == "Blacksmith") {
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
        } else if (selectedObj.tag == "Barracks") {
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
        } else if (selectedObj.tag == "Stables") {
            UI.StablesSelect();
        } else if (selectedObj.tag == "Foundation")
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
    }

    private void DeselectSingleUnit(GameObject go) {
        UI.RemoveSelectedObjects(go);
        selectedObjects.Remove(go);
        selectedInfo = go.GetComponent<UnitSelection>();
        unitScript = go.GetComponent<UnitController>();
        selectedInfo.selected = false;
        selectedInfo.transform.GetChild(2).gameObject.GetComponent<Projector>().material.SetColor("_Color", Color.green);
        selectedInfo.transform.GetChild(2).gameObject.SetActive(false);
        go.transform.GetChild(2).gameObject.SetActive(false); 
        go.GetComponent<UnitSelection>().selected = false;
        if(!go.GetComponent<UnitController>().isDead) {
            isSelected = false;
        }
    }
    GameObject CheckClosestUnitFromRaycast(RaycastHit hit) {
        // Find the closest enemy
        GameObject closestSelectableObject = null;
        float closestDistance = 1f;
        Vector3 position = hit.point;

        foreach(GameObject targetEnemy in unitList.selectableUnits)
        {
            if(!targetEnemy.GetComponent<UnitController>().isDead) {
                Vector3 direction = targetEnemy.transform.position - position;
                float distance = direction.sqrMagnitude;
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSelectableObject = targetEnemy;
                }
            }
        }
        return closestSelectableObject;
    }

    bool IsVisible(Renderer renderer) {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return (GeometryUtility.TestPlanesAABB(planes, renderer.bounds)) ? true : false;
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
            } else if (buildingScript.GetComponent<BarracksController>().unit == "Archer") {
                progressIcon.sprite = barracksScript.archerPrefab.GetComponent<UnitController>().unitIcon;
            } else if (buildingScript.GetComponent<BarracksController>().unit == "Outrider") {
                progressIcon.sprite = barracksScript.outriderPrefab.GetComponent<UnitController>().unitIcon;
            } else if (buildingScript.GetComponent<BarracksController>().unit == "Knight") {
                progressIcon.sprite = barracksScript.knightPrefab.GetComponent<UnitController>().unitIcon;
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
        UI.ResetSelectionIcons();
        selectedObj = null;
        UI.CloseAllPanels();
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            // Grabs all objects in selected array and deselects them
            selectedInfo = selectedObjects[i].GetComponent<UnitSelection>();
            selectedInfo.selected = false;
        }
        selectedObjects.Clear();
        GameObject[] selectedIndicators = GameObject.FindGameObjectsWithTag("SelectedIndicator");
        if(selectedIndicators.Length > 0) {
            // UnitSelection indicators
            for (int j = 0; j < selectedIndicators.Length; j++)
            {
                // Turns the unit UnitSelection indicator off
                selectedIndicators[j].transform.gameObject.SetActive(false);

                // Deselects the unit
                if(selectedIndicators[j].transform.parent.GetComponent<UnitSelection>()) {
                    selectedIndicators[j].transform.parent.GetComponent<UnitSelection>().selected = false;
                }
            }
        }
        isSelected = false;
        attackMoving = false;
    }

    void SaveControlGroup(int number) {
        List<GameObject> currentList = new List<GameObject>();
        if(number == 1) {
            currentList = controlGroup1;
        } else if (number == 2) {
            currentList = controlGroup2;
        } else if (number == 3) {
            currentList = controlGroup3;
        } else if (number == 4) {
            currentList = controlGroup4;
        } else if (number == 5) {
            currentList = controlGroup5;
        } else if (number == 6) {
            currentList = controlGroup6;
        } else if (number == 7) {
            currentList = controlGroup7;
        } else if (number == 8) {
            currentList = controlGroup8;
        } else if (number == 9) {
            currentList = controlGroup9;
        } else if (number == 0) {
            currentList = controlGroup0;
        }
        currentList.Clear();
        foreach (GameObject go in selectedObjects) {
            currentList.Add(go);
        }
    }

    void LoadControlGroup(int number) {
        
        if(!Input.GetKey(KeyCode.LeftShift)) {
            DeselectUnits();
        }
        List<GameObject> currentList = new List<GameObject>();
        if(number == 1) {
            currentList = controlGroup1;
        } else if (number == 2) {
            currentList = controlGroup2;
        } else if (number == 3) {
            currentList = controlGroup3;
        } else if (number == 4) {
            currentList = controlGroup4;
        } else if (number == 5) {
            currentList = controlGroup5;
        } else if (number == 6) {
            currentList = controlGroup6;
        } else if (number == 7) {
            currentList = controlGroup7;
        } else if (number == 8) {
            currentList = controlGroup8;
        } else if (number == 9) {
            currentList = controlGroup9;
        } else if (number == 0) {
            currentList = controlGroup0;
        }
        
        foreach (GameObject go in currentList) {
            SelectMultipleUnits(go);
        }
    }

    public IEnumerator ClickCursorHit(RaycastHit hit) {
        Destroy(currentCursorHit);
        if(!PM.gamePaused) {
            currentCursorHit = Instantiate(cursorHit);
            currentCursorHit.transform.position = new Vector3 (hit.point.x, hit.point.y + 2.0f, hit.point.z);
            yield return new WaitForSeconds(0.1f);
            Destroy(currentCursorHit);
        }
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


﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    private GameObject player;

    // World Bounds
   public float xMin, xMax, yMin, yMax, zMin, zMax;

    // Reference Scripts
    UnitController unitScript;
    Selection selectScript;
    BuildingController buildingScript;
    NodeManager nodeScript;
    BuildingButtonController buildingButtonScript;
    TownHallController townHallScript;
    FoundationController foundationScript;

    // UI FOR UNITS
    private CanvasGroup UnitPanel;
    private CanvasGroup UnitActionPanel;
    private CanvasGroup PeasantPanel;
 //   private CanvasGroup GameMenuPanel;

    private AudioSource peasantAudio;
    public AudioClip peasantAudioClip;

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

    private bool isSelected;

    // UI FOR BUILDINGS
    private CanvasGroup BuildingPanel;
    private CanvasGroup BuildingActionPanel;
    private CanvasGroup AdvancedBuildingsPanel;
    private CanvasGroup BuildingProgressPanel;
    
    private GameObject PeasantProgressBar;
    public Slider PeasantProgressSlider;

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

    private Selection selectedInfo;
    public GameObject selectedObj;
        
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // Unit panels
        UnitPanel = GameObject.Find("UnitPanel").GetComponent<CanvasGroup>();
        UnitActionPanel = GameObject.Find("BasicBuildingsPanel").GetComponent<CanvasGroup>();
        AdvancedBuildingsPanel = GameObject.Find("AdvancedBuildingsPanel").GetComponent<CanvasGroup>();
        PeasantPanel = GameObject.Find("PeasantPanel").GetComponent<CanvasGroup>();
       // GameMenuPanel = GameObject.Find("GameMenu").GetComponent<CanvasGroup>();

        // Building panels
        BuildingPanel = GameObject.Find("BuildingPanel").GetComponent<CanvasGroup>();
        BuildingActionPanel = GameObject.Find("BuildingActions").GetComponent<CanvasGroup>();
        BuildingProgressPanel = GameObject.Find("BuildingProgressPanel").GetComponent<CanvasGroup>();

       // progressIcon = GameObject.Find("ProgressIcon").GetComponent<Image>();
       // foundationScript = selectedObj.GetComponent<FoundationController>();
      //  progressIcon.sprite = foundationScript.buildingPrefab.GetComponent<BuildingController>().icon;

        PeasantProgressBar = GameObject.Find("PeasantProgressBar");
        PeasantProgressSlider = PeasantProgressBar.GetComponent<Slider>();

        // UI functions
        //   MenuPanel = GameObject.Find("PauseMenu").GetComponent<CanvasGroup>();

       // HideGameMenuPanel();
        HideUnitPanel();
        HidePeasantPanel();
        HideBuildingPanel();
        HideAdvancedBuildingsPanel();
        HideBuildingProgressPanel();
        Debug.Log("here");
        rotation = Camera.main.transform.rotation;
        cam = Camera.main;
        minimap = GameObject.Find("Minimap").GetComponent<Camera>();
     }

    // Update is called once per frame
    void Update()
    {
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
                    if (hit1.transform.tag == "Selectable" || hit1.transform.tag == "Foundation" || hit1.transform.tag == "Ground" || hit1.transform.tag == "Yard" || hit1.transform.tag == "Barracks" || hit1.transform.tag == "House" || hit1.transform.tag == "Resource" || hit1.transform.tag == "Fort" || hit1.transform.tag == "Blacksmith" || hit1.transform.tag == "Lumber Yard" || hit1.transform.tag == "Stables")
                    {
                        LeftClick();
                    }
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.F10))
        {
            ShowGameMenuPanel();
        }
        
        if(selectedObj != null)
        {
            if (buildingScript.unitType == "Town Hall")
            {
                if (townHallScript != null && townHallScript.isTraining)
                {
                    PeasantProgressSlider.value = townHallScript.i * 10;
                }
            }
            else if (selectedObj.tag == "Foundation")
            {
                if (foundationScript != null && foundationScript.isBuilding)
                {
                    PeasantProgressSlider.value = foundationScript.buildPercent;
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
          //  Debug.Log(destination);
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

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 350, clickableLayer.value))
        {
            bool isAttackable = hit.collider.GetComponent(typeof(IAttackable)) != null;
                       
            if (hit.collider.gameObject.tag == "Doorway")
            {
                Cursor.SetCursor(doorway, new Vector2(0, 0), CursorMode.Auto);
            }
            else if (isAttackable)
            {
                Cursor.SetCursor(combat, new Vector2(0, 0), CursorMode.Auto);
            }
            else if (hit.collider.gameObject.tag == "Selectable")
            {
                Cursor.SetCursor(pointer, new Vector2(0, 0), CursorMode.Auto);
            }
            else if (hit.collider.gameObject.tag == "Resource")
            {
                Cursor.SetCursor(target, new Vector2(0, 0), CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(combat, new Vector2(0, 0), CursorMode.Auto);
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
        // If we press the left mouse button, save mouse location and begin selection
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            mousePosition1 = Input.mousePosition;
        }
        // If we let go of the left mouse button, end selection
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

            for (int i = 0; i < units.Length; i++)
            {
                unitPosX = units[i].transform.position.x;
                unitPosY = units[i].transform.position.y;
                unitPosZ = units[i].transform.position.z;

                unitPos = new Vector3(unitPosX, unitPosY, unitPosZ);
                Vector3 screenPos = cam.WorldToScreenPoint(unitPos);

                selectedInfo = units[i].GetComponent<Selection>();

                // Adds all units in selection box to selected
                if (selectRect.Contains(screenPos, true))
                {
                    isSelected = true;
                    selectedInfo.selected = true;
                    selectedInfo.transform.GetChild(2).gameObject.SetActive(true);

                    peasantAudio = selectedInfo.GetComponent<AudioSource>();
                    peasantAudio.clip = peasantAudioClip;
                    peasantAudio.Play();

                    HideBuildingPanel();
                    UpdateUnitPanel();
                    ShowUnitPanel();
                }
            }
        }
    }
    
    private void UpdateUnitPanel()
    {
        // UI Functions
        unitScript = selectedInfo.GetComponent<UnitController>();
        selectScript = selectedInfo.GetComponent<Selection>();

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
        armourDisp.text = unitScript.armour;
        NodeManager.ResourceTypes resourceType = selectScript.heldResourceType;
        float resourceHeld = selectScript.heldResource;
        itemDisp.text = resourceType + ": " + resourceHeld.ToString();
    }

    private void UpdateBuildingPanel()
    {
        // UI Functions
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
    
    private void ShowUnitPanel()
    {
        // UI Update
        UnitPanel.alpha = 1;
        UnitPanel.blocksRaycasts = true;
        UnitPanel.interactable = true;

        PeasantPanel.alpha = 1;
        PeasantPanel.blocksRaycasts = true;
        PeasantPanel.interactable = true;

    }

    private void HideGameMenuPanel()
    {
        // UI Update
        // UI Functions
     //   GameMenuPanel.alpha = 0;
    //    GameMenuPanel.blocksRaycasts = false;
     //   GameMenuPanel.interactable = false;
    }

    private void ShowGameMenuPanel()
    {
        // UI Update
        // UI Functions
     //   GameMenuPanel.alpha = 1;
      //  GameMenuPanel.blocksRaycasts = true;
    //    GameMenuPanel.interactable = true;
    }

    private void HideUnitPanel()
    {
        // UI Update
        // UI Functions
        UnitPanel.alpha = 0;
        UnitPanel.blocksRaycasts = false;
        UnitPanel.interactable = false;

        UnitActionPanel.alpha = 0;
        UnitActionPanel.blocksRaycasts = false;
        UnitActionPanel.interactable = false;
    }

    private void HidePeasantPanel()
    {
        // UI Update
        // UI Functions
        PeasantPanel.alpha = 0;
        PeasantPanel.blocksRaycasts = false;
        PeasantPanel.interactable = false;
    }

    private void ShowBuildingPanel()
    {
        // UI Update
        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;

    }

    private void ShowBuildingActionPanel()
    {
        BuildingActionPanel.alpha = 1;
        BuildingActionPanel.blocksRaycasts = true;
        BuildingActionPanel.interactable = true;
    }

    private void HideBuildingPanel()
    {
        // UI Update
        // UI Functions
        BuildingPanel.alpha = 0;
        BuildingPanel.blocksRaycasts = false;
        BuildingPanel.interactable = false;

        BuildingActionPanel.alpha = 0;
        BuildingActionPanel.blocksRaycasts = false;
        BuildingActionPanel.interactable = false;
    }

    private void HideAdvancedBuildingsPanel()
    {
        // UI Update
        // UI Functions
        AdvancedBuildingsPanel.alpha = 0;
        AdvancedBuildingsPanel.blocksRaycasts = false;
        AdvancedBuildingsPanel.interactable = false;
    }

    private void HideBuildingProgressPanel()
    {
        // UI Update
        // UI Functions
        BuildingProgressPanel.alpha = 0;
        BuildingProgressPanel.blocksRaycasts = false;
        BuildingProgressPanel.interactable = false;
    }

    // Left click controller
    public void LeftClick()
    {
        // Check for all selectable units in the game
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 350))
        {
            if (hit.collider.tag == "Ground" && (!Input.GetKey(KeyCode.LeftShift)))
            {
                if(selectedObjects.Length >= 0)
                {
                    DeselectUnits();
                    HideUnitPanel();
                    HideAdvancedBuildingsPanel();
                   // Debug.Log("Standing down, Sir!");
                }
                HideBuildingPanel();
                HideBuildingProgressPanel();
            }
            else if (hit.collider.tag == "Selectable")
            {
                selectedObj = hit.collider.gameObject;
                selectedInfo = selectedObj.GetComponent<Selection>();
                
                if (selectedInfo.selected == true)
                {
                    DeselectUnits();
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    selectedInfo.selected = true;

                    isSelected = true;
                    // Selection indicators
                    selectedObj.transform.GetChild(2).gameObject.SetActive(true);

                    peasantAudio = selectedObj.GetComponent<AudioSource>();
                    peasantAudio.clip = peasantAudioClip;
                    peasantAudio.Play();
                    HideBuildingPanel();
                    HideBuildingProgressPanel();
                    UpdateUnitPanel();
                    ShowUnitPanel();
                }
                else
                {
                    GameObject[] selectedIndicators = GameObject.FindGameObjectsWithTag("SelectedIndicator");
                    for (int j = 0; j < selectedIndicators.Length; j++)
                    {
                        // Turns the unit selection indicator off
                        selectedIndicators[j].transform.gameObject.SetActive(false);

                        // Deselects the unit
                        selectedIndicators[j].transform.parent.GetComponent<Selection>().selected = false;
                    }

                    selectedInfo.selected = true;

                    // Selection indicators
                    selectedObj.transform.GetChild(2).gameObject.SetActive(true);
                    peasantAudio = selectedObj.GetComponent<AudioSource>();
                    peasantAudio.clip = peasantAudioClip;
                    peasantAudio.Play();
                    HideBuildingPanel();
                    HideBuildingProgressPanel();
                    isSelected = true;

                    UpdateUnitPanel();
                    ShowUnitPanel();
                }
            }
            else if (hit.collider.tag == "Yard" || hit.collider.tag == "Foundation" || hit.collider.tag == "Barracks" || hit.collider.tag == "House" || hit.collider.tag == "Resource" || hit.collider.tag == "Fort" || hit.collider.tag == "Blacksmith" || hit.transform.tag == "Lumber Yard" || hit.transform.tag == "Stables" )
            {
                if (selectedObjects.Length >= 0)
                {
                    DeselectUnits();
                    HideUnitPanel();
                    HidePeasantPanel();
                    HideAdvancedBuildingsPanel();
                   // Debug.Log("Standing down, Sir!");
                }

                selectedObj = hit.collider.gameObject;

                // Handle town hall selection
                if(selectedObj.tag == "Yard")
                {
                    townHallScript = selectedObj.GetComponent<TownHallController>();
                    isTraining = townHallScript.isTraining;
                    if (isTraining)
                    {
                        SwapProgressIcon();
                        ShowBuildingProgressPanel();
                        HideBuildingActionPanel();
                    }
                    else
                    {
                        ShowBuildingActionPanel();
                        HideBuildingProgressPanel();
                    }
                }
                else if (selectedObj.tag == "Foundation")
                {
                    foundationScript = selectedObj.GetComponent<FoundationController>();
                    isBuilding = foundationScript.isBuilding;
                    if (isBuilding)
                    {
                        SwapProgressIcon();
                        ShowBuildingProgressPanel();
                        HideBuildingActionPanel();
                    }
                    else
                    {
                        ShowBuildingActionPanel();
                        HideBuildingProgressPanel();
                    }
                }

                HideUnitPanel();
                HidePeasantPanel();
                HideAdvancedBuildingsPanel();
                UpdateBuildingPanel();
                ShowBuildingPanel();
            }
            else if (hit.collider.tag != "Selectable" && (!Input.GetKey(KeyCode.LeftShift)))
            {
                DeselectUnits();
                HideBuildingPanel();
                HidePeasantPanel();
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

    void HideBuildingActionPanel()
    {
        BuildingActionPanel.alpha = 0;
        BuildingActionPanel.blocksRaycasts = false;
        BuildingActionPanel.interactable = false;
    }

    void ShowBuildingProgressPanel()
    {
        BuildingProgressPanel.alpha = 1;
        BuildingProgressPanel.blocksRaycasts = true;
        BuildingProgressPanel.interactable = true;
    }

    private void DeselectUnits()
    {
        selectedObj = null;
        HideUnitPanel();
        HidePeasantPanel();
        for (int i = 0; i < selectedObjects.Length; i++)
        {
            // Grabs all objects in selected array and deselects them
            selectedInfo = selectedObjects[i].GetComponent<Selection>();
            selectedInfo.selected = false;
        }
        GameObject[] selectedIndicators = GameObject.FindGameObjectsWithTag("SelectedIndicator");

        // Selection indicators
        for (int j = 0; j < selectedIndicators.Length; j++)
        {
            // Turns the unit selection indicator off
            selectedIndicators[j].transform.gameObject.SetActive(false);

            // Deselects the unit
            selectedIndicators[j].transform.parent.GetComponent<Selection>().selected = false;
        }

        isSelected = false;
    }

}

[System.Serializable]
public class EventVector3 : UnityEvent<Vector3> { }

[System.Serializable]
public class EventGameObject : UnityEvent<GameObject> { }


﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // PLAYER
    public GameObject player;
    public GameObject team;
    public GameObject saveMenu;
    public GameObject loadMenu;
    ResourceManager RM;
    InputManager IM;

    // GAME MENU
    public CanvasGroup GameMenuPanel;

    public int panelOpen = 0;
    // 0 - no panel
    // 1 - unit panel
    // 2 - building panel

    // UNIT PANELS
    public CanvasGroup UnitPanel;
    public CanvasGroup VillagerPanel;
    public CanvasGroup BasicBuildingsPanel;
    public CanvasGroup AdvancedBuildingsPanel;
    public CanvasGroup FootmanPanel;

    public CanvasGroup armour1;
    public CanvasGroup armour2;
    public CanvasGroup armour3;
    public CanvasGroup armour4;
    public CanvasGroup armour5;

    // BUILDING PANELS
    public CanvasGroup BuildingPanel;
    public CanvasGroup BuildingActionPanel;
    public CanvasGroup BuildingProgressPanel;
    public CanvasGroup BlacksmithActionPanel;
    public CanvasGroup LumberYardActionPanel;
    public CanvasGroup BarracksActionPanel;
    public CanvasGroup TrainingProgressPanel;

    BuildingButtonController BBC;

    // NOTIFICATION PANELS
    public CanvasGroup noResourcesText;
    public CanvasGroup rotationText;
    public CanvasGroup placementText;
    
    public CanvasGroup attackMovingText;
    public CanvasGroup movingText;
    public CanvasGroup patrolText;

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            FindAllPanels();
            CloseGameMenuPanel();
            CloseAllPanels();
        }
    }
    
    public void FindAllPanels() {
        GameMenuPanel = GameObject.Find("GameMenu").GetComponent<CanvasGroup>();

        saveMenu = GameObject.Find("SaveMenu");
        loadMenu = GameObject.Find("loadMenu");
        UnitPanel = GameObject.Find("UnitPanel").GetComponent<CanvasGroup>();
        VillagerPanel = GameObject.Find("VillagerPanel").GetComponent<CanvasGroup>();
        BasicBuildingsPanel = GameObject.Find("BasicBuildingsPanel").GetComponent<CanvasGroup>();
        AdvancedBuildingsPanel = GameObject.Find("AdvancedBuildingsPanel").GetComponent<CanvasGroup>();
        FootmanPanel = GameObject.Find("FootmanPanel").GetComponent<CanvasGroup>();
                
        BuildingPanel = GameObject.Find("BuildingPanel").GetComponent<CanvasGroup>();
        BlacksmithActionPanel = GameObject.Find("BlacksmithActionPanel").GetComponent<CanvasGroup>();
        LumberYardActionPanel = GameObject.Find("LumberYardActionPanel").GetComponent<CanvasGroup>();
        BarracksActionPanel = GameObject.Find("BarracksActionPanel").GetComponent<CanvasGroup>();

        TrainingProgressPanel = GameObject.Find("TrainingProgressPanel").GetComponent<CanvasGroup>();
        BuildingProgressPanel = GameObject.Find("BuildingProgressPanel").GetComponent<CanvasGroup>();
        BuildingActionPanel = GameObject.Find("BuildingActions").GetComponent<CanvasGroup>();

        armour1 = GameObject.Find("Armour1").GetComponent<CanvasGroup>();
        armour2 = GameObject.Find("Armour2").GetComponent<CanvasGroup>();
        armour3 = GameObject.Find("Armour3").GetComponent<CanvasGroup>();
        armour4 = GameObject.Find("Armour4").GetComponent<CanvasGroup>();
        armour5 = GameObject.Find("Armour5").GetComponent<CanvasGroup>();

        noResourcesText = GameObject.Find("No Resources Panel").GetComponent<CanvasGroup>();
        rotationText = GameObject.Find("Rotation Text").GetComponent<CanvasGroup>();
        placementText = GameObject.Find("Placement Text").GetComponent<CanvasGroup>();

        attackMovingText = GameObject.Find("Attack Move Text").GetComponent<CanvasGroup>();
        movingText = GameObject.Find("Normal Move Text").GetComponent<CanvasGroup>();
        patrolText = GameObject.Find("Patrol Move Text").GetComponent<CanvasGroup>();

        team = GameObject.Find("Faction");
        player = GameObject.Find("Game").GetComponent<SaveLoad>().loadedPlayer;
        RM = team.GetComponent<ResourceManager>();
        IM = player.GetComponent<InputManager>();
    }

    public void DisplaySelectedObjects(GameObject selectedUnit)
    {
        if(!IM.selectedObjects.Contains(selectedUnit)) {
            UnitController unitScript = selectedUnit.GetComponent<UnitController>();
            GameObject[] allSelectedUnitIcons = GameObject.FindGameObjectsWithTag("SelectedUnitIcon");

            bool unitTypePresentInArray = false;
            GameObject selectedIcon = null;
            for(int i = 0; i<allSelectedUnitIcons.Length; i++) {
                if(allSelectedUnitIcons[i].name == unitScript.unitType) {
                    unitTypePresentInArray = true;
                    selectedIcon = allSelectedUnitIcons[i];
                }
            }

            int unitCount = 1;
            for(int i = 0; i<IM.selectedObjects.Count; i++) {
                if(IM.selectedObjects[i].GetComponent<UnitController>().unitType == unitScript.unitType) {
                    unitCount += 1;
                }
            }

            if(unitTypePresentInArray) {
                selectedIcon.GetComponentInChildren<Text>().text = unitCount + "";
            } else {
                GameObject newUnit = new GameObject();
                GameObject text = new GameObject();
                newUnit.name = unitScript.unitType;
                newUnit.tag = "SelectedUnitIcon";
                newUnit.AddComponent<Image>();
                newUnit.GetComponent<Image>().sprite = unitScript.unitIcon;
                newUnit.AddComponent<Outline>();         
                newUnit.GetComponent<Outline>().effectColor = new Color(0, 0, 255);
                newUnit.GetComponent<Outline>().effectDistance = new Vector2(5, 5);

                newUnit.transform.SetParent(UnitPanel.transform); 
                newUnit.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                text.AddComponent<Text>();
                text.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                text.GetComponent<Text>().text = "1";
                text.transform.SetParent(newUnit.transform);
                text.transform.Translate(50, 0, Time.deltaTime);
                if(unitScript.unitType == "Worker") {
                    newUnit.transform.position = new Vector3(620, 270, 50);
                } else if (unitScript.unitType == "Swordsman") {
                    newUnit.transform.position = new Vector3(670, 270, 50);
                } else if (unitScript.unitType == "Archer") {
                    newUnit.transform.position = new Vector3(720, 270, 50);
                } else if (unitScript.unitType == "Footman") {
                    newUnit.transform.position = new Vector3(770, 270, 50);
                } else if (unitScript.unitType == "Outrider") {
                    newUnit.transform.position = new Vector3(820, 270, 50);
                } else if (unitScript.unitType == "Knight") {
                    newUnit.transform.position = new Vector3(870, 270, 50);
                } else if (unitScript.unitType == "Wizard") {
                    newUnit.transform.position = new Vector3(920, 270, 50);
                }
            }
        }
    }

    public void RemoveSelectedObjects(GameObject selectedUnit) {
        GameObject selectedIcon = null;
        GameObject[] allSelectedUnitIcons = GameObject.FindGameObjectsWithTag("SelectedUnitIcon");
        string unitType = selectedUnit.GetComponent<UnitController>().unitType;
        int unitCount = 0;
        for(int i = 0; i<IM.selectedObjects.Count; i++) {
            if(IM.selectedObjects[i].GetComponent<UnitController>().unitType == selectedUnit.GetComponent<UnitController>().unitType) {
                unitCount += 1;
            }
        }

        for(int i=allSelectedUnitIcons.Length - 1; i>0; i--) {
            if(unitType == allSelectedUnitIcons[i].name) {
                selectedIcon = allSelectedUnitIcons[i];
                selectedIcon.GetComponentInChildren<Text>().text = (unitCount-1) + "";
                if(unitCount-1 == 0) {
                    Destroy(selectedIcon);
                }
                break;
            }
        }
    }

    public void RemoveAllSelectedObjects(GameObject selectedUnit) {
        GameObject[] allSelectedUnitIcons = GameObject.FindGameObjectsWithTag("SelectedUnitIcon");
        string unitType = selectedUnit.GetComponent<UnitController>().unitType;
        for(int i=allSelectedUnitIcons.Length - 1; i>0; i--) {
            if(unitType == allSelectedUnitIcons[i].name) {
                Destroy(allSelectedUnitIcons[i]);
                break;
            }
        }
    }

    public void ResetSelectionIcons() {
        GameObject[] allSelectedUnitIcons = GameObject.FindGameObjectsWithTag("SelectedUnitIcon");
        foreach(GameObject go in allSelectedUnitIcons)
        {
            Destroy(go);
        }
    }

    public void OpenGameMenuPanel()
    {
        GameMenuPanel.alpha = 1;
        GameMenuPanel.blocksRaycasts = true;
        GameMenuPanel.interactable = true;
    }

    public void CloseGameMenuPanel()
    {
        GameMenuPanel.alpha = 0;
        GameMenuPanel.blocksRaycasts = false;
        GameMenuPanel.interactable = false;
    }

    public void CloseAllPanels() {
        // UNITS
        UnitPanel.alpha = 0;
        UnitPanel.blocksRaycasts = false;
        UnitPanel.interactable = false;
        VillagerPanel.alpha = 0;
        VillagerPanel.blocksRaycasts = false;
        VillagerPanel.interactable = false;

        // VILLAGER PANELS
        BasicBuildingsPanel.alpha = 0;
        BasicBuildingsPanel.blocksRaycasts = false;
        BasicBuildingsPanel.interactable = false;
        AdvancedBuildingsPanel.alpha = 0;
        AdvancedBuildingsPanel.blocksRaycasts = false;
        AdvancedBuildingsPanel.interactable = false;

        // FOOTMAN PANELS
        FootmanPanel.alpha = 0;
        FootmanPanel.blocksRaycasts = false;
        FootmanPanel.interactable = false;

        // BUILDINGS
        BuildingPanel.alpha = 0;
        BuildingPanel.blocksRaycasts = false;
        BuildingPanel.interactable = false;
        BuildingActionPanel.alpha = 0;
        BuildingActionPanel.blocksRaycasts = false;
        BuildingActionPanel.interactable = false;
        BuildingProgressPanel.alpha = 0;
        BuildingProgressPanel.blocksRaycasts = false;
        BuildingProgressPanel.interactable = false;

        // BLACKSMITH
        BlacksmithActionPanel.alpha = 0;
        BlacksmithActionPanel.blocksRaycasts = false;
        BlacksmithActionPanel.interactable = false;

        // LUMBER YARD
        LumberYardActionPanel.alpha = 0;
        LumberYardActionPanel.blocksRaycasts = false;
        LumberYardActionPanel.interactable = false;

        // BARRACKS
        BarracksActionPanel.alpha = 0;
        BarracksActionPanel.blocksRaycasts = false;
        BarracksActionPanel.interactable = false;

        // TRAINING
        TrainingProgressPanel.alpha = 0;
        TrainingProgressPanel.blocksRaycasts = false;
        TrainingProgressPanel.interactable = false;

        // NOTIFICATIONS
        CloseNoResourcesText();
        NoModeText();

        panelOpen = 0;        
    }

    // DIFFERENT STATES

    // NOTIFICATIONS TEXT
    public void CloseNoResourcesText() {
        noResourcesText.alpha = 0;
        noResourcesText.blocksRaycasts = false;
        noResourcesText.interactable = false;
    }

    public void OpenNoResourcesText(string text) {
        noResourcesText.alpha = 1;
        noResourcesText.blocksRaycasts = true;
        noResourcesText.interactable = true;
        noResourcesText.GetComponentInChildren<Text>().text = text;
    }

    public void RotationModeText() {
        rotationText.alpha = 1;
        rotationText.blocksRaycasts = true;
        rotationText.interactable = true;
    }

    public void PlacementModeText() {
        placementText.alpha = 1;
        placementText.blocksRaycasts = true;
        placementText.interactable = true;
    }

    public void AttackMovementText() {
        attackMovingText.alpha = 1;
        attackMovingText.blocksRaycasts = true;
        attackMovingText.interactable = true;
    }

    public void StandardMovementText() {
        movingText.alpha = 1;
        movingText.blocksRaycasts = true;
        movingText.interactable = true;
    }

    public void PatrolMovementText() {
        patrolText.alpha = 1;
        patrolText.blocksRaycasts = true;
        patrolText.interactable = true;
    }

    public void NoModeText() {
        rotationText.alpha = 0;
        rotationText.blocksRaycasts = false;
        rotationText.interactable = false;

        placementText.alpha = 0;
        placementText.blocksRaycasts = false;
        placementText.interactable = false;

        attackMovingText.alpha = 0;
        attackMovingText.blocksRaycasts = false;
        attackMovingText.interactable = false;

        movingText.alpha = 0;
        movingText.blocksRaycasts = false;
        movingText.interactable = false;

        patrolText.alpha = 0;
        patrolText.blocksRaycasts = false;
        patrolText.interactable = false;
    }


    // On worker selection
    public void WorkerSelect() {
        CloseAllPanels();

        UnitPanel.alpha = 1;
        UnitPanel.blocksRaycasts = true;
        UnitPanel.interactable = true;

        panelOpen = 1;        
        VillagerPanel.alpha = 1;
        VillagerPanel.blocksRaycasts = true;
        VillagerPanel.interactable = true;
    }

    // On villager clicking basic buildings
    public void VillagerBasicBuildings() {
        CloseAllPanels();

        UnitPanel.alpha = 1;
        UnitPanel.blocksRaycasts = true;
        UnitPanel.interactable = true;

        panelOpen = 1;        
        BasicBuildingsPanel.alpha = 1;
        BasicBuildingsPanel.blocksRaycasts = true;
        BasicBuildingsPanel.interactable = true;
    }

    // On villager clicking advanced buildings
    public void VillagerAdvancedBuildings() {
        CloseAllPanels();

        UnitPanel.alpha = 1;
        UnitPanel.blocksRaycasts = true;
        UnitPanel.interactable = true;

        panelOpen = 1;        
        AdvancedBuildingsPanel.alpha = 1;
        AdvancedBuildingsPanel.blocksRaycasts = true;
        AdvancedBuildingsPanel.interactable = true;
    }

    public void SwordsmanSelect() {
        CloseAllPanels();

        UnitPanel.alpha = 1;
        UnitPanel.blocksRaycasts = true;
        UnitPanel.interactable = true;

        panelOpen = 1;        
        FootmanPanel.alpha = 1;
        FootmanPanel.blocksRaycasts = true;
        FootmanPanel.interactable = true;
    }

    public void FootmanSelect() {
        CloseAllPanels();

        UnitPanel.alpha = 1;
        UnitPanel.blocksRaycasts = true;
        UnitPanel.interactable = true;

        panelOpen = 1;        
        FootmanPanel.alpha = 1;
        FootmanPanel.blocksRaycasts = true;
        FootmanPanel.interactable = true;
    }
    
    public void ArcherSelect() {
        CloseAllPanels();

        UnitPanel.alpha = 1;
        UnitPanel.blocksRaycasts = true;
        UnitPanel.interactable = true;

        panelOpen = 1;        
        FootmanPanel.alpha = 1;
        FootmanPanel.blocksRaycasts = true;
        FootmanPanel.interactable = true;
    }
    
    public void OutriderSelect() {
        CloseAllPanels();

        UnitPanel.alpha = 1;
        UnitPanel.blocksRaycasts = true;
        UnitPanel.interactable = true;

        panelOpen = 1;        
        FootmanPanel.alpha = 1;
        FootmanPanel.blocksRaycasts = true;
        FootmanPanel.interactable = true;
    }

    public void KnightSelect() {
        CloseAllPanels();

        UnitPanel.alpha = 1;
        UnitPanel.blocksRaycasts = true;
        UnitPanel.interactable = true;

        panelOpen = 1;        
        FootmanPanel.alpha = 1;
        FootmanPanel.blocksRaycasts = true;
        FootmanPanel.interactable = true;
    }

    public void WizardSelect() {
        CloseAllPanels();

        UnitPanel.alpha = 1;
        UnitPanel.blocksRaycasts = true;
        UnitPanel.interactable = true;

        panelOpen = 1;        
        FootmanPanel.alpha = 1;
        FootmanPanel.blocksRaycasts = true;
        FootmanPanel.interactable = true;
    }

    // Enemy selection
    public void EnemySelect() {
        CloseAllPanels();

        UnitPanel.alpha = 1;
        UnitPanel.blocksRaycasts = true;
        UnitPanel.interactable = true;

        panelOpen = 1;        
    }

    // On house selection
    public void HouseSelect() {
        CloseAllPanels();

        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;

        panelOpen = 2;
    }

    // On town hall selection
    public void TownHallSelect() {
        CloseAllPanels();

        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;

        BuildingActionPanel.alpha = 1;
        BuildingActionPanel.blocksRaycasts = true;
        BuildingActionPanel.interactable = true;
        CloseTrainingProgressPanel();
        panelOpen = 2;
    }

    // On town hall selection if it is training
    public void TownHallTraining() {
        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;

        BuildingActionPanel.alpha = 1;
        BuildingActionPanel.blocksRaycasts = true;
        BuildingActionPanel.interactable = true;

        OpenTrainingProgressPanel();
        panelOpen = 2;
    }

    // On town hall selection
    public void BlacksmithSelect() {
        CloseAllPanels();

        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;

        BlacksmithActionPanel.alpha = 1;
        BlacksmithActionPanel.blocksRaycasts = true;
        BlacksmithActionPanel.interactable = true;

        panelOpen = 2;
    }

    public void BlacksmithTraining() {
        CloseAllPanels();

        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;

        OpenBuildingProgressPanel();
        panelOpen = 2;
    }

    // On town hall selection
    public void LumberYardSelect() {
        CloseAllPanels();

        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;

        LumberYardActionPanel.alpha = 1;
        LumberYardActionPanel.blocksRaycasts = true;
        LumberYardActionPanel.interactable = true;
        panelOpen = 2;
    }

    // On town hall selection
    public void BarracksSelect() {
        CloseAllPanels();

        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;

        BarracksActionPanel.alpha = 1;
        BarracksActionPanel.blocksRaycasts = true;
        BarracksActionPanel.interactable = true;
        CloseTrainingProgressPanel();
        panelOpen = 2;
    }

    public void BarracksTraining() {
        CloseAllPanels();

        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;

        BarracksActionPanel.alpha = 1;
        BarracksActionPanel.blocksRaycasts = true;
        BarracksActionPanel.interactable = true;
        OpenTrainingProgressPanel();
        panelOpen = 2;
    }

    // On town hall selection
    public void StablesSelect() {
        CloseAllPanels();

        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;

        panelOpen = 2;
    }

    // On resource node selection (including farms)
    public void ResourceSelect() {
        CloseAllPanels();

        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;

        panelOpen = 2;
    }

   // On resource node selection (including farms)
    public void FoundationSelect() {
        CloseAllPanels();

        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;

        panelOpen = 2;
    }
    
   // On resource node selection (including farms)
    public void FoundationBuilding() {
        CloseAllPanels();

        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;
        
        OpenBuildingProgressPanel();
        panelOpen = 2;
    }

    public void OpenBuildingProgressPanel(){
        BuildingProgressPanel.alpha = 1;
        BuildingProgressPanel.blocksRaycasts = true;
        BuildingProgressPanel.interactable = true;
    }

    public void OpenTrainingProgressPanel(){
        TrainingProgressPanel.alpha = 1;
        TrainingProgressPanel.blocksRaycasts = true;
        TrainingProgressPanel.interactable = true;
    }

    public void CloseTrainingProgressPanel(){
        TrainingProgressPanel.alpha = 0;
        TrainingProgressPanel.blocksRaycasts = false;
        TrainingProgressPanel.interactable = false;
    }
}


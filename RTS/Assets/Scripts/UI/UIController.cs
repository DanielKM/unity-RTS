using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // PLAYER
    public GameObject player;
    public GameObject team;
    public GameObject saveMenu;
    public GameObject loadMenu;
    ResourceManager RM;

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

    public GameObject noResourcesText;
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

        BuildingProgressPanel = GameObject.Find("BuildingProgressPanel").GetComponent<CanvasGroup>();
        BuildingActionPanel = GameObject.Find("BuildingActions").GetComponent<CanvasGroup>();

        armour1 = GameObject.Find("Armour1").GetComponent<CanvasGroup>();
        armour2 = GameObject.Find("Armour2").GetComponent<CanvasGroup>();
        armour3 = GameObject.Find("Armour3").GetComponent<CanvasGroup>();
        armour4 = GameObject.Find("Armour4").GetComponent<CanvasGroup>();
        armour5 = GameObject.Find("Armour5").GetComponent<CanvasGroup>();
        team = GameObject.Find("Faction");
        player = GameObject.FindGameObjectWithTag("Player");
        RM = team.GetComponent<ResourceManager>();
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

        panelOpen = 0;        
    }

    // DIFFERENT STATES
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
        panelOpen = 2;
    }

    // On town hall selection if it is training
    public void TownHallTraining() {
        CloseAllPanels();

        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;

        BuildingProgressPanel.alpha = 1;
        BuildingProgressPanel.blocksRaycasts = true;
        BuildingProgressPanel.interactable = true;
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

        BuildingProgressPanel.alpha = 1;
        BuildingProgressPanel.blocksRaycasts = true;
        BuildingProgressPanel.interactable = true;

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

        panelOpen = 2;
    }

    public void BarracksTraining() {
        CloseAllPanels();

        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;

        BuildingProgressPanel.alpha = 1;
        BuildingProgressPanel.blocksRaycasts = true;
        BuildingProgressPanel.interactable = true;
        
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

        BuildingProgressPanel.alpha = 1;
        BuildingProgressPanel.blocksRaycasts = true;
        BuildingProgressPanel.interactable = true;
        panelOpen = 2;
    }
}

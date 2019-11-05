using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // PLAYER
    public GameObject player;
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

    // BUILDING PANELS
    public CanvasGroup BuildingPanel;
    public CanvasGroup BuildingActionPanel;
    public CanvasGroup BuildingProgressPanel;
    public CanvasGroup BlacksmithActionPanel;
    public CanvasGroup BlacksmithProgressPanel;

    public GameObject noResourcesText;
    // Start is called before the first frame update
    void Start()
    {
        GameMenuPanel = GameObject.Find("GameMenu").GetComponent<CanvasGroup>();

        UnitPanel = GameObject.Find("UnitPanel").GetComponent<CanvasGroup>();
        VillagerPanel = GameObject.Find("VillagerPanel").GetComponent<CanvasGroup>();
        BasicBuildingsPanel = GameObject.Find("BasicBuildingsPanel").GetComponent<CanvasGroup>();
        AdvancedBuildingsPanel = GameObject.Find("AdvancedBuildingsPanel").GetComponent<CanvasGroup>();
                
        BuildingPanel = GameObject.Find("BuildingPanel").GetComponent<CanvasGroup>();
        BlacksmithActionPanel = GameObject.Find("BlacksmithActionPanel").GetComponent<CanvasGroup>();
        BlacksmithProgressPanel = GameObject.Find("BlacksmithProgressPanel").GetComponent<CanvasGroup>();
        BuildingProgressPanel = GameObject.Find("BuildingProgressPanel").GetComponent<CanvasGroup>();
        BuildingActionPanel = GameObject.Find("BuildingActions").GetComponent<CanvasGroup>();

        player = GameObject.FindGameObjectWithTag("Player");
        RM = player.GetComponent<ResourceManager>();

        CloseGameMenuPanel();
        CloseAllPanels();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void CloseAllPanels() {
        CloseUnitPanel();
        CloseVillagerPanel();
        CloseBasicBuildingsPanel();
        CloseAdvancedBuildingsPanel();

        CloseBuildingPanel();
        CloseBuildingActionPanel();
        CloseBuildingProgressPanel();
        CloseBlacksmithActionPanel();
        CloseBlacksmithProgressPanel();
    }

    public void CloseBuildingPanels() {
        CloseBuildingPanel();
        CloseBuildingActionPanel();
        CloseBuildingProgressPanel();
        CloseBlacksmithActionPanel();
        CloseBlacksmithProgressPanel();
    }

    public void CloseUnitPanels() {
        CloseUnitPanel();
        CloseVillagerPanel();
        CloseBasicBuildingsPanel();
        CloseAdvancedBuildingsPanel();
    }

    public void OpenVillagerPanels() {
        CloseAllPanels();

        OpenUnitPanel();
        OpenVillagerPanel();
    }

    public void OpenBuildingPanels() {
        CloseAllPanels();

        OpenBuildingPanel();
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

    // UNIT PANEL
    public void OpenUnitPanel()
    {
        UnitPanel.alpha = 1;
        UnitPanel.blocksRaycasts = true;
        UnitPanel.interactable = true;

        panelOpen = 1;
    }

    public void CloseUnitPanel()
    {
        UnitPanel.alpha = 0;
        UnitPanel.blocksRaycasts = false;
        UnitPanel.interactable = false;
    }
    
    // VILLAGER PANELS
    public void OpenVillagerPanel()
    {
        VillagerPanel.alpha = 1;
        VillagerPanel.blocksRaycasts = true;
        VillagerPanel.interactable = true;
    }

    public void CloseVillagerPanel()
    {
        VillagerPanel.alpha = 0;
        VillagerPanel.blocksRaycasts = false;
        VillagerPanel.interactable = false;
    }

    // BASIC BUILDING PANELS
    public void OpenBuildingPanel()
    {
        BuildingPanel.alpha = 1;
        BuildingPanel.blocksRaycasts = true;
        BuildingPanel.interactable = true;

        panelOpen = 2;
    }

    public void CloseBuildingPanel()
    {
        BuildingPanel.alpha = 0;
        BuildingPanel.blocksRaycasts = false;
        BuildingPanel.interactable = false;
    }
    
    public void OpenBlacksmithActionPanel() {
        BlacksmithActionPanel.alpha = 1;
        BlacksmithActionPanel.blocksRaycasts = true;
        BlacksmithActionPanel.interactable = true;
    }

    public void CloseBlacksmithActionPanel() {
        BlacksmithActionPanel.alpha = 0;
        BlacksmithActionPanel.blocksRaycasts = false;
        BlacksmithActionPanel.interactable = false;
    }

    public void OpenBlacksmithProgressPanel() {
        BlacksmithProgressPanel.alpha = 1;
        BlacksmithProgressPanel.blocksRaycasts = true;
        BlacksmithProgressPanel.interactable = true;
    }

    public void CloseBlacksmithProgressPanel() {
        BlacksmithProgressPanel.alpha = 0;
        BlacksmithProgressPanel.blocksRaycasts = false;
        BlacksmithProgressPanel.interactable = false;
    }

    public void OpenBuildingActionPanel()
    {
        BuildingActionPanel.alpha = 1;
        BuildingActionPanel.blocksRaycasts = true;
        BuildingActionPanel.interactable = true;
    }

    public void CloseBuildingActionPanel()
    {
        BuildingActionPanel.alpha = 0;
        BuildingActionPanel.blocksRaycasts = false;
        BuildingActionPanel.interactable = false;
    }

    public void OpenBuildingProgressPanel()
    {
        BuildingProgressPanel.alpha = 1;
        BuildingProgressPanel.blocksRaycasts = true;
        BuildingProgressPanel.interactable = true;
    }

    public void CloseBuildingProgressPanel()
    {
        BuildingProgressPanel.alpha = 0;
        BuildingProgressPanel.blocksRaycasts = false;
        BuildingProgressPanel.interactable = false;
    }

    // BASIC BUILDINGS PANEL
    public void OpenBasicBuildingsPanel()
    {
        BasicBuildingsPanel.alpha = 1;
        BasicBuildingsPanel.blocksRaycasts = true;
        BasicBuildingsPanel.interactable = true;
    }
    
    public void CloseBasicBuildingsPanel()
    {
        BasicBuildingsPanel.alpha = 0;
        BasicBuildingsPanel.blocksRaycasts = false;
        BasicBuildingsPanel.interactable = false;
    }

    // ADVANCED BUILDINGS PANELS
    public void OpenAdvancedBuildingsPanel()
    {
        AdvancedBuildingsPanel.alpha = 1;
        AdvancedBuildingsPanel.blocksRaycasts = true;
        AdvancedBuildingsPanel.interactable = true;
    }

    public void CloseAdvancedBuildingsPanel()
    {
        AdvancedBuildingsPanel.alpha = 0;
        AdvancedBuildingsPanel.blocksRaycasts = false;
        AdvancedBuildingsPanel.interactable = false;
    }

    // UPDATING FUNCTIONS
    public void SwapProgressIcon()
    {
        // progressIcon = GameObject.Find("ProgressIcon").GetComponent<Image>();

        // UI Functions
        // buildingScript = selectedObj.GetComponent<BuildingController>();
        // if (buildingScript.unitType == "Town Hall")
        // {
        //     progressIcon.sprite = townHallScript.villagerPrefab.GetComponent<UnitController>().unitIcon;

        // } else if (buildingScript.tag == "Foundation")
        // {
        //     foundationScript = selectedObj.GetComponent<FoundationController>();
        //     progressIcon.sprite = foundationScript.buildingPrefab.GetComponent<BuildingController>().icon;
        // }
        // else
        // {
        //     progressIcon.sprite = buildingScript.icon;
        // }
    }

    public void UpdateUnitPanel()
    {
        // UI Functions
        // unitScript = selectedInfo.GetComponent<UnitController>();
        // selectScript = selectedInfo.GetComponent<Selection>();

        // HB.maxValue = unitScript.maxHealth;
        // HB.value = unitScript.health;

        // EB.maxValue = unitScript.maxEnergy;
        // EB.value = unitScript.energy;

        // healthDisp.text = "HEALTH: " + unitScript.health;
        // energyDisp.text = "ENERGY: " + unitScript.energy;

        // nameDisp.text = unitScript.unitName;
        // unitName.text = unitScript.unitType;
        // rankDisp.text = unitScript.unitRank;
        // killDisp.text = "Kills: " + unitScript.unitKills;

        // weaponDisp.text = unitScript.weapon;
        // armourDisp.text = unitScript.armour;
        // NodeManager.ResourceTypes resourceType = selectScript.heldResourceType;
        // float resourceHeld = selectScript.heldResource;
        // itemDisp.text = resourceType + ": " + resourceHeld.ToString();
    }

    public void UpdateBuildingPanel()
    {
        // UI Functions
        // buildingScript = selectedObj.GetComponent<BuildingController>();
        // Image icon = buildingIcon.GetComponent<Image>();
        // icon.sprite = buildingScript.icon;

        // buildingHB.maxValue = buildingScript.maxHealth;
        // buildingHB.value = buildingScript.health;

        // buildingEB.maxValue = buildingScript.maxEnergy;
        // buildingEB.value = buildingScript.energy;

        // buildingHealthDisp.text = "HEALTH: " + buildingScript.health;
        // buildingEnergyDisp.text = "ENERGY: " + buildingScript.energy;

        // buildingNameDisp.text = buildingScript.unitName;
        // buildingName.text = buildingScript.unitType;
        // buildingRankDisp.text = buildingScript.unitRank;
        // buildingKillDisp.text = "Kills: " + buildingScript.unitKills;

        // buildingWeaponDisp.text = buildingScript.weapon;
        // buildingArmourDisp.text = buildingScript.armour;

        // buildingItemDisp.text = "None";
        // if (selectedObj.transform.tag == "Resource")
        // {
        //     nodeScript = selectedObj.GetComponent<NodeManager>();
        //     buildingItemDisp.text = nodeScript.resourceType + ": " + nodeScript.availableResource;
        // }
    }
}

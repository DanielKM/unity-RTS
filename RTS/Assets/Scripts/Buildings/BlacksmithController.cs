using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class BlacksmithController : MonoBehaviour
{
    UIController UI;
    private float nextSpawnTime;
    public int i = 0;

    private AudioSource swordsmanAudio;
    private AudioSource footmanAudio;
    public AudioClip swordsmanReporting;
    public AudioClip footmanReporting;

    private NavMeshAgent agent;

    [SerializeField]
    public float spawnDelay;
    public bool selected = false;

    GameObject player;
    GameObject team;
    InputManager inputScript;
    UnitSelection swordsmanUnitSelection;
    UnitSelection footmanUnitSelection;
    BuildingController buildingScript;
    ResourceManager RM;
    ResearchController RC;

    public GameObject selectedObj;
    private Vector3 spawnPosition;
    public bool isTraining;
    public string research;

    //Progress bar
    private GameObject BuildingProgressBar;
    private Slider BuildingProgressSlider;
    public Image progressIcon;

    //UI Elements
    private CanvasGroup BuildingProgressPanel;
    private CanvasGroup BuildingActionPanel;

    private UnitSelection selectscript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        team = GameObject.Find("Faction");
        UI = player.GetComponent<UIController>();
        RM = team.GetComponent<ResourceManager>();
        RC = team.GetComponent<ResearchController>();
        inputScript = player.GetComponent<InputManager>();
        BuildingProgressPanel = GameObject.Find("BuildingProgressPanel").GetComponent<CanvasGroup>();
        BuildingActionPanel = GameObject.Find("BuildingActions").GetComponent<CanvasGroup>();

        // Progress bar
        BuildingProgressBar = GameObject.Find("BuildingProgressBar");
        BuildingProgressSlider = BuildingProgressBar.GetComponent<Slider>();
    }

    public void StartResearch (string selectedResearch) {
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        float goldCost = 0;
        float ironCost = 0;

        switch(selectedResearch) {
            case "basicBlacksmithing":
                goldCost = RC.basicBlacksmithingGold;
                ironCost = RC.basicBlacksmithingIron;
                break;
            case "basicToolSmithing":
                goldCost = RC.basicToolSmithingGold;
                ironCost = RC.basicToolSmithingIron;
                break;
            case "basicArmourSmithing":
                goldCost = RC.basicArmourSmithingGold;
                ironCost = RC.basicArmourSmithingIron;
                break;
            case "basicWeaponSmithing":
                goldCost = RC.basicWeaponSmithingGold;
                ironCost = RC.basicWeaponSmithingIron;
                break;
            case "steelSmithing":
                goldCost = RC.steelSmithingGold;
                ironCost = RC.steelSmithingIron;
                break;
            case "artisanToolSmithing":
                goldCost = RC.artisanToolSmithingGold;
                ironCost = RC.artisanToolSmithingIron;
                break;
            case "artisanArmourSmithing":
                goldCost = RC.artisanArmourSmithingGold;
                ironCost = RC.artisanArmourSmithingIron;
                break;
            case "artisanWeaponSmithing":
                goldCost = RC.artisanWeaponSmithingGold;
                ironCost = RC.artisanWeaponSmithingIron;
                break;
            case "horseshoes":
                goldCost = RC.horseshoesGold;
                ironCost = RC.horseshoesIron;
                break;
            case "minecarts":
                goldCost = RC.minecartsGold;
                ironCost = RC.minecartsIron;
                break;
            case "caltrops":
                goldCost = RC.caltropsGold;
                ironCost = RC.caltropsIron;
                break;
            case "reinforcedBuildings":
                goldCost = RC.reinforcedBuildingsGold;
                ironCost = RC.reinforcedBuildingsIron;
                break;
            default:
                return;
        }

        if(RM.gold < goldCost){
            UI.OpenNoResourcesText("Not enough gold!");
            StartCoroutine(CloseResourcesText());
        } else if (RM.iron < ironCost) {
            UI.OpenNoResourcesText("Not enough iron!");
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= goldCost;
            RM.iron -= ironCost;
            StartCoroutine(Research(selectedResearch));
        }
    }

    IEnumerator CloseResourcesText()
    {
        yield return new WaitForSeconds(3);
        UI.CloseNoResourcesText();
    }

    IEnumerator Research(string selectedResearch) 
    {
        UI.BlacksmithTraining();
        isTraining = true;
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();

        for (i = 1; i < 11; i++)
        {
            yield return new WaitForSeconds(1);
        }

        if(selectedResearch == "basicBlacksmithing") {
            RC.basicToolSmithingButton.interactable = true;
            RC.basicArmourSmithingButton.interactable = true;
            RC.basicWeaponSmithingButton.interactable = true;
            RC.steelSmithingButton.interactable = true;
            RC.basicBlacksmithing = true;
            var colors = RC.basicBlacksmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.basicBlacksmithingButton.colors = colors; 
            RC.basicBlacksmithingButton.interactable = false;

        } else if (selectedResearch == "basicToolSmithing") {
            RC.basicToolSmithing = true;
            if(RC.steelSmithing) {
                RC.artisanToolSmithingButton.interactable = true;
            }
            var colors = RC.basicToolSmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.basicToolSmithingButton.colors = colors;             
            RC.basicToolSmithingButton.interactable = false;

        } else if(selectedResearch == "basicArmourSmithing") {
            RC.basicArmourSmithing = true;
            if(RC.steelSmithing) {
                RC.artisanArmourSmithingButton.interactable = true;
            }
            var colors = RC.basicArmourSmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.basicArmourSmithingButton.colors = colors;             
            RC.basicArmourSmithingButton.interactable = false;
        } else if (selectedResearch == "basicWeaponSmithing") {
            RC.basicWeaponSmithing = true;
            if(RC.steelSmithing) {
                RC.artisanWeaponSmithingButton.interactable = true;
            }
            var colors = RC.basicWeaponSmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.basicWeaponSmithingButton.colors = colors;   
            RC.basicWeaponSmithingButton.interactable = false;
        } else if (selectedResearch == "steelSmithing") {
            RC.steelSmithing = true;
            if(RC.basicToolSmithing) {
                RC.artisanToolSmithingButton.interactable = true;
            } 
            if(RC.basicArmourSmithing) {
                RC.artisanArmourSmithingButton.interactable = true;
            } 
            if(RC.basicWeaponSmithing) {
                RC.artisanWeaponSmithingButton.interactable = true;
            } 
            var colors = RC.steelSmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.steelSmithingButton.colors = colors; 
            RC.steelSmithingButton.interactable = false;
        } else if (selectedResearch == "artisanToolSmithing") {
            RC.artisanToolSmithing = true;
            var colors = RC.artisanToolSmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.artisanToolSmithingButton.colors = colors; 
            RC.artisanToolSmithingButton.interactable = false;
        } else if (selectedResearch == "artisanArmourSmithing") {
            RC.artisanArmourSmithing = true;
            var colors = RC.artisanArmourSmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.artisanArmourSmithingButton.colors = colors; 
            RC.artisanArmourSmithingButton.interactable = false;
        } else if (selectedResearch == "artisanWeaponSmithing") {
            RC.artisanWeaponSmithing = true;
            var colors = RC.artisanWeaponSmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.artisanWeaponSmithingButton.colors = colors; 
            RC.artisanWeaponSmithingButton.interactable = false;
        } else if (selectedResearch == "horseshoes") {
            RC.horseshoes = true;
            var colors = RC.horseshoesButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.horseshoesButton.colors = colors; 
            RC.horseshoesButton.interactable = false;
        } else if (selectedResearch == "minecarts") {
            RC.minecarts = true;
            var colors = RC.minecartsButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.minecartsButton.colors = colors; 
            RC.minecartsButton.interactable = false;
        } else if (selectedResearch == "caltrops") {
            RC.caltrops = true;
            var colors = RC.caltropsButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.caltropsButton.colors = colors; 
            RC.caltropsButton.interactable = false;
        } else if (selectedResearch == "reinforcedBuildings") {
            RC.reinforcedBuildings = true;
            var colors = RC.reinforcedBuildingsButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.reinforcedBuildingsButton.colors = colors; 
            RC.reinforcedBuildingsButton.interactable = false;
        }
            
        isTraining = false;
    }

    public void OnCollisionEnter(Collision col) {
        selectscript = col.gameObject.GetComponent<UnitSelection>();
        if (col.collider.tag == "Selectable" && selectscript.task == ActionList.Gathering)
        {
            if (selectscript.heldResource > 0 && selectscript.heldResourceType == NodeManager.ResourceTypes.Iron)
            {
                UnitController unit = col.gameObject.GetComponent<UnitController>();
                NavMeshAgent agent = col.gameObject.GetComponent<NavMeshAgent>();
                if(unit) {
                    if(unit.unitType == "Worker" && selectscript.heldResource > 0 && selectscript.heldResourceType == NodeManager.ResourceTypes.Iron) {
                        StartCoroutine(SmeltIron(selectscript, agent));
                    }
                }
            }
        }
    }

    public IEnumerator SmeltIron(UnitSelection unitSelection, NavMeshAgent currentAgent) {
        while (true)
        {
            yield return new WaitForSeconds(10);
            unitSelection.task = ActionList.Delivering;
            unitSelection.drops = GameObject.FindGameObjectsWithTag("Player 1");
            unitSelection.drops = unitSelection.AdjustDrops(unitSelection.drops);

            if(unitSelection.drops.Length > 0 && unitSelection.task != ActionList.Idle && unitSelection.task != ActionList.Moving ) 
            {
                currentAgent.destination = unitSelection.GetClosestDropOff(unitSelection.drops).transform.position;
                unitSelection.heldResourceType = NodeManager.ResourceTypes.Steel;
                unitSelection.drops = null;
            }
            else
            {
                unitSelection.task = ActionList.Idle;
            }
            yield break;
        }
    }
}

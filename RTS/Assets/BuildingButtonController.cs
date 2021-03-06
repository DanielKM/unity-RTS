﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class BuildingButtonController : MonoBehaviour
{
    public GameObject player;
    public GameObject team;
    ResourceManager RM;
    UIController UI;

    public Button buttonOne;
    public Button barracksButtonOne;
    public Button barracksButtonTwo;

    public Button barracksButtonFour;
    public Button barracksButtonNine;
    public Button barracksButtonTen;

    public Button BuildingCancelButton;

    public AudioSource playerAudio;
    public AudioClip trainVillagerAudio;
    public AudioClip trainFootmanAudio;
    public AudioClip trainSwordsmanAudio;
    public AudioClip trainOutriderAudio;
    public AudioClip trainKnightAudio;
    public AudioClip trainArcherAudio;

    public GameObject villager;

    [SerializeField]
    private float nextSpawnTime;
    public bool isTraining;

    // UI Elements
    private CanvasGroup UnitPanel;
    private CanvasGroup BasicBuildingsPanel;
    private CanvasGroup AdvancedBuildingsPanel;
    private CanvasGroup WorkerPanel;
    private CanvasGroup BuildingProgressPanel;
    private CanvasGroup BuildingActionPanel;
    private CanvasGroup BarracksActionPanel;

    TownHallController townHallScript;
    BarracksController barracksScript;

    InputManager inputScript;
    BuildingController buildingScript;

    UnitController footmanUC;
    UnitController swordsmanUC;
    UnitController villagerUC;
    UnitController outriderUC;
    UnitController knightUC;
    UnitController archerUC;

    UnitController unitUC;

    public GameObject selectedObj;
    private Vector3 spawnPosition;

    [SerializeField]
    private float spawnDelay;

    // Trained units
    public GameObject swordsmanPrefab;
    public GameObject footmanPrefab;
    public GameObject villagerPrefab;
    public GameObject outriderPrefab;
    public GameObject knightPrefab;
    public GameObject archerPrefab;

    // Button text
    Text [] blacksmithText;

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            isTraining = false;
            BuildingProgressPanel = GameObject.Find("BuildingProgressPanel").GetComponent<CanvasGroup>();
            BasicBuildingsPanel = GameObject.Find("BasicBuildingsPanel").GetComponent<CanvasGroup>();
            AdvancedBuildingsPanel = GameObject.Find("AdvancedBuildingsPanel").GetComponent<CanvasGroup>();
            WorkerPanel = GameObject.Find("VillagerPanel").GetComponent<CanvasGroup>();
            BuildingActionPanel = GameObject.Find("BuildingActions").GetComponent<CanvasGroup>();
            BarracksActionPanel = GameObject.Find("BarracksActionPanel").GetComponent<CanvasGroup>();

            // Trained units
            swordsmanUC = swordsmanPrefab.GetComponent<UnitController>();
            footmanUC = footmanPrefab.GetComponent<UnitController>();
            villagerUC = villagerPrefab.GetComponent<UnitController>();
            archerUC = archerPrefab.GetComponent<UnitController>();
            outriderUC = outriderPrefab.GetComponent<UnitController>();
            knightUC = knightPrefab.GetComponent<UnitController>();

            player = GameObject.Find("Game").GetComponent<SaveLoad>().loadedPlayer;
            team = GameObject.Find("Faction");
            inputScript = player.GetComponent<InputManager>();
            RM = team.GetComponent<ResourceManager>();
            UI = player.GetComponent<UIController>();

            BuildingActionPanel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate{HireUnit(villagerPrefab);});

            barracksButtonOne = BarracksActionPanel.transform.GetChild(0).GetComponent<Button>();
            barracksButtonTwo = BarracksActionPanel.transform.GetChild(1).GetComponent<Button>();

            barracksButtonFour = BarracksActionPanel.transform.GetChild(5).GetComponent<Button>();
            barracksButtonNine = BarracksActionPanel.transform.GetChild(8).GetComponent<Button>();
            barracksButtonTen = BarracksActionPanel.transform.GetChild(9).GetComponent<Button>();

            barracksButtonOne.onClick.AddListener(delegate{HireUnit(swordsmanPrefab);});
            barracksButtonTwo.onClick.AddListener(delegate{HireUnit(footmanPrefab);});
            barracksButtonFour.onClick.AddListener(delegate{HireUnit(archerPrefab);});


            barracksButtonNine.onClick.AddListener(delegate{HireUnit(outriderPrefab);});
            barracksButtonTen.onClick.AddListener(delegate{HireUnit(knightPrefab);});

            BarracksActionPanel.transform.GetChild(11).GetComponent<Button>().onClick.AddListener(CancelConstruction);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            if(RM.lumberYardCount > 0) {
                barracksButtonFour.interactable = true;
            } else {
                barracksButtonFour.interactable = false;
            }

            if(RM.blacksmithCount > 0) {
                barracksButtonTwo.interactable = true;
            } else {
                barracksButtonTwo.interactable = false;
            }

            if(RM.stablesCount > 0) {
                barracksButtonNine.interactable = true;
            } else {
                barracksButtonNine.interactable = false;
            }

            if(RM.stablesCount > 0 && RM.blacksmithCount > 0) {
                barracksButtonTen.interactable = true;
            } else {
                barracksButtonTen.interactable = false;
            }
        }
    }

    void HireUnit(GameObject unit) {
        unitUC = unit.GetComponent<UnitController>();
        string notEnoughResourcesText = "";
        bool enoughResources = true;

        if (RM.gold < unitUC.gold){
            notEnoughResourcesText = "Not enough gold!";
            enoughResources = false;
        } else if (RM.wood < unitUC.wood){
            notEnoughResourcesText = "Not enough wood!";
            enoughResources = false;
        } else if (RM.food < unitUC.food){
            notEnoughResourcesText = "Not enough food!";
            enoughResources = false;
        } else if (RM.iron < unitUC.iron){
            notEnoughResourcesText = "Not enough iron!";
            enoughResources = false;
        } else if (RM.steel < unitUC.steel){
            notEnoughResourcesText = "Not enough steel!";
            enoughResources = false;
        } else if (RM.skymetal < unitUC.skymetal){
            notEnoughResourcesText = "Not enough skymetal!";
            enoughResources = false;
        } else if (RM.stone < unitUC.stone){
            notEnoughResourcesText = "Not enough stone!";
            enoughResources = false;
        } else if (RM.housing > RM.maxHousing){
            notEnoughResourcesText = "Not enough housing!";
            enoughResources = false;
        }

        if(enoughResources) {
            // Remove required resources
            RM.gold -= unitUC.gold;
            RM.wood -= unitUC.wood;
            RM.food -= unitUC.food;
            RM.iron -= unitUC.iron;
            RM.steel -= unitUC.steel;
            RM.skymetal -= unitUC.skymetal;
            RM.stone -= unitUC.stone;
            RM.housing += 1;

            // Select current building
            selectedObj = inputScript.selectedObj;
            buildingScript = selectedObj.GetComponent<BuildingController>();

            // Start hiring based on which unittype
            if(unitUC.unitType == "Worker") {
                UI.TownHallTraining();
                townHallScript = selectedObj.GetComponent<TownHallController>();
                playerAudio.clip = trainVillagerAudio;
                townHallScript.HireWorker();
            } else if (unitUC.unitType == "Swordsman" || unitUC.unitType == "Footman" || unitUC.unitType == "Archer" || unitUC.unitType == "Outrider" || unitUC.unitType == "Knight") {
                UI.BarracksTraining();
                barracksScript = selectedObj.GetComponent<BarracksController>();
                barracksScript.Hire(unitUC.unitType);
            }
        } else {
            UI.OpenNoResourcesText(notEnoughResourcesText);
            StartCoroutine(Wait());
        }
    }

    public void CancelConstruction() {
        selectedObj = inputScript.selectedObj;
        FoundationController FC = selectedObj.GetComponent<FoundationController>();
        BuildingController prefabBC = FC.buildingPrefab.GetComponent<BuildingController>();
        
        RM.gold += prefabBC.gold;
        RM.wood += prefabBC.wood;
        RM.food += prefabBC.food;
        RM.iron += prefabBC.iron;
        RM.steel += prefabBC.steel;
        RM.skymetal += prefabBC.skymetal;
        RM.stone += prefabBC.stone;
        Destroy(selectedObj);
        UI.CloseAllPanels();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        //my code here after 3 seconds
        UI.CloseNoResourcesText();
    }
}

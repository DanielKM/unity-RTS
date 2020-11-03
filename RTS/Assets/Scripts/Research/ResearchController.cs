using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ResearchController : MonoBehaviour
{
    public GameObject currentPlayer;
    public GameObject team;
    UIController UI;
    ResourceManager RM;
    InputManager inputScript;
    BuildingController buildingScript;
    BlacksmithController blacksmithScript;
    LumberYardController lumberYardScript;

    //Research variables
    //Basic blacksmithing
    public bool basicBlacksmithing;
    public float basicBlacksmithingGold;
    public float basicBlacksmithingIron;

    public bool basicToolSmithing;
    public float basicToolSmithingGold;
    public float basicToolSmithingIron;

    public bool basicArmourSmithing;
    public float basicArmourSmithingGold;
    public float basicArmourSmithingIron;

    public bool basicWeaponSmithing;
    public float basicWeaponSmithingGold;
    public float basicWeaponSmithingIron;

    //Artisan blacksmithing
    public bool steelSmithing;
    public float steelSmithingGold;
    public float steelSmithingIron;

    public bool artisanToolSmithing;
    public float artisanToolSmithingGold;
    public float artisanToolSmithingIron;

    public bool artisanArmourSmithing;
    public float artisanArmourSmithingGold;
    public float artisanArmourSmithingIron;

    public bool artisanWeaponSmithing;
    public float artisanWeaponSmithingGold;
    public float artisanWeaponSmithingIron;

    //Extra blacksmithing
    public bool horseshoes;
    public float horseshoesGold;
    public float horseshoesIron;

    public bool minecarts;
    public float minecartsGold;
    public float minecartsIron;

    public bool caltrops;
    public float caltropsGold;
    public float caltropsIron;

    public bool reinforcedBuildings;
    public float reinforcedBuildingsGold;
    public float reinforcedBuildingsIron;

    //Research variables
    //Basic woodworking
    public bool buildingScience;
    public float buildingScienceGold;
    public float buildingScienceWood;

    public bool efficientBuildings;
    public float efficientBuildingsGold;
    public float efficientBuildingsWood;

    public bool sturdyConstruction;
    public float sturdyConstructionGold;
    public float sturdyConstructionWood;

    public bool betterGear;
    public float betterGearGold;
    public float betterGearWood;

    //Artisan blacksmithing
    public bool basicWoodworking;
    public float basicWoodworkingGold;
    public float basicWoodworkingWood;

    public bool improvedHandles;
    public float improvedHandlesGold;
    public float improvedHandlesWood;

    public bool wheelbarrows;
    public float wheelbarrowsGold;
    public float wheelbarrowsWood;

    public bool carts;
    public float cartsGold;
    public float cartsWood;
    
    //Extra blacksmithing
    public bool artisanWoodworking;
    public float artisanWoodworkingGold;
    public float artisanWoodworkingWood;

    public bool bridges;
    public float bridgesGold;
    public float bridgesWood;

    public bool walls;
    public float wallsGold;
    public float wallsWood;

    public bool watchTowers;
    public float watchTowersGold;
    public float watchTowersWood;

    //Blacksmith Research Buttons
    public Button basicBlacksmithingButton;
    public Button basicToolSmithingButton;
    public Button basicArmourSmithingButton;
    public Button basicWeaponSmithingButton;
 
    public Button steelSmithingButton;
    public Button artisanToolSmithingButton;
    public Button artisanArmourSmithingButton;
    public Button artisanWeaponSmithingButton;

    public Button horseshoesButton;
    public Button minecartsButton;
    public Button caltropsButton;
    public Button reinforcedBuildingsButton;

    //Lumber Mill Research Buttons
    public Button buildingScienceButton;
    public Button efficientBuildingsButton;
    public Button sturdyConstructionButton;
    public Button betterGearButton;
 
    public Button basicWoodworkingButton;
    public Button improvedHandlesButton;
    public Button wheelbarrowsButton;
    public Button cartsButton;

    public Button artisanWoodworkingButton;
    public Button bridgesButton;
    public Button wallsButton;
    public Button watchTowersButton;
    
    public GameObject selectedObj;
    public string research;
    public bool isTraining;
    public int i = 0;
    public GameObject SaveLoad;

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            team = GameObject.Find("Faction");
            SaveLoad = GameObject.Find("Game");
            currentPlayer = SaveLoad.GetComponent<SaveLoad>().loadedPlayer;
            RM = team.GetComponent<ResourceManager>();
            UI = currentPlayer.GetComponent<UIController>();
            inputScript = currentPlayer.GetComponent<InputManager>();

            // Blacksmith Research
            basicBlacksmithingButton.onClick.AddListener(delegate{SelectBlacksmithResearch("basicBlacksmithing");});
            basicToolSmithingButton.onClick.AddListener(delegate{SelectBlacksmithResearch("basicToolSmithing");});
            basicArmourSmithingButton.onClick.AddListener(delegate{SelectBlacksmithResearch("basicArmourSmithing");});
            basicWeaponSmithingButton.onClick.AddListener(delegate{SelectBlacksmithResearch("basicWeaponSmithing");});
            
            steelSmithingButton.onClick.AddListener(delegate{SelectBlacksmithResearch("steelSmithing");});
            artisanToolSmithingButton.onClick.AddListener(delegate{SelectBlacksmithResearch("artisanToolSmithing");});
            artisanArmourSmithingButton.onClick.AddListener(delegate{SelectBlacksmithResearch("artisanArmourSmithing");});
            artisanWeaponSmithingButton.onClick.AddListener(delegate{SelectBlacksmithResearch("artisanWeaponSmithing");});
            
            horseshoesButton.onClick.AddListener(delegate{SelectBlacksmithResearch("horseshoes");});
            minecartsButton.onClick.AddListener(delegate{SelectBlacksmithResearch("minecarts");});
            caltropsButton.onClick.AddListener(delegate{SelectBlacksmithResearch("caltrops");});
            reinforcedBuildingsButton.onClick.AddListener(delegate{SelectBlacksmithResearch("reinforcedBuildings");});

            // Lumber Mill Research            
            buildingScienceButton.onClick.AddListener(ResearchBuildingScience);
            efficientBuildingsButton.onClick.AddListener(ResearchEfficientBuildings);
            sturdyConstructionButton.onClick.AddListener(ResearchSturdyConstruction);
            betterGearButton.onClick.AddListener(ResearchBetterGear);
            
            basicWoodworkingButton.onClick.AddListener(ResearchBasicWoodworking);
            improvedHandlesButton.onClick.AddListener(ResearchImprovedHandles);
            wheelbarrowsButton.onClick.AddListener(ResearchWheelbarrows);
            cartsButton.onClick.AddListener(ResearchCarts);
            
            artisanWoodworkingButton.onClick.AddListener(ResearchArtisanWoodworking);
            bridgesButton.onClick.AddListener(ResearchBridges);
            wallsButton.onClick.AddListener(ResearchWalls);
            watchTowersButton.onClick.AddListener(ResearchWatchTowers);
        }
    }

    void SelectBlacksmithResearch(string selectedResearch) {
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        blacksmithScript = selectedObj.GetComponent<BlacksmithController>();
        blacksmithScript.StartResearch(selectedResearch);
    }

    // BASIC WOODWORKING
    void ResearchBuildingScience () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        lumberYardScript = selectedObj.GetComponent<LumberYardController>();
        lumberYardScript.ResearchBuildingScience();
    }

    void ResearchEfficientBuildings () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        lumberYardScript = selectedObj.GetComponent<LumberYardController>();
        lumberYardScript.ResearchEfficientBuildings();
    }

    void ResearchSturdyConstruction () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        lumberYardScript = selectedObj.GetComponent<LumberYardController>();
        lumberYardScript.ResearchSturdyConstruction();
    }

    void ResearchBetterGear () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        lumberYardScript = selectedObj.GetComponent<LumberYardController>();
        lumberYardScript.ResearchBetterGear();
    }

    // ARTISAN BLACKSMITHING
    void ResearchBasicWoodworking () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        lumberYardScript = selectedObj.GetComponent<LumberYardController>();
        // lumberYardScript.ResearchBasicWoodworking();
    }

    void ResearchImprovedHandles () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        lumberYardScript = selectedObj.GetComponent<LumberYardController>();
        lumberYardScript.ResearchImprovedHandles();
    }

    void ResearchWheelbarrows () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        lumberYardScript = selectedObj.GetComponent<LumberYardController>();
        lumberYardScript.ResearchWheelbarrows();
    }

    void ResearchCarts () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        lumberYardScript = selectedObj.GetComponent<LumberYardController>();
        lumberYardScript.ResearchCarts();
    }

    // OTHER BLACKSMITHING
    void ResearchArtisanWoodworking () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        lumberYardScript = selectedObj.GetComponent<LumberYardController>();
        lumberYardScript.ResearchArtisanWoodworking();
    }

    void ResearchBridges () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        lumberYardScript = selectedObj.GetComponent<LumberYardController>();
        lumberYardScript.ResearchBridges();
    }

    void ResearchWalls () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        lumberYardScript = selectedObj.GetComponent<LumberYardController>();
        lumberYardScript.ResearchWalls();
    }

    void ResearchWatchTowers () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        lumberYardScript = selectedObj.GetComponent<LumberYardController>();
        lumberYardScript.ResearchWatchTowers();
    }
}

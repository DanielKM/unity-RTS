using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ResearchController : MonoBehaviour
{
    public GameObject player;
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
    public bool artisanBlacksmithing;
    public float artisanBlacksmithingGold;
    public float artisanBlacksmithingIron;

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
 
    public Button artisanBlacksmithingButton;
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
    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            team = GameObject.Find("Faction");
            player = GameObject.FindGameObjectWithTag("Player");
            RM = team.GetComponent<ResourceManager>();
            UI = player.GetComponent<UIController>();
            inputScript = player.GetComponent<InputManager>();

            // Blacksmith Research
            basicBlacksmithingButton.onClick.AddListener(ResearchBlacksmithing);
            basicToolSmithingButton.onClick.AddListener(ResearchBasicToolSmithing);
            basicArmourSmithingButton.onClick.AddListener(ResearchBasicArmourSmithing);
            basicWeaponSmithingButton.onClick.AddListener(ResearchBasicWeaponSmithing);
            
            artisanBlacksmithingButton.onClick.AddListener(ResearchArtisanBlacksmithing);
            artisanToolSmithingButton.onClick.AddListener(ResearchArtisanToolSmithing);
            artisanArmourSmithingButton.onClick.AddListener(ResearchArtisanArmourSmithing);
            artisanWeaponSmithingButton.onClick.AddListener(ResearchArtisanWeaponSmithingButton);
            
            horseshoesButton.onClick.AddListener(ResearchHorseshoes);
            minecartsButton.onClick.AddListener(ResearchMinecarts);
            caltropsButton.onClick.AddListener(ResearchCaltrops);
            reinforcedBuildingsButton.onClick.AddListener(ResearchReinforcedBuildings);

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

    // BASIC BLACKSMITHING
    void ResearchBlacksmithing () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        blacksmithScript = selectedObj.GetComponent<BlacksmithController>();
        blacksmithScript.ResearchBlacksmithing();
    }

    void ResearchBasicToolSmithing () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        blacksmithScript = selectedObj.GetComponent<BlacksmithController>();
        blacksmithScript.ResearchBasicToolSmithing();
    }

    void ResearchBasicArmourSmithing () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        blacksmithScript = selectedObj.GetComponent<BlacksmithController>();
        blacksmithScript.ResearchBasicArmourSmithing();
    }

    void ResearchBasicWeaponSmithing () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        blacksmithScript = selectedObj.GetComponent<BlacksmithController>();
        blacksmithScript.ResearchBasicWeaponSmithing();
    }

    // ARTISAN BLACKSMITHING
    void ResearchArtisanBlacksmithing () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        blacksmithScript = selectedObj.GetComponent<BlacksmithController>();
        blacksmithScript.ResearchArtisanBlacksmithing();
    }

    void ResearchArtisanToolSmithing () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        blacksmithScript = selectedObj.GetComponent<BlacksmithController>();
        blacksmithScript.ResearchArtisanToolSmithing();
    }

    void ResearchArtisanArmourSmithing () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        blacksmithScript = selectedObj.GetComponent<BlacksmithController>();
        blacksmithScript.ResearchArtisanArmourSmithing();
    }

    void ResearchArtisanWeaponSmithingButton () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        blacksmithScript = selectedObj.GetComponent<BlacksmithController>();
        blacksmithScript.ResearchArtisanWeaponSmithing();
    }

    // OTHER BLACKSMITHING
    void ResearchHorseshoes () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        blacksmithScript = selectedObj.GetComponent<BlacksmithController>();
        blacksmithScript.ResearchHorseshoes();
    }

    void ResearchMinecarts () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        blacksmithScript = selectedObj.GetComponent<BlacksmithController>();
        blacksmithScript.ResearchMinecarts();
    }

    void ResearchCaltrops () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        blacksmithScript = selectedObj.GetComponent<BlacksmithController>();
        blacksmithScript.ResearchCaltrops();
    }

    void ResearchReinforcedBuildings () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        blacksmithScript = selectedObj.GetComponent<BlacksmithController>();
        blacksmithScript.ResearchReinforcedBuildings();
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
        lumberYardScript.ResearchBasicWoodworking();
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

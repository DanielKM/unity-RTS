using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ResearchController : MonoBehaviour
{
    public GameObject player;
    UIController UI;
    ResourceManager RM;
    InputManager inputScript;
    BuildingController buildingScript;
    BlacksmithController blacksmithScript;

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
    public bool horshoes;
    public float horshoesGold;
    public float horshoesIron;

    public bool minecarts;
    public float minecartsGold;
    public float minecartsIron;

    public bool caltrops;
    public float caltropsGold;
    public float caltropsIron;

    public bool reinforcedBuildings;
    public float reinforcedBuildingsGold;
    public float reinforcedBuildingsIron;

    //Research Buttons
    public Button basicBlacksmithingButton;
    public Button basicToolSmithingButton;
    public Button basicArmourSmithingButton;
    public Button basicWeaponSmithingButton;
 
    public Button artisanBlacksmithingButton;
    public Button artisanToolSmithingButton;
    public Button artisanArmourSmithingButton;
    public Button artisanWeaponSmithingButton;

    public Button horshoesButton;
    public Button minecartsButton;
    public Button caltropsButton;
    public Button reinforcedBuildingsButton;

    public GameObject selectedObj;
    public string research;
    public bool isTraining;
    public int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            player = GameObject.FindGameObjectWithTag("Player");
            RM = player.GetComponent<ResourceManager>();
            UI = player.GetComponent<UIController>();
            inputScript = player.GetComponent<InputManager>();

            basicBlacksmithingButton.onClick.AddListener(ResearchBlacksmithing);
            basicToolSmithingButton.onClick.AddListener(ResearchBasicToolSmithing);
            basicArmourSmithingButton.onClick.AddListener(ResearchBasicArmourSmithing);
            basicWeaponSmithingButton.onClick.AddListener(ResearchBasicWeaponSmithing);
            
            artisanBlacksmithingButton.onClick.AddListener(ResearchArtisanBlacksmithing);
            artisanToolSmithingButton.onClick.AddListener(ResearchArtisanToolSmithing);
            artisanArmourSmithingButton.onClick.AddListener(ResearchArtisanArmourSmithing);
            artisanWeaponSmithingButton.onClick.AddListener(ResearchArtisanWeaponSmithingButton);
            
            horshoesButton.onClick.AddListener(ResearchHorseshoes);
            minecartsButton.onClick.AddListener(ResearchMinecarts);
            caltropsButton.onClick.AddListener(ResearchCaltrops);
            reinforcedBuildingsButton.onClick.AddListener(ResearchReinforcedBuildings);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (ShouldSpawn())
        //{
        //    Spawn();
        //}
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

    IEnumerator ArtisanResearch()
    {
        yield return new WaitForSeconds(30);
        //my code here after 3 seconds
    }
    
    IEnumerator ExtraResearch()
    {
        yield return new WaitForSeconds(20);
        //my code here after 3 seconds
    }
    
    IEnumerator CloseResourcesText()
    {
        yield return new WaitForSeconds(3);
        //my code here after 3 seconds
        UI.noResourcesText.SetActive(false);
    }
}

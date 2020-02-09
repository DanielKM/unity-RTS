﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithController : MonoBehaviour
{
    UIController UI;
    // footmen names
    string[] footmen = new string[14]{ "Bron", "Darek", "Krom", "Turin", "Zerk", "Rua", "Vos", "Barros", "Braxis", "Kraye", "Sloa", "Kolin", "Kaleb", "Arvan"};
    string[] firstNames = new string[14]{ "Aubrey", "Braum", "Braxis", "Davin", "Garen", "Oren", "Gavin", "Derek", "Kevan", "Stephen", "David", "Ruan", "Edward", "Marcus"};

    string[] lastNameFirst = new string[14]{ "Foe", "Strong", "Ox", "Deer", "Swift", "Bright", "Light", "Dark", "Fire", "Shade", "Stout", "Quick", "Moose", "Dread"};
    string[] lastNameSecond = new string[14]{ "hammer", "fist", "bridge", "wind", "blade", "spear", "shield", "bane", "sheen", "whip", "strike", "stone", "wind", "arm"};

    string[] SMFirstNames = new string[14]{ "Aubrey", "Braum", "Braxis", "Davin", "Garen", "Oren", "Gavin", "Derek", "Kevan", "Stephen", "David", "Ruan", "Edward", "Marcus"};

    string[] SMLastNameFirst = new string[14]{ "Foe", "Fear", "Doom", "Gloom", "Dusk", "Dawn", "Light", "Dark", "Hope", "Swift", "Summer", "Winter", "Fall", "Tall"};
    string[] SMLastNameSecond = new string[14]{ "arm", "fist", "biter", "slayer", "hammer", "fighter", "shield", "crusher", "shredder", "rune", "strike", "stone", "wind", "arm"};


    private float nextSpawnTime;
    public int i = 0;

    private AudioSource swordsmanAudio;
    private AudioSource footmanAudio;
    public AudioClip swordsmanReporting;
    public AudioClip footmanReporting;

    [SerializeField]
    public float spawnDelay;
    public bool selected = false;

    GameObject player;
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


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UI = player.GetComponent<UIController>();
        RM = player.GetComponent<ResourceManager>();
        RC = player.GetComponent<ResearchController>();
        inputScript = player.GetComponent<InputManager>();
        BuildingProgressPanel = GameObject.Find("BuildingProgressPanel").GetComponent<CanvasGroup>();
        BuildingActionPanel = GameObject.Find("BuildingActions").GetComponent<CanvasGroup>();

        // Progress bar
        BuildingProgressBar = GameObject.Find("BuildingProgressBar");
        BuildingProgressSlider = BuildingProgressBar.GetComponent<Slider>();
    }

    public void ResearchBlacksmithing () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();

        if(RM.gold < RC.basicBlacksmithingGold || RM.iron < RC.basicBlacksmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.basicBlacksmithingGold;
            RM.iron -= RC.basicBlacksmithingIron;
            research = "basicBlacksmithing";
            StartCoroutine(Research());
            RC.basicBlacksmithingButton.interactable = false;
        }
    }
    public void ResearchBasicToolSmithing () 
    { 
        if(RM.gold < RC.basicToolSmithingGold || RM.iron < RC.basicToolSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.basicToolSmithingGold;
            RM.iron -= RC.basicToolSmithingIron;
            research = "basicToolSmithing";
            StartCoroutine(Research());
            RC.basicToolSmithingButton.interactable = false;
        }
    }

    public void ResearchBasicArmourSmithing () 
    { 
        if(RM.gold < RC.basicArmourSmithingGold || RM.iron < RC.basicArmourSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.basicArmourSmithingGold;
            RM.iron -= RC.basicArmourSmithingIron;
            research = "basicArmourSmithing";
            StartCoroutine(Research());
            RC.basicArmourSmithingButton.interactable = false;
        }
    }

    public void ResearchBasicWeaponSmithing () 
    { 
        if(RM.gold < RC.basicWeaponSmithingGold || RM.iron < RC.basicWeaponSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.basicWeaponSmithingGold;
            RM.iron -= RC.basicWeaponSmithingIron;
            research = "basicWeaponSmithing";
            StartCoroutine(Research());
            RC.basicWeaponSmithingButton.interactable = false;
        }
    }

    public void ResearchArtisanBlacksmithing () 
    { 
        if(RM.gold < RC.artisanBlacksmithingGold || RM.iron < RC.artisanBlacksmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.artisanBlacksmithingGold;
            RM.iron -= RC.artisanBlacksmithingIron;
            research = "artisanBlacksmithing";
            StartCoroutine(Research());
            RC.artisanBlacksmithingButton.interactable = false;
        }
    }

    public void ResearchArtisanToolSmithing () 
    { 
        if(RM.gold < RC.artisanToolSmithingGold || RM.iron < RC.artisanToolSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.artisanToolSmithingGold;
            RM.iron -= RC.artisanToolSmithingIron;
            research = "artisanToolSmithing";
            StartCoroutine(Research());
            RC.artisanToolSmithingButton.interactable = false;
        }
    }

    public void ResearchArtisanArmourSmithing () 
    { 
        if(RM.gold < RC.artisanArmourSmithingGold || RM.iron < RC.artisanArmourSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.artisanArmourSmithingGold;
            RM.iron -= RC.artisanArmourSmithingIron;
            research = "artisanArmourSmithing";
            StartCoroutine(Research());
            RC.artisanArmourSmithingButton.interactable = false;
        }
    }

    public void ResearchArtisanWeaponSmithing () 
    { 
        if(RM.gold < RC.artisanWeaponSmithingGold || RM.iron < RC.artisanWeaponSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.artisanWeaponSmithingGold;
            RM.iron -= RC.artisanWeaponSmithingIron;
            research = "artisanWeaponSmithing";
            StartCoroutine(Research());
            RC.artisanWeaponSmithingButton.interactable = false;
        }
    }

    public void ResearchHorseshoes () 
    { 
        if(RM.gold < RC.horseshoesGold || RM.iron < RC.horseshoesIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.horseshoesGold;
            RM.iron -= RC.horseshoesIron;
            research = "horseshoes";
            StartCoroutine(Research());
            RC.horseshoesButton.interactable = false;
        }
    }

    public void ResearchMinecarts () 
    { 
        if(RM.gold < RC.minecartsGold || RM.iron < RC.minecartsGold) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.minecartsGold;
            RM.iron -= RC.minecartsIron;
            research = "minecarts";
            StartCoroutine(Research());
            RC.minecartsButton.interactable = false;
        }
    }
    
    public void ResearchCaltrops () 
    { 
        if(RM.gold < RC.caltropsGold || RM.iron < RC.caltropsIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.caltropsGold;
            RM.iron -= RC.caltropsIron;
            research = "caltrops";
            StartCoroutine(Research());
            RC.caltropsButton.interactable = false;
        }
    }
    
    public void ResearchReinforcedBuildings () 
    { 
        if(RM.gold < RC.reinforcedBuildingsGold || RM.iron < RC.reinforcedBuildingsIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.reinforcedBuildingsGold;
            RM.iron -= RC.reinforcedBuildingsIron;
            research = "reinforcedBuildings";
            StartCoroutine(Research());
            RC.reinforcedBuildingsButton.interactable = false;
        }
    }

    IEnumerator CloseResourcesText()
    {
        yield return new WaitForSeconds(3);
        UI.noResourcesText.SetActive(false);
    }

    IEnumerator Research() 
    {
        UI.BlacksmithTraining();
        isTraining = true;
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();

        for (i = 1; i < 11; i++)
        {
            yield return new WaitForSeconds(1);
        }

        if(research == "basicBlacksmithing") {
            RC.basicToolSmithingButton.interactable = true;
            RC.basicArmourSmithingButton.interactable = true;
            RC.basicWeaponSmithingButton.interactable = true;
            RC.artisanBlacksmithingButton.interactable = true;
            RC.basicBlacksmithing = true;
            var colors = RC.basicBlacksmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.basicBlacksmithingButton.colors = colors; 

        } else if (research == "basicToolSmithing") {
            RC.basicToolSmithing = true;
            if(RC.artisanBlacksmithing) {
                RC.artisanToolSmithingButton.interactable = true;
            }
            var colors = RC.basicToolSmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.basicToolSmithingButton.colors = colors; 
        } else if(research == "basicArmourSmithing") {
            RC.basicArmourSmithing = true;
            if(RC.artisanBlacksmithing) {
                RC.artisanArmourSmithingButton.interactable = true;
            }
            var colors = RC.basicArmourSmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.basicArmourSmithingButton.colors = colors; 
        } else if (research == "basicWeaponSmithing") {
            RC.basicWeaponSmithing = true;
            if(RC.artisanBlacksmithing) {
                RC.artisanWeaponSmithingButton.interactable = true;
            }
            var colors = RC.basicWeaponSmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.basicWeaponSmithingButton.colors = colors; 
        } else if (research == "artisanBlacksmithing") {
            RC.artisanBlacksmithing = true;
            if(RC.basicToolSmithing) {
                RC.artisanToolSmithingButton.interactable = true;
            } 
            if(RC.basicArmourSmithing) {
                RC.artisanArmourSmithingButton.interactable = true;
            } 
            if(RC.basicWeaponSmithing) {
                RC.artisanWeaponSmithingButton.interactable = true;
            } 
            var colors = RC.artisanBlacksmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.artisanBlacksmithingButton.colors = colors; 
        } else if (research == "artisanToolSmithing") {
            RC.artisanToolSmithing = true;
            var colors = RC.artisanBlacksmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.artisanBlacksmithingButton.colors = colors; 
        } else if (research == "artisanArmourSmithing") {
            RC.artisanArmourSmithing = true;
            var colors = RC.artisanBlacksmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.artisanBlacksmithingButton.colors = colors; 
        } else if (research == "artisanWeaponSmithing") {
            RC.artisanWeaponSmithing = true;
            var colors = RC.artisanWeaponSmithingButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.artisanWeaponSmithingButton.colors = colors; 
        } else if (research == "horseshoes") {
            RC.horseshoes = true;
            var colors = RC.horseshoesButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.horseshoesButton.colors = colors; 
        } else if (research == "minecarts") {
            RC.minecarts = true;
            var colors = RC.minecartsButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.minecartsButton.colors = colors; 
        } else if (research == "caltrops") {
            RC.caltrops = true;
            var colors = RC.caltropsButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.caltropsButton.colors = colors; 
        } else if (research == "reinforcedBuildings") {
            RC.reinforcedBuildings = true;
            var colors = RC.reinforcedBuildingsButton.colors; 
            colors.disabledColor = new Color(0, 0.4f, 0, 1);
            RC.reinforcedBuildingsButton.colors = colors; 
        }
            
        isTraining = false;
    }
}

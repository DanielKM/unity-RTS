using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResearchController : MonoBehaviour
{
    public GameObject player;
    UIController UI;
    ResourceManager RM;

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


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        RM = player.GetComponent<ResourceManager>();
        UI = player.GetComponent<UIController>();

        basicBlacksmithingButton.onClick.AddListener(ResearchBlacksmithing);
        basicToolSmithingButton.onClick.AddListener(ResearchBasicToolSmithing);
        basicArmourSmithingButton.onClick.AddListener(ResearchBasicArmourSmithing);
        basicWeaponSmithingButton.onClick.AddListener(ResearchBasicWeaponSmithing);
        
        artisanBlacksmithingButton.onClick.AddListener(ResearchArtisanBlacksmithing);
        artisanToolSmithingButton.onClick.AddListener(ResearchArtisanToolSmithing);
        artisanArmourSmithingButton.onClick.AddListener(ResearchArtisanArmourSmithing);
        artisanWeaponSmithingButton.onClick.AddListener(ResearchArtisanWeaponSmithingButton);
        
        horshoesButton.onClick.AddListener(ResearchHorshoes);
        minecartsButton.onClick.AddListener(ResearchMinecarts);
        caltropsButton.onClick.AddListener(ResearchCaltrops);
        reinforcedBuildingsButton.onClick.AddListener(ResearchReinforcedBuildings);
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
        if(RM.gold < basicBlacksmithingGold || RM.iron < basicBlacksmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            basicBlacksmithingButton.interactable = false;
            RM.gold -= basicBlacksmithingGold;
            RM.iron -= basicBlacksmithingIron;
            StartCoroutine(BasicResearch());
            basicBlacksmithing = true;
            Debug.Log("basicBlacksmithing complete");
        }
    }

    void ResearchBasicToolSmithing () 
    { 
        if(RM.gold < basicToolSmithingGold || RM.iron < basicToolSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            basicToolSmithingButton.interactable = false;
            RM.gold -= basicToolSmithingGold;
            RM.iron -= basicToolSmithingIron;
            StartCoroutine(BasicResearch());
            basicToolSmithing = true;
            Debug.Log("basicToolSmithing complete");
        }
    }

    void ResearchBasicArmourSmithing () 
    { 
        if(RM.gold < basicArmourSmithingGold || RM.iron < basicArmourSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            basicArmourSmithingButton.interactable = false;
            RM.gold -= basicArmourSmithingGold;
            RM.iron -= basicArmourSmithingIron;
            StartCoroutine(BasicResearch());
            basicArmourSmithing = true;
            Debug.Log("basicarmourSmithing complete");
        }
    }

    void ResearchBasicWeaponSmithing () 
    { 
        if(RM.gold < basicWeaponSmithingGold || RM.iron < basicWeaponSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            basicWeaponSmithingButton.interactable = false;
            RM.gold -= basicWeaponSmithingGold;
            RM.iron -= basicWeaponSmithingIron;
            StartCoroutine(BasicResearch());
            basicWeaponSmithing = true;
            Debug.Log("basic weapon smithing complete");
        }
    }

    // ARTISAN BLACKSMITHING
    void ResearchArtisanBlacksmithing () 
    { 
        if(RM.gold < artisanBlacksmithingGold || RM.iron < artisanBlacksmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            artisanBlacksmithingButton.interactable = false;
            RM.gold -= artisanBlacksmithingGold;
            RM.iron -= artisanBlacksmithingIron;
            StartCoroutine(BasicResearch());
            artisanBlacksmithing = true;
            Debug.Log("artisan blacksmithing complete");
        }
    }

    void ResearchArtisanToolSmithing () 
    { 
        if(RM.gold < artisanToolSmithingGold || RM.iron < artisanToolSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            artisanToolSmithingButton.interactable = false;
            RM.gold -= artisanToolSmithingGold;
            RM.iron -= artisanToolSmithingIron;
            StartCoroutine(BasicResearch());
            artisanWeaponSmithing = true;
            Debug.Log("artisanToolSmithingGold complete");
        }
    }

    void ResearchArtisanArmourSmithing () 
    { 
        if(RM.gold < artisanArmourSmithingGold || RM.iron < artisanArmourSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            artisanArmourSmithingButton.interactable = false;
            RM.gold -= artisanArmourSmithingGold;
            RM.iron -= artisanArmourSmithingIron;
            StartCoroutine(BasicResearch());
            artisanArmourSmithing = true;
            Debug.Log("artisanArmourSmithingIron complete");
        }
    }

    void ResearchArtisanWeaponSmithingButton () 
    { 
        if(RM.gold < artisanWeaponSmithingGold || RM.iron < artisanWeaponSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            artisanWeaponSmithingButton.interactable = false;
            RM.gold -= artisanWeaponSmithingGold;
            RM.iron -= artisanWeaponSmithingIron;
            StartCoroutine(BasicResearch());
            artisanWeaponSmithing = true;
            Debug.Log("artisan weaponsmithing complete");
        }
    }

    // OTHER BLACKSMITHING
    void ResearchHorshoes () 
    { 
        if(RM.gold < horshoesGold || RM.iron < horshoesIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            horshoesButton.interactable = false;
            RM.gold -= horshoesGold;
            RM.iron -= horshoesIron;
            StartCoroutine(BasicResearch());
            horshoes = true;
            Debug.Log("horseshoes complete");
        }
    }

    void ResearchMinecarts () 
    { 
        if(RM.gold < minecartsGold || RM.iron < minecartsGold) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            minecartsButton.interactable = false;
            RM.gold -= minecartsGold;
            RM.iron -= minecartsIron;
            StartCoroutine(BasicResearch());
            minecarts = true;
            Debug.Log("minecarts complete");
        }
    }

    void ResearchCaltrops () 
    { 
        if(RM.gold < caltropsGold || RM.iron < caltropsIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            caltropsButton.interactable = false;
            RM.gold -= caltropsGold;
            RM.iron -= caltropsIron;
            StartCoroutine(BasicResearch());
            caltrops = true;
            Debug.Log("artisan blacksmithing complete");
        }
    }

    void ResearchReinforcedBuildings () 
    { 
        if(RM.gold < reinforcedBuildingsGold || RM.iron < reinforcedBuildingsIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            reinforcedBuildingsButton.interactable = false;
            RM.gold -= reinforcedBuildingsGold;
            RM.iron -= reinforcedBuildingsIron;
            StartCoroutine(BasicResearch());
            reinforcedBuildings = true;
            Debug.Log("reinforced buildings complete");
        }
    }

    IEnumerator BasicResearch()
    {
        yield return new WaitForSeconds(20);
        //my code here after 3 seconds
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

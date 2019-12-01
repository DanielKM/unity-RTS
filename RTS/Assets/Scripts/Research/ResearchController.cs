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
            RM.gold -= basicBlacksmithingGold;
            RM.iron -= basicBlacksmithingIron;
            StartCoroutine(BasicResearch());
            basicBlacksmithing = true;
            Debug.Log("basicBlacksmithing complete");
        }
    }

    void ResearchBasicToolSmithing () 
    { 
        basicToolSmithing = true;
        Debug.Log("basicToolSmithing complete");
    }

    void ResearchBasicArmourSmithing () 
    { 
        basicArmourSmithing = true;
        Debug.Log("basicArmourSmithing complete");
    }

    void ResearchBasicWeaponSmithing () 
    { 
        basicWeaponSmithing = true;
        Debug.Log("basicWeaponSmithing complete");
    }

    // ARTISAN BLACKSMITHING
    void ResearchArtisanBlacksmithing () 
    { 
        artisanBlacksmithing = true;
        Debug.Log("artisanBlacksmithing complete");
    }

    void ResearchArtisanToolSmithing () 
    { 
        artisanToolSmithing = true;
        Debug.Log("artisanToolSmithing complete");
    }

    void ResearchArtisanArmourSmithing () 
    { 
        artisanArmourSmithing = true;
        Debug.Log("artisanArmourSmithing complete");
    }

    void ResearchArtisanWeaponSmithingButton () 
    { 
        artisanWeaponSmithing = true;
        Debug.Log("artisanWeaponSmithingButton complete");
    }

    // OTHER BLACKSMITHING
    void ResearchHorshoes () 
    { 
        horshoes = true;
        Debug.Log("horshoes complete");
    }

    void ResearchMinecarts () 
    { 
        minecarts = true;
        Debug.Log("minecarts complete");
    }

    void ResearchCaltrops () 
    { 
        caltrops = true;
        Debug.Log("caltrops complete");
    }

    void ResearchReinforcedBuildings () 
    { 
        reinforcedBuildings = true;
        Debug.Log("reinforcedBuildings complete");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResearchController : MonoBehaviour
{
    //Research variables
    public bool basicBlacksmithing;
    public bool basicToolSmithing;
    public bool basicArmourSmithing;
    public bool basicWeaponSmithing;
 
    public bool artisanBlacksmithing;
    public bool artisanToolSmithing;
    public bool artisanArmourSmithing;
    public bool artisanWeaponSmithing;

    public bool horshoes;
    public bool minecarts;
    public bool caltrops;
    public bool reinforcedBuildings;

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
        basicBlacksmithing = true;
        Debug.Log("basicBlacksmithing complete");
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


}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class BarracksController : MonoBehaviour
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

    public GameObject swordsmanPrefab;
    public GameObject footmanPrefab;
    public GameObject archerPrefab;

    private AudioSource swordsmanAudio;
    private AudioSource footmanAudio;
    private AudioSource archerAudio;

    public AudioClip swordsmanReporting;
    public AudioClip footmanReporting;
    public AudioClip archerReporting;

    [SerializeField]
    public float spawnDelay;
    public bool selected = false;

    GameObject player;
    GameObject team;
    InputManager inputScript;
    UnitSelection swordsmanUnitSelection;
    UnitSelection footmanUnitSelection;
    UnitSelection archerUnitSelection;

    BuildingController buildingScript;

    public GameObject selectedObj;
    private Vector3 spawnPosition;
    public bool isTraining;
    public string unit;

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
        team = GameObject.Find("Faction");
        UI = player.GetComponent<UIController>();
        inputScript = player.GetComponent<InputManager>();
        BuildingProgressPanel = GameObject.Find("BuildingProgressPanel").GetComponent<CanvasGroup>();
        BuildingActionPanel = GameObject.Find("BuildingActions").GetComponent<CanvasGroup>();

        // Progress bar
        BuildingProgressBar = GameObject.Find("BuildingProgressBar");
        BuildingProgressSlider = BuildingProgressBar.GetComponent<Slider>();
    }

    public void HireSwordsman()
    {
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        unit = "Swordsman";
        StartCoroutine(Train());
    }

    public void HireFootman()
    {
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        unit = "Footman";
        StartCoroutine(Train());
    }

    public void HireArcher()
    {
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        unit = "Archer";
        StartCoroutine(Train());
    }

    IEnumerator Train() 
    {
        isTraining = true;
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        spawnPosition = new Vector3(buildingScript.location.x, buildingScript.location.y, buildingScript.location.z - 5f);
        nextSpawnTime = Time.time + spawnDelay;
        GameObject prefab = swordsmanPrefab;

        if(unit == "Footman") {
            var iteration1 = Random.Range(0, firstNames.Length);
            var iteration2 = Random.Range(0, lastNameFirst.Length);
            var iteration3 = Random.Range(0, lastNameSecond.Length);
            progressIcon = GameObject.Find("ProgressIcon").GetComponent<Image>();
            progressIcon.sprite = footmanPrefab.GetComponent<UnitController>().unitIcon;
            footmanPrefab.GetComponent<UnitController>().unitName = firstNames[iteration1] + " " + lastNameFirst[iteration2] + lastNameSecond[iteration3];
            footmanUnitSelection = footmanPrefab.GetComponent<UnitSelection>();
            footmanUnitSelection.owner = team;

            footmanAudio = selectedObj.GetComponent<AudioSource>();
            footmanAudio.clip = footmanReporting;
            prefab = footmanPrefab;
        } else if (unit == "Swordsman") {
            var iteration1 = Random.Range(0, SMFirstNames.Length);
            var iteration2 = Random.Range(0, SMLastNameFirst.Length);
            var iteration3 = Random.Range(0, SMLastNameSecond.Length);
            progressIcon = GameObject.Find("ProgressIcon").GetComponent<Image>();
            progressIcon.sprite = swordsmanPrefab.GetComponent<UnitController>().unitIcon;
            swordsmanPrefab.GetComponent<UnitController>().unitName = SMFirstNames[iteration1] + " " + SMLastNameFirst[iteration2] + SMLastNameSecond[iteration3];
            swordsmanUnitSelection = swordsmanPrefab.GetComponent<UnitSelection>();
            swordsmanUnitSelection.owner = team;

            swordsmanAudio = selectedObj.GetComponent<AudioSource>();
            swordsmanAudio.clip = swordsmanReporting;
            prefab = swordsmanPrefab;
        } else if (unit == "Archer") {
            var iteration1 = Random.Range(0, SMFirstNames.Length);
            var iteration2 = Random.Range(0, SMLastNameFirst.Length);
            var iteration3 = Random.Range(0, SMLastNameSecond.Length);
            progressIcon = GameObject.Find("ProgressIcon").GetComponent<Image>();
            progressIcon.sprite = archerPrefab.GetComponent<UnitController>().unitIcon;
            archerPrefab.GetComponent<UnitController>().unitName = SMFirstNames[iteration1] + " " + SMLastNameFirst[iteration2] + SMLastNameSecond[iteration3];
            archerUnitSelection = archerPrefab.GetComponent<UnitSelection>();
            archerUnitSelection.owner = team;

            archerAudio = selectedObj.GetComponent<AudioSource>();
            archerAudio.clip = archerReporting;
            prefab = archerPrefab;
        }

        for (i = 1; i < 11; i++)
        {
            yield return new WaitForSeconds(1);
        }

        Instantiate(prefab, spawnPosition, Quaternion.identity);

        if(unit == "Footman") {
            footmanAudio.Play();
        } else if (unit == "Swordsman") {
            swordsmanAudio.Play();
        } else if (unit == "Swordsman") {
            archerAudio.Play();
        }
        isTraining = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class BarracksController : MonoBehaviour
{
    UIController UI;
    string[] footmen = new string[14]{ "Bron", "Darek", "Krom", "Turin", "Zerk", "Rua", "Vos", "Barros", "Braxis", "Kraye", "Sloa", "Kolin", "Kaleb", "Arvan"};
    string[] firstNames = new string[14]{ "Aubrey", "Braum", "Braxis", "Davin", "Garen", "Oren", "Gavin", "Derek", "Kevan", "Stephen", "David", "Ruan", "Edward", "Marcus"};

    string[] lastNameFirst = new string[14]{ "Foe", "Strong", "Ox", "Deer", "Swift", "Bright", "Light", "Dark", "Fire", "Shade", "Stout", "Quick", "Moose", "Dread"};
    string[] lastNameSecond = new string[14]{ "hammer", "fist", "bridge", "wind", "blade", "spear", "shield", "bane", "sheen", "whip", "strike", "stone", "wind", "arm"};

    private float nextSpawnTime;
    public int i = 0;

    public GameObject footmanPrefab;
    private AudioSource footmanAudio;
    public AudioClip footmanReporting;

    [SerializeField]
    public float spawnDelay;
    public bool selected = false;

    GameObject player;
    InputManager inputScript;
    Selection footmanSelection;
    BuildingController buildingScript;

    public GameObject selectedObj;
    private Vector3 spawnPosition;
    public bool isTraining;

    //Progress bar
    private GameObject VillagerProgressBar;
    private Slider VillagerProgressSlider;
    public Image progressIcon;

    //UI Elements
    private CanvasGroup BuildingProgressPanel;
    private CanvasGroup BuildingActionPanel;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UI = player.GetComponent<UIController>();
        inputScript = player.GetComponent<InputManager>();
        BuildingProgressPanel = GameObject.Find("BuildingProgressPanel").GetComponent<CanvasGroup>();
        BuildingActionPanel = GameObject.Find("BuildingActions").GetComponent<CanvasGroup>();

        // Progress bar
        VillagerProgressBar = GameObject.Find("VillagerProgressBar");
        VillagerProgressSlider = VillagerProgressBar.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (ShouldSpawn())
        //{
        //    Spawn();
        //}
    }

    public void HireFootman()
    {
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();

        StartCoroutine(FootmanSpawn());
    }

    private void Spawn()
    {
        nextSpawnTime = Time.time + spawnDelay;
        Instantiate(footmanPrefab, transform.position, transform.rotation);
    }

    IEnumerator FootmanSpawn() 
    {
        isTraining = true;
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        spawnPosition = new Vector3(buildingScript.location.x, buildingScript.location.y, buildingScript.location.z - 5f);
        nextSpawnTime = Time.time + spawnDelay;

        for (i = 1; i < 11; i++)
        {
            yield return new WaitForSeconds(1);
        }
        isTraining = false;

        var iteration1 = Random.Range(0, firstNames.Length);
        var iteration2 = Random.Range(0, lastNameFirst.Length);
        var iteration3 = Random.Range(0, lastNameSecond.Length);
        progressIcon = GameObject.Find("ProgressIcon").GetComponent<Image>();
        progressIcon.sprite = footmanPrefab.GetComponent<UnitController>().unitIcon;
        footmanPrefab.GetComponent<UnitController>().unitName = firstNames[iteration1] + " " + lastNameFirst[iteration2] + lastNameSecond[iteration3];
        footmanSelection = footmanPrefab.GetComponent<Selection>();
        footmanSelection.owner = player;

        Instantiate(footmanPrefab, spawnPosition, Quaternion.identity);
        footmanAudio = selectedObj.GetComponent<AudioSource>();
        footmanAudio.clip = footmanReporting;
        footmanAudio.Play();
        // UI.BarracksSelect();
    }
}

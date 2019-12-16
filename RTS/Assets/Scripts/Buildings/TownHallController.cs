using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class TownHallController : MonoBehaviour
{
    // Villager names
    UIController UI;
    string[] villagers = new string[14]{ "Farah", "Dan", "Dave", "Steve", "Katie", "Sam", "Ryan", "Sid", "Bill", "Will", "Sarah", "Arj", "Izzy", "Aron"};

    private float nextSpawnTime;
    public int i = 0;

    public GameObject villagerPrefab;
    private AudioSource villagerAudio;
    public AudioClip villagerReporting;

    [SerializeField]
    public float spawnDelay;

    public bool selected = false;

    GameObject player;
    InputManager inputScript;
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

    }

    public void HireVillager()
    {
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();

        StartCoroutine(VillagerSpawn());
     //   nextSpawnTime = Time.time + spawnDelay;
       // Instantiate(villagerPrefab, spawnPosition, Quaternion.identity);
    }

    private bool ShouldSpawn()
    {
        return Time.time >= nextSpawnTime;
    }
    
    IEnumerator VillagerSpawn()
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

        var iteration = Random.Range(0,villagers.Length);
        progressIcon = GameObject.Find("ProgressIcon").GetComponent<Image>();
        progressIcon.sprite = villagerPrefab.GetComponent<UnitController>().unitIcon;
        villagerPrefab.GetComponent<UnitController>().unitName = villagers[iteration];

        Instantiate(villagerPrefab, spawnPosition, Quaternion.identity);
        villagerAudio = selectedObj.GetComponent<AudioSource>();
        villagerAudio.clip = villagerReporting;
        villagerAudio.Play();
        UI.TownHallSelect();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class TownHallController : MonoBehaviour
{
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
    private GameObject PeasantProgressBar;
    private Slider PeasantProgressSlider;

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
        PeasantProgressBar = GameObject.Find("PeasantProgressBar");
        PeasantProgressSlider = PeasantProgressBar.GetComponent<Slider>();

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
        Debug.Log((spawnPosition));
     //   nextSpawnTime = Time.time + spawnDelay;
       // Instantiate(villagerPrefab, spawnPosition, Quaternion.identity);
    }

    private bool ShouldSpawn()
    {
        return Time.time >= nextSpawnTime;
    }


    void HideBuildingProgressPanel()
    {
        BuildingProgressPanel.alpha = 0;
        BuildingProgressPanel.blocksRaycasts = false;
        BuildingProgressPanel.interactable = false;
    }

    void ShowBuildingActionPanel()
    {
        BuildingActionPanel.alpha = 1;
        BuildingActionPanel.blocksRaycasts = true;
        BuildingActionPanel.interactable = true;
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
        Instantiate(villagerPrefab, spawnPosition, Quaternion.identity);

        villagerAudio = selectedObj.GetComponent<AudioSource>();
        villagerAudio.clip = villagerReporting;
        villagerAudio.Play();

        HideBuildingProgressPanel();
        //ShowBuildingActionPanel();
    }
}

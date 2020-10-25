using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class BarracksController : MonoBehaviour
{
    UIController UI;
    // footmen names
    string[] firstNames = new string[30]{ "Aubrey", "Braum", "Braxis", "Davin", "Garen", "Oren", "Gavin", "Derek", "Kevan", "Stephen", "David", "Ruan", "Edward", "Marcus","Dane", "Bron", "Daren", "Darek", "Krom", "Turin", "Zerk", "Rua", "Vos", "Barros", "Braxis", "Kraye", "Sloa", "Kolin", "Kaleb", "Arvan"};

    string[] lastNameFirst = new string[28]{ "Foe", "Fear", "Doom", "Gloom", "Dusk", "Dawn", "Light", "Dark", "Hope", "Swift", "Summer", "Winter", "Fall", "Tall", "Foe", "Strong", "Ox", "Deer", "Swift", "Bright", "Light", "Dark", "Fire", "Shade", "Stout", "Quick", "Moose", "Dread"};
    string[] lastNameSecond = new string[28]{ "arm", "fist", "biter", "slayer", "hammer", "fighter", "shield", "crusher", "shredder", "rune", "strike", "stone", "wind", "arm", "hammer", "fist", "bridge", "wind", "blade", "spear", "shield", "bane", "sheen", "whip", "strike", "stone", "wind", "arm"};

    private float nextSpawnTime;
    public int i = 0;

    public GameObject swordsmanPrefab;
    public GameObject footmanPrefab;
    public GameObject archerPrefab;
    public GameObject outriderPrefab;
    public GameObject knightPrefab;

    private AudioSource barracksAudio;

    public AudioClip swordsmanReporting;
    public AudioClip footmanReporting;
    public AudioClip archerReporting;
    public AudioClip outriderReporting;
    public AudioClip knightReporting;

    [SerializeField]
    public float spawnDelay;
    public bool selected = false;

    GameObject player;
    GameObject team;
    InputManager inputScript;

    UnitSelection selectedUnitSelection;

    BuildingController buildingScript;
    ResourceManager RM;
    ResearchController RC;

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

    // Progress variables
    public int progress = 0;
    public List<GameObject> queuedUnits = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        team = GameObject.Find("Faction");
        UI = player.GetComponent<UIController>();
        RM = team.GetComponent<ResourceManager>();
        RC = team.GetComponent<ResearchController>();
        inputScript = player.GetComponent<InputManager>();
        BuildingProgressPanel = GameObject.Find("BuildingProgressPanel").GetComponent<CanvasGroup>();
        BuildingActionPanel = GameObject.Find("BuildingActions").GetComponent<CanvasGroup>();
        progressIcon = GameObject.Find("ProgressIcon").GetComponent<Image>();

        // Progress bar
        BuildingProgressBar = GameObject.Find("BuildingProgressBar");
        BuildingProgressSlider = BuildingProgressBar.GetComponent<Slider>();
    }

    public void Hire(string unitType) {
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        unit = unitType;
        GameObject prefab = null;
        if(unit == "Footman") {
            prefab = footmanPrefab;
        } else if (unit == "Swordsman") {
            prefab = swordsmanPrefab;
        } else if (unit == "Archer") {
            prefab = archerPrefab;
        } else if (unit == "Outrider") {
            prefab = outriderPrefab;
        } else if (unit == "Knight") {
            prefab = knightPrefab;
        }
        
        queuedUnits.Add(prefab);
        StartCoroutine(Train(selectedObj, prefab));
    }

    IEnumerator Train(GameObject selectedBarracks, GameObject queuedUnit) 
    {
        yield return new WaitUntil(() => isTraining == false);
        isTraining = true;
        buildingScript = selectedBarracks.GetComponent<BuildingController>();
        spawnPosition = new Vector3(buildingScript.location.x, buildingScript.location.y, buildingScript.location.z - 5f);
        nextSpawnTime = Time.time + spawnDelay;
        GameObject prefab = swordsmanPrefab;
        UnitController prefabUnitScript = queuedUnit.GetComponent<UnitController>();


        for (i = 1; i < 11; i++)
        { 
            progress = i;
            yield return new WaitForSeconds(1);
        }
        queuedUnits.RemoveAt(0);
        isTraining = false;

        var iteration1 = Random.Range(0, firstNames.Length);
        var iteration2 = Random.Range(0, lastNameFirst.Length);
        var iteration3 = Random.Range(0, lastNameSecond.Length);
        progressIcon.sprite = prefabUnitScript.unitIcon;
        prefabUnitScript.unitName = firstNames[iteration1] + " " + lastNameFirst[iteration2] + lastNameSecond[iteration3];
        selectedUnitSelection = queuedUnit.GetComponent<UnitSelection>();
        selectedUnitSelection.owner = team;
        barracksAudio = selectedBarracks.GetComponent<AudioSource>();
        prefab = queuedUnit;

        GameObject newUnit = Instantiate(prefab, spawnPosition, Quaternion.identity);
        if(prefabUnitScript.unitType == "Footman") {
            barracksAudio.clip = footmanReporting;
        } else if (prefabUnitScript.unitType == "Swordsman") {
            barracksAudio.clip = swordsmanReporting;
        } else if (prefabUnitScript.unitType == "Archer") {
            barracksAudio.clip = archerReporting;
        } else if (prefabUnitScript.unitType == "Outrider") {
            barracksAudio.clip = outriderReporting;
        } else if (prefabUnitScript.unitType == "Knight") {
            barracksAudio.clip = knightReporting;
        }
        barracksAudio.Play();
        // If the rally point is set go to the rally point
        if(gameObject.transform.position != buildingScript.rallyPoint) {
            newUnit.GetComponent<NavMeshAgent>().destination = buildingScript.rallyPoint;
        }
        if(queuedUnits.Count == 0) {
            UI.CloseTrainingProgressPanel();
        }
    }
}

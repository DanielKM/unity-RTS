using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class BuildingButtonController : MonoBehaviour
{
    public GameObject player;
    ResourceManager RM;
    UIController UI;

    public Button buttonOne;
    public AudioSource playerAudio;
    public AudioClip trainPeasantAudio;

    public GameObject villager;

    [SerializeField]
    private float nextSpawnTime;

    public bool isTraining;

    // UI Elements
    private CanvasGroup UnitPanel;
    private CanvasGroup BasicBuildingsPanel;
    private CanvasGroup AdvancedBuildingsPanel;
    private CanvasGroup PeasantPanel;
    private CanvasGroup BuildingProgressPanel;
    private CanvasGroup BuildingActionPanel;

    TownHallController townHallScript;
    InputManager inputScript;
    BuildingController buildingScript;
    public GameObject selectedObj;
    private Vector3 spawnPosition;

    [SerializeField]
    private float spawnDelay;
    // Start is called before the first frame update
    void Start()
    {
        isTraining = false;
        BuildingProgressPanel = GameObject.Find("BuildingProgressPanel").GetComponent<CanvasGroup>();
        BasicBuildingsPanel = GameObject.Find("BasicBuildingsPanel").GetComponent<CanvasGroup>();
        AdvancedBuildingsPanel = GameObject.Find("AdvancedBuildingsPanel").GetComponent<CanvasGroup>();
        PeasantPanel = GameObject.Find("VillagerPanel").GetComponent<CanvasGroup>();
        BuildingActionPanel = GameObject.Find("BuildingActions").GetComponent<CanvasGroup>();

        player = GameObject.FindGameObjectWithTag("Player");
        inputScript = player.GetComponent<InputManager>();
        RM = player.GetComponent<ResourceManager>();
        UI = player.GetComponent<UIController>();

        buttonOne.onClick.AddListener(HireVillager);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void HireVillager()
    {
        if (RM.gold >= 400 && RM.food >= 200 && RM.housing < RM.maxHousing)
        {
            HideBuildingActionPanel();
            ShowBuildingProgressPanel();
            RM.gold -= 400;
            RM.food -= 200;
            RM.housing += 1;
            selectedObj = inputScript.selectedObj;
            townHallScript = selectedObj.GetComponent<TownHallController>();
            buildingScript = selectedObj.GetComponent<BuildingController>();

           
          //  spawnPosition = new Vector3(buildingScript.location.x, buildingScript.location.y, buildingScript.location.z +50f);

            playerAudio.clip = trainPeasantAudio;
            playerAudio.Play();
            townHallScript.HireVillager();
        }
        else if (RM.gold < 400 || RM.food < 200 || RM.housing >= RM.maxHousing)
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
    }

    void HideBuildingActionPanel()
    {
        BuildingActionPanel.alpha = 0;
        BuildingActionPanel.blocksRaycasts = false;
        BuildingActionPanel.interactable = false;
    }

    void ShowBuildingProgressPanel()
    {
        BuildingProgressPanel.alpha = 1;
        BuildingProgressPanel.blocksRaycasts = true;
        BuildingProgressPanel.interactable = true;
    }

    void ShowBuildingActionPanel()
    {
        BuildingActionPanel.alpha = 1;
        BuildingActionPanel.blocksRaycasts = true;
        BuildingActionPanel.interactable = true;
    }

    void HideBuildingProgressPanel()
    {
        BuildingProgressPanel.alpha = 0;
        BuildingProgressPanel.blocksRaycasts = false;
        BuildingProgressPanel.interactable = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        //my code here after 3 seconds
        UI.noResourcesText.SetActive(false);
    }

}

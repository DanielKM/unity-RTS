using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingController : MonoBehaviour
{
    // Unit variables
    public string unitType;
    public string unitRank;
    public string unitName;
    public int unitKills;

    public string weapon;
    public string armour;
    public string items;

    public int health;
    public int maxHealth;

    public int energy;
    public int maxEnergy;

    public Sprite icon;
    public GameObject foundation;

    // Unit List
    public UnitList UnitList;
    public GameObject player;
    InputManager IM;

    // Placeable bool
    public bool inCollider = false;
    public bool placeable = false;
    public bool isPlaced = true;
    public Vector3 location;

    // Cost
    public int gold;
    public int wood;
    public int food;
    public int stone;
    public int iron;
    public int steel;
    public int skymetal;

    public string buildingID;
    public int mask;
    public Vector3 rallyPoint;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        IM = player.GetComponent<InputManager>();
        UnitList = GameObject.Find("Game").GetComponent<UnitList>();
        mask =~ LayerMask.GetMask("FogOfWar");
    }

    void Start()
    {   
        rallyPoint = gameObject.transform.position;
        UnitList.friendlyBuildings.Add(gameObject);
        if(buildingID == null || buildingID == "") {
            buildingID = System.Guid.NewGuid().ToString();
        }
    }

    void Update()
    {
        location = gameObject.transform.position;
    
        if (Input.GetMouseButtonDown(1) && IM.selectedObj == gameObject)
        {
            BuildingRightClick();
        }
    }

    public void BuildingRightClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 350))
        {   
            SetRallyPoint(hit);
            StartCoroutine(IM.ClickCursorHit(hit));
        }
    }

    void SetRallyPoint(RaycastHit hit) {
        rallyPoint = hit.point;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Ground")
        {
            placeable = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Ground")
        {
            inCollider = true;
            placeable = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        inCollider = false;
        placeable = true;
    }
}
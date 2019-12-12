using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementController : MonoBehaviour
{
    // House Variables
    [SerializeField]
    public GameObject housePrefab;
    [SerializeField]
    private KeyCode newObjectHotkey = KeyCode.H;

    // Town Hall Variables
    [SerializeField]
    public GameObject townHallPrefab;
    [SerializeField]
    private KeyCode townHallObjectHotkey = KeyCode.T;

    // Barracks Variables
    [SerializeField]
    public GameObject barracksPrefab;
    [SerializeField]
    private KeyCode barracksObjectHotkey = KeyCode.B;

    // Money Variables
    private NodeManager.ResourceTypes Skymetal;
    private NodeManager.ResourceTypes Iron;
    private NodeManager.ResourceTypes Steel;
    private NodeManager.ResourceTypes Stone;
    private NodeManager.ResourceTypes Wood;
    private NodeManager.ResourceTypes Food;
    private NodeManager.ResourceTypes Gold;
    private NodeManager.ResourceTypes Housing;

    // Player/Resource Manager
    public GameObject player;
    ResourceManager RM;

    private GameObject currentPlaceableObject;
    private float mouseWheelRotation;
    private float adjustedY;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        RM = player.GetComponent<ResourceManager>();
    }


    // Update is called once per frame
    private void Update()
    {
        HandleNewObjectHotkey();
        if(currentPlaceableObject != null)
        {
            if (!EventSystem.current.IsPointerOverGameObject(-1))
            {
                MoveCurrentPlaceableObjectToMouse();
                //RotateFromMouseWheel();
                ReleaseIfClicked();
            }
        }
    }

    private void ReleaseIfClicked()
    {
        if(Input.GetMouseButtonDown(0) && currentPlaceableObject.tag == "House")
        {
            RM.gold -= 200;
            RM.Wood -= 200;
            currentPlaceableObject = null;
        } else if(Input.GetMouseButtonDown(0) && currentPlaceableObject.tag == "Yard")
        {
            RM.gold -= 1200;
            RM.Wood -= 800;
            RM.lumberYardCount += 1;
            currentPlaceableObject = null;
        }
        else if (Input.GetMouseButtonDown(0) && currentPlaceableObject.tag == "Barracks")
        {
            RM.gold -= 500;
            RM.Wood -= 400;
            RM.barracksCount += 1;
            currentPlaceableObject = null;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            currentPlaceableObject = null;
        }
    }

    private void RotateFromMouseWheel()
    {
        mouseWheelRotation += Input.mouseScrollDelta.y;
        currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 1f);
    }

    private void MoveCurrentPlaceableObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo))
        {
            if (currentPlaceableObject.tag == "House")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, 2f, hitInfo.point.z);
            } else if (currentPlaceableObject.tag == "Yard")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, 0.5f, hitInfo.point.z);
            }
            else if (currentPlaceableObject.tag == "Barracks")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, 0.5f, hitInfo.point.z);
            }
            else
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
            }

            //enable to allow terrain adherence
            //currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void HandleNewObjectHotkey()
    {
        if (Input.GetKeyDown(newObjectHotkey))
        {
            if (currentPlaceableObject == null && RM.gold >= 200 && RM.Wood >= 200)
            {
                currentPlaceableObject = Instantiate(housePrefab);
            }
            else if (currentPlaceableObject == null && RM.gold < 200 || currentPlaceableObject == null && RM.Wood < 200)
            {
                StartCoroutine(Wait());
            }
            else
            {
                Destroy(currentPlaceableObject);
            }
        }else if (Input.GetKeyDown(townHallObjectHotkey))
        {
            if (currentPlaceableObject == null && RM.gold >= 1200 && RM.Wood >= 800)
            {
                currentPlaceableObject = Instantiate(townHallPrefab);
            }
            else if (currentPlaceableObject == null && RM.gold < 1200 || currentPlaceableObject == null && RM.Wood < 800)
            {
                StartCoroutine(Wait());
            }
            else
            {
                Destroy(currentPlaceableObject);
            }
        }
        else if (Input.GetKeyDown(barracksObjectHotkey))
        {
            if (currentPlaceableObject == null && RM.gold >= 500 && RM.Wood >= 400)
            {
                currentPlaceableObject = Instantiate(barracksPrefab);
            }
            else if (currentPlaceableObject == null && RM.gold < 500 || currentPlaceableObject == null && RM.Wood < 400)
            {
                StartCoroutine(Wait());
            }
            else
            {
                Destroy(currentPlaceableObject);
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        //my code here after 3 seconds
    }
}

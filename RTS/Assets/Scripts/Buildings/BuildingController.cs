using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    // Unit variables
    public string unitType;

    public string unitRank;

    public string unitName;

    public string unitKills;

    public string weapon;
    public string armour;
    public string items;

    public int health;
    public int maxHealth;

    public int energy;
    public int maxEnergy;

    public Sprite icon;
    public GameObject foundation;

    // Placeable bool
    public bool inCollider = false;
    public bool placeable = true;
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

    private MeshRenderer[] meshes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        location = gameObject.transform.position;
        if(isPlaced == true)
        {
         //   gameObject.layer = 11;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        foreach (ContactPoint contact in other.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (other.gameObject.tag != "Ground")
        {
            inCollider = true;
            placeable = false;
        }
    }

    void OnCollisionStay(Collision other)
    {
        foreach (ContactPoint contact in other.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (other.gameObject.tag != "Ground")
        {
            inCollider = true;
            placeable = false;
        }
    }

    void OnCollisionExit(Collision other)
    {
        inCollider = false;
        // placeable = true;
    }
}
//COPY UNITCONTROLLEr
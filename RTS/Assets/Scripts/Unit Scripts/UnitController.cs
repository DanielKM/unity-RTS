using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UnitController : MonoBehaviour
{
    // UI Variables
    private bool isSelected;

    // Naming
    public string unitType;
    public string unitRank;
    public string unitName;
    public string unitKills;

    // Equipment
    public string weapon;
    public string armour;
    public string items;

    // Attributes
    public int health;
    public int maxHealth;
    public int energy;
    public int maxEnergy;

    // Cost
    public int gold;
    public int wood;
    public int food;
    public int stone;
    public int iron;
    public int steel;
    public int skymetal;

    private Animator anim;
    private NavMeshAgent agent;
    private Selection selection;
    private Tasklist newTask;
    NodeManager.ResourceTypes resourceType;
    public int heldResource;

    public bool isBuilding;
    public bool isGathering;

    public Sprite unitIcon;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        selection = GetComponent<Selection>();
    }

    // Update is called once per frame
    void Update()
    {
        resourceType = selection.heldResourceType;
        heldResource = selection.heldResource;

        //For attacking
        anim.SetFloat("Speed", agent.velocity.magnitude);
        newTask = selection.task;

        isBuilding = selection.isBuilding;
        isGathering = selection.isGathering;
        
        // Setting animation state
        if(heldResource > 0) {
            if(resourceType == NodeManager.ResourceTypes.Wood) {
                if(isBuilding && newTask == Tasklist.Building || isGathering && newTask == Tasklist.Gathering ) {
                    anim.SetInteger("condition", 5);
                } else if (!isBuilding && !isGathering || newTask != Tasklist.Building && newTask != Tasklist.Gathering) {
                    anim.SetInteger("condition", 4);
                }
            } else {
                if(isBuilding && newTask == Tasklist.Building || isGathering && newTask == Tasklist.Gathering ) {
                    anim.SetInteger("condition", 3);
                } else if (!isBuilding && !isGathering || newTask != Tasklist.Building && newTask != Tasklist.Gathering) {
                    anim.SetInteger("condition", 2);
                }
            }
        } else {
            if(isBuilding && newTask == Tasklist.Building || isGathering && newTask == Tasklist.Gathering ) {
                anim.SetInteger("condition", 1);
            } else if (!isBuilding && !isGathering || newTask != Tasklist.Building && newTask != Tasklist.Gathering) {
                anim.SetInteger("condition", 0);
            }
        }
//         if (Input.GetKeyDown(KeyCode.Mouse1))
//         {
//             anim.SetLayerWeight(1, 1f);
//             anim.SetTrigger("IsAttacking");
// //print("Attacking!");
//         }
//         else
//         {
//             anim.SetLayerWeight(0, 0f);
//         }
    }

}

//https://www.youtube.com/watch?v=sb9jnpN9Chc&index=2&list=PLzDRvYVwl53t1vBNhjHANpXXz5M6EuT1q&t=0s

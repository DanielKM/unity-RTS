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
    public int attackDamage;
    public int attackRange;
    public int attackSpeed;

    // Cost
    public int gold;
    public int wood;
    public int food;
    public int stone;
    public int iron;
    public int steel;
    public int skymetal;

    // Enemy variables
    private UnitController enemyUC;
    private GameObject enemy;
    private int enemyHealth;

    // Audio
    public AudioSource unitAudio;
    public AudioClip metalChop;
    public AudioClip metalChop2;
    public AudioClip metalChop3;
    public AudioClip metalChop4;


    public AudioClip woodChop;

    private Animator anim;
    private NavMeshAgent agent;
    private Selection selection;
    private Tasklist newTask;
    NodeManager.ResourceTypes resourceType;
    public int heldResource;

    public bool isBuilding;
    public bool isGathering;
    public bool isMeleeing;

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
        if(health <= 0) {
            Debug.Log("Play death");
            Destroy(gameObject);
        }
        resourceType = selection.heldResourceType;
        heldResource = selection.heldResource;

        //For attacking
        anim.SetFloat("Speed", agent.velocity.magnitude);
        newTask = selection.task;

        isBuilding = selection.isBuilding;
        isGathering = selection.isGathering;
        isMeleeing = selection.isMeleeing;
        
        // Setting animation state
        if(unitType == "Worker") {
            if(heldResource > 0) {
                if(resourceType == NodeManager.ResourceTypes.Wood) {
                    if(isBuilding && newTask == Tasklist.Building || isGathering && newTask == Tasklist.Gathering || isMeleeing) {
                        anim.SetInteger("condition", 5);
                    } else if (!isBuilding && !isGathering || newTask != Tasklist.Building && newTask != Tasklist.Gathering) {
                        anim.SetInteger("condition", 4);
                    }
                } else {
                    if(isBuilding && newTask == Tasklist.Building || isGathering && newTask == Tasklist.Gathering || isMeleeing) {
                        anim.SetInteger("condition", 3);
                    } else if (!isBuilding && !isGathering || newTask != Tasklist.Building && newTask != Tasklist.Gathering) {
                        anim.SetInteger("condition", 2);
                    }
                }
            } else {
                if(isBuilding && newTask == Tasklist.Building || isGathering && newTask == Tasklist.Gathering || isMeleeing) {
                    anim.SetInteger("condition", 1);
                } else if (!isBuilding && !isGathering || newTask != Tasklist.Building && newTask != Tasklist.Gathering) {
                    anim.SetInteger("condition", 0);
                }
            }
        } else if (unitType == "Footman") {
            if(isMeleeing) {
                anim.SetInteger("condition", 1);
            } else if (!isMeleeing) {
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

    public IEnumerator Attack() {
        enemy = selection.targetNode;
        enemyUC = enemy.GetComponent<UnitController>();

        while(selection.isMeleeing) {                       
            if(unitType == "Worker") {
                unitAudio = agent.GetComponent<AudioSource>();
                unitAudio.clip = woodChop;
                unitAudio.maxDistance = 55;
                unitAudio.Play();
            } else if (unitType == "Footman") {
                AudioClip[] metalAttacks = new AudioClip[4]{ metalChop, metalChop2, metalChop3, metalChop4};
                unitAudio = agent.GetComponent<AudioSource>();
                    
                var random = Random.Range(0, metalAttacks.Length);
                unitAudio.clip = metalAttacks[random];
                unitAudio.maxDistance = 55;
                unitAudio.Play();
            }
            
            enemyUC.health -= attackDamage;
            enemyHealth = enemyUC.health;
            yield return new WaitForSeconds(attackSpeed);
        }
    }
}

//https://www.youtube.com/watch?v=sb9jnpN9Chc&index=2&list=PLzDRvYVwl53t1vBNhjHANpXXz5M6EuT1q&t=0s

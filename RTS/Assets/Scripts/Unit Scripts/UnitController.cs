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
    public string armourType;
    public string items;

    // Attributes
    public float health;
    public float maxHealth;
    public float armour;
    public float maxArmour;
    public int energy;
    public int maxEnergy;
    public float attackDamage;
    public int attackRange;
    public int attackSpeed;
    public float aggroRange;

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
    private GameObject[] enemyUnits;
    private float enemyHealth;

    // Audio
    public AudioSource unitAudio;
    public AudioClip metalChop;
    public AudioClip metalChop2;
    public AudioClip metalChop3;
    public AudioClip metalChop4;
    public AudioClip woodChop;

    // Player scripts
    private GameObject player;
    private ResourceManager RM;

    // Unit scripts
    private Animator anim;
    private NavMeshAgent agent;
    private UnitSelection UnitSelection;
    private Tasklist newTask;

    ResearchController RC;
    UIController UI;

    NodeManager.ResourceTypes resourceType;
    public int heldResource;
    public bool currentlyMeleeing;
    public bool isDead;
    public Sprite unitIcon;
    private bool armourUpgrade;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UI = player.GetComponent<UIController>();
        RM = player.GetComponent<ResourceManager>();
        RC = player.GetComponent<ResearchController>();

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        UnitSelection = GetComponent<UnitSelection>();
    }

    private void Awake()
    {
        InvokeRepeating("Tick", 0, 1.0f);
    }
    // Update is called once per frame
    void Update()
    {
        if(health <= 0) { 
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            if(unitType == "Worker") { 
                anim.SetInteger("condition", 10);
                isDead = true;
                UnitSelection.isBuilding = false;
                UnitSelection.isGathering = false;
                UnitSelection.isFollowing = false;
                UnitSelection.isAttacking = false;
                UnitSelection.isMeleeing = false;
            } else if(unitType == "Footman" || unitType == "Swordsman") {
                anim.SetInteger("condition", 10);
                isDead = true;
                UnitSelection.isBuilding = false;
                UnitSelection.isGathering = false;
                UnitSelection.isFollowing = false;
                UnitSelection.isAttacking = false;
                UnitSelection.isMeleeing = false;
            }
            RM.housing -= 1.0f;
        }

        if(!isDead) {
            resourceType = UnitSelection.heldResourceType;
            heldResource = UnitSelection.heldResource;

            //For attacking
            anim.SetFloat("Speed", agent.velocity.magnitude);
            newTask = UnitSelection.task;

            // Setting animation state
            if(unitType == "Worker") {
                if(heldResource > 0) {
                    if(resourceType == NodeManager.ResourceTypes.Wood) {
                        if(UnitSelection.isBuilding && newTask == Tasklist.Building || UnitSelection.isGathering && newTask == Tasklist.Gathering || UnitSelection.isMeleeing) {
                            anim.SetInteger("condition", 5);
                        } else if (!UnitSelection.isBuilding && !UnitSelection.isGathering || newTask != Tasklist.Building && newTask != Tasklist.Gathering) {
                            anim.SetInteger("condition", 4);
                        }
                    } else {
                        if(UnitSelection.isBuilding && newTask == Tasklist.Building || UnitSelection.isGathering && newTask == Tasklist.Gathering || UnitSelection.isMeleeing) {
                            anim.SetInteger("condition", 3);
                        } else if (!UnitSelection.isBuilding && !UnitSelection.isGathering || newTask != Tasklist.Building && newTask != Tasklist.Gathering) {
                            anim.SetInteger("condition", 2);
                        }
                    }
                } else {
                    if(UnitSelection.isBuilding && newTask == Tasklist.Building || UnitSelection.isGathering && newTask == Tasklist.Gathering || UnitSelection.isMeleeing) {
                        anim.SetInteger("condition", 1);
                    } else if (!UnitSelection.isBuilding && !UnitSelection.isGathering || newTask != Tasklist.Building && newTask != Tasklist.Gathering) {
                        anim.SetInteger("condition", 0);
                    }
                }
            } else if (unitType == "Footman" || unitType == "Swordsman") {
                if(UnitSelection.isMeleeing) {
                    anim.SetInteger("condition", 1);
                } else if (!UnitSelection.isMeleeing ) {
                    anim.SetInteger("condition", 0);
                }
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

    void Tick()
    {
        if(!isDead) {
            if(UnitSelection.owner == UnitSelection.player) {
                enemyUnits = GameObject.FindGameObjectsWithTag("Enemy Unit");
                GameObject currentTarget = GetClosestEnemy(enemyUnits);
                if(currentTarget && !currentTarget.GetComponent<UnitController>().isDead) {
                    if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) < aggroRange)
                    {
                        UnitSelection.targetNode = currentTarget;
                        // Debug.Log("Enemy " + currentTarget.GetComponent<UnitController>().unitType + " spotted!");
                        float dist = Vector3.Distance(agent.transform.position, currentTarget.transform.position);
                        agent.destination = currentTarget.transform.position;
                        UnitSelection.isFollowing = true;

                        if(dist < attackRange && currentTarget != null && !currentTarget.GetComponent<UnitController>().isDead) {
                            UnitSelection.isMeleeing = true;
                            enemy = currentTarget;
                            if(!currentlyMeleeing && enemy != null) {
                                agent.destination = agent.transform.position;
                                StartCoroutine(Attack());
                            }
                        } else {
                            currentlyMeleeing = false;
                            UnitSelection.isAttacking = false;
                            UnitSelection.isMeleeing = false;
                            UnitSelection.isFollowing = false;
                        }
                    } else if (currentTarget == null) {
                        currentlyMeleeing = false;
                        UnitSelection.isAttacking = false;
                        UnitSelection.isMeleeing = false;
                        UnitSelection.isFollowing = false;
                    }
                } else {
                    currentlyMeleeing = false;
                    UnitSelection.isAttacking = false;
                    UnitSelection.isMeleeing = false;
                    UnitSelection.isFollowing = false;
                }
            } 
            // else {
            //     enemyUnits = GameObject.FindGameObjectsWithTag("Selectable");
            //     GameObject currentTarget = GetClosestEnemy(enemyUnits);
            //     if(currentTarget && !currentTarget.GetComponent<UnitController>().isDead) {
            //         if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) < aggroRange)
            //         {
            //             UnitSelection.targetNode = currentTarget;
            //             // Debug.Log("Enemy " + currentTarget.GetComponent<UnitController>().unitType + " spotted!");
            //             float dist = Vector3.Distance(agent.transform.position, currentTarget.transform.position);
            //             agent.destination = currentTarget.transform.position;
            //             UnitSelection.isFollowing = true;

            //             if(dist < attackRange && currentTarget != null) {
            //                 UnitSelection.isMeleeing = true;
            //                 enemy = currentTarget;
            //                 if(!currentlyMeleeing && enemy != null) {
            //                     // StartCoroutine(Attack());
            //                 }
            //             } else {
            //                 currentlyMeleeing = false;
            //                 UnitSelection.isMeleeing = false;
            //                 UnitSelection.isFollowing = false;
            //             }
            //         } else if (currentTarget == null) {
            //             currentlyMeleeing = false;
            //             UnitSelection.isMeleeing = false;
            //             UnitSelection.isFollowing = false;
            //         }
            //     } else {
            //         currentlyMeleeing = false;
            //         UnitSelection.isMeleeing = false;
            //         UnitSelection.isFollowing = false;
            //     }
            // }
        }
    }

        // Find the closest enemy
    GameObject GetClosestEnemy(GameObject[] enemies)
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach(GameObject targetEnemy in enemies)
        {
            if(!targetEnemy.GetComponent<UnitController>().isDead) {
                Vector3 direction = targetEnemy.transform.position - position;
                float distance = direction.sqrMagnitude;
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = targetEnemy;
                }
            }
        }
        return closestEnemy;
    }

    public IEnumerator Attack() {
        currentlyMeleeing = true;

        while(UnitSelection.isMeleeing) {          
            enemy = UnitSelection.targetNode;            
            if(enemy == null) {
                currentlyMeleeing = false;
                UnitSelection.isMeleeing = false;
                UnitSelection.isFollowing = false;
                break;
            } else {
                enemyUC = enemy.GetComponent<UnitController>();    
                if(enemyUC.isDead) {
                    currentlyMeleeing = false;
                    UnitSelection.isMeleeing = false;
                    UnitSelection.isFollowing = false;
                    break;
                }   
            }          
            if(unitType == "Worker") {
                unitAudio = agent.GetComponent<AudioSource>();
                unitAudio.clip = woodChop;
                unitAudio.maxDistance = 55;
                unitAudio.Play();
            } else if (unitType == "Footman" || unitType == "Swordsman") {
                AudioClip[] metalAttacks = new AudioClip[4]{ metalChop, metalChop2, metalChop3, metalChop4};
                unitAudio = agent.GetComponent<AudioSource>();
                    
                var random = Random.Range(0, metalAttacks.Length);
                unitAudio.clip = metalAttacks[random];
                unitAudio.maxDistance = 55;
                unitAudio.Play();
            }

            // Research
            float weaponModifier;
            if(RC.artisanWeaponSmithing) {
                weaponModifier = 2.0f;
            } else if(RC.basicWeaponSmithing) {
                weaponModifier = 1.5f;
            } else {
                weaponModifier = 1.0f;
            }

            if(enemyUC.armour > 0.0f) {
                enemyUC.armour -= weaponModifier;
            } else {
                enemyUC.health -= attackDamage * weaponModifier;
                enemyHealth = enemyUC.health;
            }
            yield return new WaitForSeconds(attackSpeed);
        }
    }
}

//https://www.youtube.com/watch?v=sb9jnpN9Chc&index=2&list=PLzDRvYVwl53t1vBNhjHANpXXz5M6EuT1q&t=0s

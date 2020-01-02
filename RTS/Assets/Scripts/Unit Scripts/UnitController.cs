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
    private int enemyHealth;

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
    private Selection selection;
    private Tasklist newTask;

    NodeManager.ResourceTypes resourceType;
    public int heldResource;
    
    public bool currentlyMeleeing;

    public bool isDead;

    public Sprite unitIcon;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        RM = player.GetComponent<ResourceManager>();

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        selection = GetComponent<Selection>();
    }

    private void Awake()
    {
        InvokeRepeating("Tick", 0, 1.0f);
    }
    // Update is called once per frame
    void Update()
    {
        if(health <= 0) {
            if(unitType == "Worker") { 
                anim.SetInteger("condition", 10);
                isDead = true;
                agent.destination = gameObject.transform.position;
                agent.radius = 0;
                agent.height = 0;
                agent.avoidancePriority = 1;
                selection.isBuilding = false;
                selection.isGathering = false;
                selection.isFollowing = false;
                selection.isAttacking = false;
                selection.isMeleeing = false;
            } else if(unitType == "Footman" || unitType == "Swordsman") {
                anim.SetInteger("condition", 10);
                isDead = true;
                agent.destination = gameObject.transform.position;
                agent.radius = 0;
                agent.height = 0;
                agent.avoidancePriority = 1;
                selection.isBuilding = false;
                selection.isGathering = false;
                selection.isFollowing = false;
                selection.isAttacking = false;
                selection.isMeleeing = false;
            }
            RM.housing -= 1.0f;
        }

        if(!isDead) {
            resourceType = selection.heldResourceType;
            heldResource = selection.heldResource;

            //For attacking
            anim.SetFloat("Speed", agent.velocity.magnitude);
            newTask = selection.task;

            // Setting animation state
            if(unitType == "Worker") {
                if(heldResource > 0) {
                    if(resourceType == NodeManager.ResourceTypes.Wood) {
                        if(selection.isBuilding && newTask == Tasklist.Building || selection.isGathering && newTask == Tasklist.Gathering || selection.isMeleeing) {
                            anim.SetInteger("condition", 5);
                        } else if (!selection.isBuilding && !selection.isGathering || newTask != Tasklist.Building && newTask != Tasklist.Gathering) {
                            anim.SetInteger("condition", 4);
                        }
                    } else {
                        if(selection.isBuilding && newTask == Tasklist.Building || selection.isGathering && newTask == Tasklist.Gathering || selection.isMeleeing) {
                            anim.SetInteger("condition", 3);
                        } else if (!selection.isBuilding && !selection.isGathering || newTask != Tasklist.Building && newTask != Tasklist.Gathering) {
                            anim.SetInteger("condition", 2);
                        }
                    }
                } else {
                    if(selection.isBuilding && newTask == Tasklist.Building || selection.isGathering && newTask == Tasklist.Gathering || selection.isMeleeing) {
                        anim.SetInteger("condition", 1);
                    } else if (!selection.isBuilding && !selection.isGathering || newTask != Tasklist.Building && newTask != Tasklist.Gathering) {
                        anim.SetInteger("condition", 0);
                    }
                }
            } else if (unitType == "Footman" || unitType == "Swordsman") {
                if(selection.isMeleeing) {
                    anim.SetInteger("condition", 1);
                } else if (!selection.isMeleeing ) {
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
            if(selection.owner == selection.player) {
                enemyUnits = GameObject.FindGameObjectsWithTag("Enemy Unit");
                GameObject currentTarget = GetClosestEnemy(enemyUnits);
                if(currentTarget && !currentTarget.GetComponent<UnitController>().isDead) {
                    if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) < aggroRange)
                    {
                        selection.targetNode = currentTarget;
                        // Debug.Log("Enemy " + currentTarget.GetComponent<UnitController>().unitType + " spotted!");
                        float dist = Vector3.Distance(agent.transform.position, currentTarget.transform.position);
                        agent.destination = currentTarget.transform.position;
                        selection.isFollowing = true;

                        if(dist < attackRange && currentTarget != null && !currentTarget.GetComponent<UnitController>().isDead) {
                            selection.isMeleeing = true;
                            enemy = currentTarget;
                            if(!currentlyMeleeing && enemy != null) {
                                StartCoroutine(Attack());
                            }
                        } else {
                            currentlyMeleeing = false;
                            selection.isMeleeing = false;
                            selection.isFollowing = false;
                        }
                    } else if (currentTarget == null) {
                        currentlyMeleeing = false;
                        selection.isMeleeing = false;
                        selection.isFollowing = false;
                    }
                } else {
                    currentlyMeleeing = false;
                    selection.isMeleeing = false;
                    selection.isFollowing = false;
                }
            } else {
                enemyUnits = GameObject.FindGameObjectsWithTag("Selectable");
                GameObject currentTarget = GetClosestEnemy(enemyUnits);
                if(currentTarget && !currentTarget.GetComponent<UnitController>().isDead) {
                    if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) < aggroRange)
                    {
                        selection.targetNode = currentTarget;
                        // Debug.Log("Enemy " + currentTarget.GetComponent<UnitController>().unitType + " spotted!");
                        float dist = Vector3.Distance(agent.transform.position, currentTarget.transform.position);
                        agent.destination = currentTarget.transform.position;
                        selection.isFollowing = true;

                        if(dist < attackRange && currentTarget != null) {
                            selection.isMeleeing = true;
                            enemy = currentTarget;
                            if(!currentlyMeleeing && enemy != null) {
                                // StartCoroutine(Attack());
                            }
                        } else {
                            currentlyMeleeing = false;
                            selection.isMeleeing = false;
                            selection.isFollowing = false;
                        }
                    } else if (currentTarget == null) {
                        currentlyMeleeing = false;
                        selection.isMeleeing = false;
                        selection.isFollowing = false;
                    }
                } else {
                    currentlyMeleeing = false;
                    selection.isMeleeing = false;
                    selection.isFollowing = false;
                }
            }
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


        while(selection.isMeleeing) {          
            enemy = selection.targetNode;            
            
            if(enemy == null) {
                currentlyMeleeing = false;
                selection.isMeleeing = false;
                selection.isFollowing = false;
                break;
            } else {
                enemyUC = enemy.GetComponent<UnitController>();    
                if(enemyUC.isDead) {
                    currentlyMeleeing = false;
                    selection.isMeleeing = false;
                    selection.isFollowing = false;
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
            
            enemyUC.health -= attackDamage;
            enemyHealth = enemyUC.health;
            yield return new WaitForSeconds(attackSpeed);
        }
    }
}

//https://www.youtube.com/watch?v=sb9jnpN9Chc&index=2&list=PLzDRvYVwl53t1vBNhjHANpXXz5M6EuT1q&t=0s

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    // UI Variables
    private bool isSelected;

    // Naming
    public string unitType;
    public string unitRank;
    public string unitName;
    public int unitKills;

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
    public float attackSpeed;
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
    private GameObject[] enemyUnits;
    private GameObject currentTarget; 
    private float enemyHealth;

    // Unit List
    public UnitList UnitList;

    // Projectiles
    private GameObject arrowPrefab;
    private GameObject fireballPrefab;
    public GameObject boneshardsPrefab;

    // Audio
    public AudioSource unitAudio;
    public AudioClip metalChop;
    public AudioClip metalChop2;
    public AudioClip metalChop3;
    public AudioClip metalChop4;
    public AudioClip woodChop;

    public AudioClip shootArrow;
    public AudioClip shootFireball;

    // Player scripts
    private GameObject player;
    private GameObject team;
    private ResourceManager RM;

    // Unit scripts
    private Animator anim;
    private NavMeshAgent agent;
    private UnitSelection UnitSelection;
    private ActionList newTask;
    private ArcherController archer;
    private WizardController wizard;
    private NecromancerController necromancer;

    ResearchController RC;
    UIController UI;

    NodeManager.ResourceTypes resourceType;
    public int heldResource;
    public bool currentlyMeleeing;
    public bool isDead;
    public bool justKilled;
    public Sprite unitIcon;
    private bool armourUpgrade;

    public string unitID;

    void Awake() {
        justKilled = true;
        UnitList = GameObject.Find("Game").GetComponent<UnitList>();
        InvokeRepeating("Tick", 0, 1.0f);
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        team = GameObject.Find("Faction");

        UI = player.GetComponent<UIController>();
        RM = team.GetComponent<ResourceManager>();
        RC = team.GetComponent<ResearchController>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        UnitSelection = GetComponent<UnitSelection>();
        archer = GetComponent<ArcherController>();
        wizard = GetComponent<WizardController>();
        necromancer = GetComponent<NecromancerController>();
        if(archer) {
            arrowPrefab = archer.arrow;
        }        
        if(wizard) {
            fireballPrefab = wizard.fireball;
        }      
        if(necromancer) {
            boneshardsPrefab = necromancer.boneshards;
        }
        if(unitID == null || unitID == "") {
            unitID = System.Guid.NewGuid().ToString();
        }

        if(UnitSelection.owner == UnitSelection.team) {
            UnitList.friendlyUnits.Add(gameObject);
        } else {
            UnitList.enemyUnits.Add(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0) { 
            if(justKilled) {
                health = 0;
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                if(unitType == "Worker" || unitType == "Footman" || unitType == "Swordsman" || unitType == "Archer" || unitType == "Wizard" ||  unitType == "Outrider" || unitType == "Knight" || unitType == "Bandit Swordsman"  || unitType == "Bandit Footman" || unitType == "Skeleton" || unitType == "Necromancer")  { 
                    anim.SetInteger("condition", 10);
                    isDead = true;
                    UnitSelection.isBuilding = false;
                    UnitSelection.isGathering = false;
                    UnitSelection.isFollowing = false;
                    UnitSelection.isAttacking = false;
                    UnitSelection.isMeleeing = false;
                    if(gameObject.GetComponent<NPCController>()) {
                        gameObject.GetComponent<NPCController>().currentlyMeleeing = false;
                    }
                }
                RM.housing -= 1.0f;
                
                if(UnitSelection.owner == UnitSelection.team) {
                    UnitList.friendlyUnits.Remove(gameObject);
                    UnitList.friendlyDead.Add(gameObject);
                } else {
                    UnitList.enemyUnits.Remove(gameObject);
                    UnitList.enemyDead.Add(gameObject);
                }
                justKilled = false;
            }
        } else if (health > 0) {
            if(unitType == "Worker" || unitType == "Footman" || unitType == "Swordsman" || unitType == "Archer" || unitType == "Wizard" ||  unitType == "Outrider" || unitType == "Knight" || unitType == "Bandit Swordsman"  || unitType == "Bandit Footman" || unitType == "Skeleton" || unitType == "Necromancer")  { 
                anim.SetInteger("condition", 0);
                isDead = false;
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
            }
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
                        if(UnitSelection.isBuilding && newTask == ActionList.Building || UnitSelection.isGathering && newTask == ActionList.Gathering || UnitSelection.isMeleeing) {
                            anim.SetInteger("condition", 5);
                        } else if (!UnitSelection.isBuilding && !UnitSelection.isGathering || newTask != ActionList.Building && newTask != ActionList.Gathering) {
                            anim.SetInteger("condition", 4);
                        }
                    } else {
                        if(UnitSelection.isBuilding && newTask == ActionList.Building || UnitSelection.isGathering && newTask == ActionList.Gathering || UnitSelection.isMeleeing) {
                            anim.SetInteger("condition", 3);
                        } else if (!UnitSelection.isBuilding && !UnitSelection.isGathering || newTask != ActionList.Building && newTask != ActionList.Gathering) {
                            anim.SetInteger("condition", 2);
                        }
                    }
                } else {
                    if(UnitSelection.isBuilding && newTask == ActionList.Building || UnitSelection.isGathering && newTask == ActionList.Gathering || UnitSelection.isMeleeing) {
                        anim.SetInteger("condition", 1);
                    } else if (!UnitSelection.isBuilding && !UnitSelection.isGathering || newTask != ActionList.Building && newTask != ActionList.Gathering) {
                        anim.SetInteger("condition", 0);
                    }
                }
            } else if (unitType == "Footman" || unitType == "Swordsman" || unitType == "Archer" || unitType == "Wizard" ||  unitType == "Outrider" || unitType == "Knight" || unitType == "Bandit Swordsman" || unitType == "Bandit Footman" || unitType == "Skeleton" || unitType == "Necromancer" ) {
                if(UnitSelection.isMeleeing) {
                    anim.SetInteger("condition", 1);
                } else if (!UnitSelection.isMeleeing) {
                    anim.SetInteger("condition", 0);
                }
            }
            if(unitType == "Necromancer") {
                if(necromancer.raisingDead) {
                    anim.SetInteger("condition", 5);
                } else {
                    anim.SetInteger("condition", 0);
                }
            }
        }
    }

    void Tick()
    {
        if(!isDead) {
            UnitSelection = GetComponent<UnitSelection>();
            if(UnitSelection.owner == UnitSelection.team) {
                enemyUnits = GameObject.FindGameObjectsWithTag("Enemy Unit");
                currentTarget = GetClosestEnemy(enemyUnits);
                if(currentTarget && !currentTarget.GetComponent<UnitController>().isDead) {
                    if(agent) {
                        float dist = Vector3.Distance(agent.transform.position, currentTarget.transform.position);
                        if(dist <= aggroRange && dist <= attackRange) {
                            UnitSelection.isMeleeing = true;
                            UnitSelection.isFollowing = false;
                            agent.destination = agent.transform.position;
                            agent.transform.LookAt(currentTarget.transform.position);
                            if(!currentlyMeleeing) {
                                StartCoroutine(Attack(currentTarget, agent.transform.rotation));
                            }
                        } else if (dist <= aggroRange && dist > attackRange) {
                            UnitSelection.isMeleeing = false;
                            UnitSelection.isFollowing = true;
                            agent.destination = currentTarget.transform.position;
                        } else {
                            UnitSelection.isMeleeing = false;
                            UnitSelection.isFollowing = false;
                        }
                    }
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

    public IEnumerator Attack(GameObject target, Quaternion currentRotation) {
        currentlyMeleeing = true;
        while(UnitSelection.isMeleeing) {    
            target = currentTarget;       
            if(target == null) {
                currentlyMeleeing = false;
                UnitSelection.isMeleeing = false;
                UnitSelection.isFollowing = false;
                break;
            } else {
                enemyUC = target.GetComponent<UnitController>();    
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
            } else if (unitType == "Footman" || unitType == "Swordsman" || unitType == "Outrider" || unitType == "Knight" || unitType == "Bandit Swordsman" || unitType == "Bandit Footman" || unitType == "Skeleton") {
                AudioClip[] metalAttacks = new AudioClip[4]{ metalChop, metalChop2, metalChop3, metalChop4};
                unitAudio = agent.GetComponent<AudioSource>();
                    
                var random = Random.Range(0, metalAttacks.Length);
                unitAudio.clip = metalAttacks[random];
                unitAudio.maxDistance = 55;
                unitAudio.Play();
            } else if (unitType == "Archer") {
                unitAudio = agent.GetComponent<AudioSource>();
                unitAudio.clip = shootArrow;

                Vector3 arrowPosition = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
                GameObject arrow = Instantiate(arrowPrefab, arrowPosition, Quaternion.identity);

                Vector3 heading = target.transform.position - transform.position;
                float newAngle = Vector3.Angle(transform.forward, Vector3.right);

                arrow.transform.rotation = Quaternion.Euler(new Vector3(0,180 - newAngle, 0));
                arrow.GetComponent<Rigidbody>().velocity = new Vector3( heading.x, heading.y + 5.0f, heading.z);
                
                yield return new WaitForSeconds(1.02f);
                Destroy(arrow);
                unitAudio.maxDistance = 55;
                unitAudio.Play();
            } else if (unitType == "Wizard") {
                unitAudio = agent.GetComponent<AudioSource>();
                unitAudio.clip = shootFireball;

                Vector3 fireballPosition = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
                GameObject fireball = Instantiate(fireballPrefab, fireballPosition, Quaternion.identity);

                Vector3 heading = target.transform.position - transform.position;
                float newAngle = Vector3.Angle(transform.forward, Vector3.right);

                fireball.transform.rotation = Quaternion.Euler(new Vector3(0,180 - newAngle, 0));
                fireball.GetComponent<Rigidbody>().velocity = new Vector3( heading.x, heading.y + 5.0f, heading.z);
                
                // yield return new WaitForSeconds(0.3f);
                // Destroy(fireball);
                unitAudio.maxDistance = 55;
                unitAudio.Play();
            } else if (unitType == "Necromancer") {
                unitAudio = agent.GetComponent<AudioSource>();
                unitAudio.clip = shootFireball;

                Vector3 boneshardsPosition = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
                GameObject fireball = Instantiate(boneshardsPrefab, boneshardsPosition, Quaternion.identity);

                Vector3 heading = target.transform.position - transform.position;
                float newAngle = Vector3.Angle(transform.forward, Vector3.right);

                fireball.transform.rotation = Quaternion.Euler(new Vector3(0,180 - newAngle, 0));
                fireball.GetComponent<Rigidbody>().velocity = new Vector3( heading.x, heading.y + 5.0f, heading.z);
                
                // yield return new WaitForSeconds(0.3f);
                // Destroy(fireball);
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

            // Actual damage
            if(enemyUC.armour > 0.0f) {
                enemyUC.armour -= weaponModifier;
            } else {
                enemyUC.health -= attackDamage * weaponModifier;
                enemyHealth = enemyUC.health;
            }   
            yield return new WaitForSeconds(attackSpeed);
        }
        currentlyMeleeing = false;

    }
}

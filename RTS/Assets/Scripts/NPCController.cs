using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public float patrolTime = 15; // time in seconds to wait before going to next patrol destination
    public float aggroRange = 10; // distance in scene units below which the NPC will increase speed and seek the player
    public Transform[] waypoints; // collection of waypoints which define a patrol area

    int index; // the current waypoint index in the waypoints array
    float speed, agentSpeed; // current agent speed and NavMeshAgent component speed
    //Transform player; // reference to the player object transform

    //private List<GameObject> player = new List<GameObject>();

    private GameObject[] playerunits;

    Animator animator; // reference to the animator component
    NavMeshAgent agent; //reference to the navmeshagent
    UnitController UC; //reference to the navmeshagent
    Selection selection; //reference to the navmeshagent

    // Enemy variables
    private UnitController enemyUC;
    private GameObject enemy;
    private int enemyHealth;

    public bool currentlyMeleeing;
    // Start is called before the first frame update
    void Start()
    {
        playerunits = GameObject.FindGameObjectsWithTag("Selectable");
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        UC = GetComponent<UnitController>();
        selection = GetComponent<Selection>();

        if (agent != null)
        {
            agentSpeed = agent.speed;
        }

        //player = List.FindGameObjectsWithTag("Selectable");
        index = Random.Range(0, waypoints.Length);

        InvokeRepeating("Tick", 0, 1.0f);

        if (waypoints.Length > 0)
        {
            InvokeRepeating("Patrol", Random.Range(0, patrolTime), patrolTime);
        }
    }

    void Patrol()
    {
        index = index == waypoints.Length - 1 ? 0 : index + 1; // check through waypoints/cycle thru at patroltime
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

    void Tick()
    {
        if(!UC.isDead) {
            playerunits = GameObject.FindGameObjectsWithTag("Selectable");
            agent.destination = waypoints[index].position;
            agent.speed = agentSpeed / 2;
            
            GameObject currentTarget = GetClosestEnemy(playerunits);

            if(currentTarget && !currentTarget.GetComponent<UnitController>().isDead) {
                if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) < aggroRange)
                {
                    selection.targetNode = currentTarget;
                    // Debug.Log("Enemy " + currentTarget.GetComponent<UnitController>().unitType + " spotted!");
                    float dist = Vector3.Distance(agent.transform.position, currentTarget.transform.position);
                    agent.destination = currentTarget.transform.position;
                    agent.speed = agentSpeed;
                    selection.isFollowing = true;

                    if(dist < UC.attackRange && currentTarget != null) {
                        selection.isMeleeing = true;
                        enemy = currentTarget;
                        if(!currentlyMeleeing && !enemy.GetComponent<UnitController>().isDead) {
                            StartCoroutine(NPCAttack());
                        }
                    } else if (dist < UC.attackRange && currentTarget == null) {
                        currentlyMeleeing = false;
                        selection.isAttacking = false;
                        selection.isMeleeing = false;
                        selection.isFollowing = false;
                    }
                } else if (currentTarget == null) {
                    currentlyMeleeing = false;
                    selection.isAttacking = false;
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

    public IEnumerator NPCAttack() {
        
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

            if(UC.unitType == "Worker") {
                UC.unitAudio = agent.GetComponent<AudioSource>();
                UC.unitAudio.clip = UC.woodChop;
                UC.unitAudio.maxDistance = 55;
                UC.unitAudio.Play();
            } else if (UC.unitType == "Footman" || UC.unitType == "Swordsman") {
                AudioClip[] metalAttacks = new AudioClip[4]{ UC.metalChop, UC.metalChop2, UC.metalChop3, UC.metalChop4};
                UC.unitAudio = agent.GetComponent<AudioSource>();
                    
                var random = Random.Range(0, metalAttacks.Length);
                UC.unitAudio.clip = metalAttacks[random];
                UC.unitAudio.maxDistance = 55;
                UC.unitAudio.Play();
            }
            enemyUC.health -= UC.attackDamage;
            enemyHealth = enemyUC.health;
            yield return new WaitForSeconds(UC.attackSpeed);
        }
    }
}

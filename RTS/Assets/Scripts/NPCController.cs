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

    // Start is called before the first frame update
    void Start()
    {
        playerunits = GameObject.FindGameObjectsWithTag("Selectable");
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agentSpeed = agent.speed;
        }

        //player = List.FindGameObjectsWithTag("Selectable");
        index = Random.Range(0, waypoints.Length);

        InvokeRepeating("Tick", 0, 0.5f);

        if (waypoints.Length > 0)
        {
            InvokeRepeating("Patrol", Random.Range(0, patrolTime), patrolTime);
        }
    }

    void Patrol()
    {
        index = index == waypoints.Length - 1 ? 0 : index + 1; // check through waypoints/cycle thru at patroltime
    }

    void Tick()
    {
        agent.destination = waypoints[index].position;
        agent.speed = agentSpeed / 2;
        for(int i=0; i<playerunits.Length; i++)
        {
            if (playerunits[i] != null && Vector3.Distance(transform.position, playerunits[i].transform.position) < aggroRange)
            {
                agent.destination = playerunits[i].transform.position;
                agent.speed = agentSpeed;
            }
        }
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
}

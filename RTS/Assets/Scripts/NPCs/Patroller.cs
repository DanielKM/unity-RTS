using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Patroller : MonoBehaviour
{
    public float patrolTime = 15; // time in seconds to wait before going to next patrol destination
    public float aggroRange = 10; // distance in scene units below which the NPC will increase speed and seek the player

    int index; // the current waypoint index in the waypoints array
    float speed, agentSpeed; // current agent speed and NavMeshAgent component speed

    NavMeshAgent agent;
    Animator animator;
    public Transform target;
    Vector3 lastKnownPosition;
    public Transform eye;

    bool patrolling;
    public Transform[] patrolTargets;
    private int destPoint;
    bool arrived;
    //private object pathPending;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        lastKnownPosition = transform.position;
    }

    bool CanSeeTarget()
    {
        bool canSee = false;
        Ray ray = new Ray(eye.position, target.transform.position - eye.position);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.transform != target)
            {
                canSee = false;
            } else
            {
                lastKnownPosition = target.transform.position;
                canSee = true;
            }
        }
        return canSee;

    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
        if(agent.pathPending) {
            return;
        }

        if(patrolling)
        {
            if(agent.remainingDistance < agent.stoppingDistance)
            {
                if(!arrived)
                {
                    arrived = true;
                    StartCoroutine("GoToNextPoint");
                }
            }
            else
            {
                arrived = false;
            }
        }
        if(CanSeeTarget())
        {
            agent.SetDestination(target.transform.position);

            patrolling = false;
            if(agent.remainingDistance < agent.stoppingDistance)
            {
                animator.SetBool("Attack", true);
            } else
            {
                animator.SetBool("Attack", false);
            }
        }
        else
        {
            //anim.SetBool("Attack", false);
            if(!patrolling)
            {
                agent.SetDestination(lastKnownPosition);
                if(agent.remainingDistance < agent.stoppingDistance)
                {
                    patrolling = true;
                    StartCoroutine("GoToNextPoint");
                }
            }
        }
        //anim.SetFloat("Forward", agent.velocity.sqrMagnitude);
    }

    IEnumerator GoToNextPoint()
    {
        if(patrolTargets.Length == 0)
        {
            yield break;
        }
        patrolling = true;
        yield return new WaitForSeconds(2f);
        arrived = false;
        agent.destination = patrolTargets[destPoint].position;
        destPoint = (destPoint + 1) % patrolTargets.Length;
    }

}

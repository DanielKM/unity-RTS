using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UnitController : MonoBehaviour
{
    // UI Variables
    private bool isSelected;

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

    private Animator anim;
    private NavMeshAgent agent;

    public Sprite unitIcon;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //For attacking
        anim.SetFloat("Speed", agent.velocity.magnitude);
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            anim.SetLayerWeight(1, 1f);
            anim.SetTrigger("IsAttacking");
//print("Attacking!");
        }
        else
        {
            anim.SetLayerWeight(0, 0f);
        }
    }

}

//https://www.youtube.com/watch?v=sb9jnpN9Chc&index=2&list=PLzDRvYVwl53t1vBNhjHANpXXz5M6EuT1q&t=0s

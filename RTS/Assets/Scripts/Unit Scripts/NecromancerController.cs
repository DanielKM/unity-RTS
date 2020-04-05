using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NecromancerController : MonoBehaviour
{
    // UI Variables
    public GameObject boneshards;
    
    DayNightCycle DayNight;
    public GameObject faction;
    public GameObject MainCamera;
    public Transform TownCenter;

    public GameObject skeleton;
    public bool raisingDead = false;

    private Vector3 spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        faction = GameObject.Find("Faction");
        MainCamera = GameObject.Find("Main Camera");
        DayNight = MainCamera.GetComponent<DayNightCycle>();
        TownCenter = GameObject.Find("TownCenter").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameObject.GetComponent<UnitController>().isDead) {
            if(DayNight.time >= 14400 && DayNight.time <= 16000 ) {
                if(!raisingDead) {
                    StartCoroutine(RaiseDead(DayNight.days));
                }          
            }
        }
        if(!gameObject.GetComponent<UnitController>().isDead) {
            if(DayNight.time >= 68000 && DayNight.time <= 68500 ) {
                if(!raisingDead) {
                    StartCoroutine(RaiseDead(DayNight.days));
                }          
            }
        }
        if(!gameObject.GetComponent<UnitController>().isDead) {
            if(DayNight.time >= 86000 && DayNight.time <= 86300 ) {
                if(!raisingDead) {
                    StartCoroutine(RaiseDead(DayNight.days));
                }          
            }
        }
    }

    IEnumerator RaiseDead(int number) {
        raisingDead = true;
        yield return new WaitForSeconds(10);
        spawnPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 5f);
        Transform[] loadedTransforms = new Transform[1];
        loadedTransforms[0] = TownCenter;
        for(int i=0; i<number + 1; i++) {
            skeleton.GetComponent<NPCController>().waypoints = loadedTransforms;
            Instantiate(skeleton, spawnPosition, Quaternion.identity);  
            raisingDead = false;
        }
    }
}

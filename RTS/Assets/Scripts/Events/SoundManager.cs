using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource SoundAudio;
    public AudioClip nightAudioClip;
    public AudioClip dayAudioClip;

    public AudioClip nightScream;

    DayNightCycle DayNight;
    public GameObject faction;
    public GameObject MainCamera;

    private bool scream = false;
    private bool day = true;
    private bool night = false;
    
    GameObject[] allGameObjects;
    // Start is called before the first frame update
    void Start()
    {
        SoundAudio = gameObject.GetComponent<AudioSource>();
        faction = GameObject.Find("Faction");
        MainCamera = GameObject.Find("Main Camera");
        DayNight = MainCamera.GetComponent<DayNightCycle>();
    }

    // Update is called once per frame
    void Update()
    {
        //RAISE THE DEAD!
        if(DayNight.time >= 68000 && DayNight.time <= 68500 ) {
            if(scream == false) {
                SoundAudio.clip = nightScream;
                SoundAudio.Play();
                day = false;
                scream = true;
            }
            allGameObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach(GameObject go in allGameObjects) 
            {
                if(go.GetComponent<UnitController>()) {
                    if(go.GetComponent<UnitController>().unitType == "Skeleton" && go.GetComponent<UnitController>().isDead) {
                        go.GetComponent<UnitController>().isDead = false;
                        go.GetComponent<UnitController>().justKilled = true;
                        go.GetComponent<UnitController>().health = 75;
                    }
                }
            }
        }

        if(DayNight.time >= 70000 && DayNight.time <= 80000 ) {
            if(night == false) {
                SoundAudio.clip = nightAudioClip;
                SoundAudio.Play();
                scream = false;
                night = true;
            }
        }

        if(DayNight.time >= 25000 && DayNight.time <=  25500 ) {
            if(day == false) {
                SoundAudio.clip = dayAudioClip;
                SoundAudio.Play();
                night = false;
                day = true;
            }
        }
    }
}

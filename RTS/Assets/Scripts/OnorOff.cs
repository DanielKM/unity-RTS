/**
* OnorOff.cs set a boolean based on the checkbox or toggle state
* Author:  Lisa Walkosz-Migliacio  http://evilisa.com  12/18/2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnorOff : MonoBehaviour {

    public bool on;

	// Use this for initialization
	void Start () {
		
	}

    public void onChange()
    {
        if (GetComponent<Toggle>())
        {
            on = GetComponent<Toggle>().isOn;
        }
        else if (GetComponent<Slider>())
        {
            float value = GetComponent<Slider>().value;
            on = (value == 1) ? true : false;
            if (on)
            {
                transform.Find("Background").GetComponent<Image>().color = Color.green;
            }
            else
            {
                transform.Find("Background").GetComponent<Image>().color = Color.white;
            }
        }
        if (on)
        {
            Debug.Log("toggle shows: ON");
        }
        else
        {
            Debug.Log("toggle shows: OFF");
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}

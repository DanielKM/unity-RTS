/**
* Rotate.cs Move a 2D sprite around in a circle
* Author:  Lisa Walkosz-Migliacio  http://evilisa.com  12/18/2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward * 90 * Time.deltaTime);
    }
}

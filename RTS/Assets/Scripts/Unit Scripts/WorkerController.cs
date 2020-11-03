using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerController : MonoBehaviour
{
    public bool clearingDead;
    UnitButtonController UBC;

    // Start is called before the first frame update
    void Start()
    {
        UBC = GameObject.Find("Game").GetComponent<SaveLoad>().loadedPlayer.GetComponent<UnitButtonController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Selectable" || other.gameObject.tag == "Enemy Unit" && other.gameObject.GetComponent<UnitController>().isDead && gameObject.GetComponent<UnitSelection>().targetNode == other.gameObject && clearingDead)
        {
            StartCoroutine(ClearDead(other.gameObject));
        }
    }

    IEnumerator ClearDead(GameObject other)
    {
        yield return new WaitForSeconds(3);
        if(other.activeSelf) {
            UBC.dead.Remove(other);
            other.SetActive(false);
            StartCoroutine(UBC.ClearingDead(gameObject)); 
        }
        // StartCoroutine(UBC.ClearingDead(gameObject)); 
        //my code here after 3 seconds
    }
}

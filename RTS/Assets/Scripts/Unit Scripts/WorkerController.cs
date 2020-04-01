using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy Unit" && other.gameObject.GetComponent<UnitController>().isDead && gameObject.GetComponent<UnitSelection>().targetNode == other.gameObject)
        {
            StartCoroutine(ClearDead(other.gameObject));
        }
    }

    IEnumerator ClearDead(GameObject other)
    {
        yield return new WaitForSeconds(3);
        Destroy(other);
        //my code here after 3 seconds
    }
}

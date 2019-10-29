using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksController : MonoBehaviour
{
    private float nextSpawnTime;

    [SerializeField]
    public GameObject footmanPrefab;

    [SerializeField]
    public float spawnDelay;

    public bool selected = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (ShouldSpawn())
        //{
        //    Spawn();
        //}
    }

    private void Spawn()
    {
        nextSpawnTime = Time.time + spawnDelay;
        Instantiate(footmanPrefab, transform.position, transform.rotation);
    }

    private bool ShouldSpawn()
    {
        return Time.time >= nextSpawnTime;
    }
}

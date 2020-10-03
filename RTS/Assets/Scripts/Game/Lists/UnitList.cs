using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitList : MonoBehaviour
{
    public List<GameObject> enemyUnits;
    public List<GameObject> enemyInjured;
    public List<GameObject> enemyDead;

    public List<GameObject> enemyBuildings;
    public List<GameObject> enemySackedBuildings;
    public List<GameObject> enemyDestroyedBuildings;

    public List<GameObject> friendlyUnits;
    public List<GameObject> friendlyInjured;
    public List<GameObject> friendlyDead;
    
    public List<GameObject> friendlyBuildings;
    public List<GameObject> friendlySackedBuildings;
    public List<GameObject> friendlyDestroyedBuildings;

    public List<GameObject> selectableUnits;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}

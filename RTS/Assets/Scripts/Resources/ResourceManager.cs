using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// PART 13 - 4:17

public class ResourceManager : MonoBehaviour
{
    
    public float skymetal;
    public float maxSkymetal;

    public float iron;
    public float maxIron;

    public float steel;
    public float maxSteel;

    public float stone;
    public float maxStone;

    public float wood;
    public float maxWood;

    public float food;
    public float maxFood;

    public float gold;
    public float maxGold;

    public float housing;
    public float maxHousing;

    public Text SkyMetalDisp;
    public Text IronDisp;
    public Text SteelDisp;
    public Text WoodDisp;
    public Text FoodDisp;
    public Text StoneDisp;
    public Text GoldDisp;
    public Text HousingDisp;

    public GameObject[] houses;
    public GameObject[] oneFoodUnit;
    private int housingTotal;
    private int requiredfood;

    public float barracksCount;
    public float houseCount;
    public float fortCount;
    public float farmCount;
    public float stablesCount;
    public float townHallCount;
    public float lumberYardCount;
    public float blacksmithCount;

    // Start is called before the first frame update
    void Start()
    {
        houses = GameObject.FindGameObjectsWithTag("House");
        oneFoodUnit = GameObject.FindGameObjectsWithTag("Selectable");
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            SkyMetalDisp.text = "" + skymetal + "/" + maxSkymetal;
            IronDisp.text = "" + iron + "/" + maxIron;
            SteelDisp.text = "" + steel + "/" + maxSteel;
            WoodDisp.text = "" + wood + "/" + maxWood;
            FoodDisp.text = "" + food + "/" + maxFood;
            StoneDisp.text = "" + stone + "/" + maxStone;
            GoldDisp.text = "" + gold + "/" + maxGold;

            oneFoodUnit = GameObject.FindGameObjectsWithTag("Selectable");
            housingTotal = oneFoodUnit.Length;

            for(int i=0; i<oneFoodUnit.Length; i++) {
                if(oneFoodUnit[i].GetComponent<UnitController>().isDead) {
                    housingTotal -= 1;
                }
            }
            requiredfood = housingTotal;

            houses = GameObject.FindGameObjectsWithTag("House");

            housing = requiredfood;
            maxHousing = houses.Length * 5;
            HousingDisp.text = "" + housing + "/" + maxHousing;

            if(gold >= 5000) {
                Debug.Log("You win: Economic victory!");
            }
        }
    }
}

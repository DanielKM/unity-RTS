using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillagerController : MonoBehaviour
{
    // UI Variables
    private CanvasGroup InfoPanel;
    
    private bool isSelected;

    // Unit variables
    public Text typeDisp;
    public string unitType;

    public Text rankDisp;
    public string unitRank;

    public Text nameDisp;
    public string unitName;

    public Text killsDisp;
    public string unitKills;

    public string weapon;
    public string armour;
    public string items;

    public Slider HB;
    public Text healthDisp;
    public int health;
    public int maxHealth;

    public Slider EB;
    public Text energyDisp;
    public int energy;
    public int maxEnergy;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanelController : MonoBehaviour
{
    public ResourceManager RM;

    public ResourceView IronView;
    public ResourceView SteelView;
    public ResourceView SkyMetalView;
    public ResourceView GoldView;
    public ResourceView WoodView;
    public ResourceView StoneView;

    void Start() {
        IronView.setMax((int)RM.maxIron);
        SteelView.setMax((int)RM.maxSteel);
        SkyMetalView.setMax((int)RM.maxSkymetal);
        GoldView.setMax((int)RM.maxGold);
        WoodView.setMax((int)RM.maxWood);
        StoneView.setMax((int)RM.maxStone);
    }

    public void UpdateCurrentValues() {
        IronView.setCurrent((int)RM.iron);
        SteelView.setCurrent((int)RM.steel);
        SkyMetalView.setCurrent((int)RM.skymetal);
        GoldView.setCurrent((int)RM.gold);
        WoodView.setCurrent((int)RM.wood);
        StoneView.setCurrent((int)RM.stone);
    }
}

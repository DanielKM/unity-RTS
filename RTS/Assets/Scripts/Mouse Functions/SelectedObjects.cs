using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObjects
{
    // Start is called before the first frame update
    public string name;
    public int power;

    public List<SelectedObjects> selectedObjects = new List<SelectedObjects>();

    public SelectedObjects(string newName, int newPower)
    {
        name = newName;
        power = newPower;
    }

}

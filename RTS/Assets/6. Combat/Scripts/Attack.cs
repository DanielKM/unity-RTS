using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    private readonly int _damage;
    private readonly bool _critical;

    public Attack(int damage, bool critical)
    {
        _damage = damage;
        _critical = critical;
    }

    public int Damage
    {
        get { return _damage; }
    }

    public bool isCritical
    {
        get { return _critical; }
    }


}

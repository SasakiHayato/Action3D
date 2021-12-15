using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase, IDamage
{
    void Update()
    {
        Tree.Repeater(this);
    }

    public float AddDamage() => 1;
    public void GetDamage(float damage)
    {

    }
}

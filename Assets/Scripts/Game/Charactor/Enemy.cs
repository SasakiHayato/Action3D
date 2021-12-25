using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase, IDamage
{
    void Update()
    {
        Tree.Repeater(this);
    }

    public void GetDamage(float damage)
    {

    }

    public override void KnockBack(Vector3 dir)
    {
        
    }
}

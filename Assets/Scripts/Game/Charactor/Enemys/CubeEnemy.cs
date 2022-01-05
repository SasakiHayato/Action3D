using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEnemy : EnemyBase, IDamage
{
    bool _isKnockBack = false;

    void Update()
    {
        SetKnockBack(ref _isKnockBack);
        Tree.Repeater(this);
    }

    public void GetDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0) base.Dead(gameObject);
    }

    public override void KnockBack(Vector3 dir)
    {
        _isKnockBack = true;
        MoveDir = dir;
    }
}

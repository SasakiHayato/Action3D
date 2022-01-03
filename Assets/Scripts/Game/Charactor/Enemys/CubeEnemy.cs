using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEnemy : EnemyBase, IDamage
{
    [SerializeField] int _hp;
    bool _isKnockBack = false;

    void Update()
    {
        SetKnockBack(ref _isKnockBack);
        Tree.Repeater(this);
    }

    public void GetDamage(int damage)
    {
        _hp -= damage;
        if (_hp <= 0) base.Dead(gameObject);
    }

    public override void KnockBack(Vector3 dir)
    {
        _isKnockBack = true;
        MoveDir = dir;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase, IDamage
{
    bool _isKnockBack = false;
    Vector3 _moveDir = Vector3.zero;
    float _timer = 0;

    void Update()
    {
        if (_isKnockBack)
        {
            _timer += Time.deltaTime;
            if (_timer > GetKnockBackTime) _isKnockBack = false;
            Character.Move(_moveDir * Time.deltaTime * GetKnockBackPower);
            return;
        }

        Tree.Repeater(this);
    }

    public void GetDamage(float damage)
    {

    }

    public override void KnockBack(Vector3 dir)
    {
        _timer = 0;
        _isKnockBack = true;
        _moveDir = dir;
    }
}

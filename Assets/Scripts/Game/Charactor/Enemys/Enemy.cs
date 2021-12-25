using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase, IDamage
{
    [SerializeField] float _hp;

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
        _hp -= damage;
        if (_hp <= 0) base.Dead(gameObject);
    }

    public override void KnockBack(Vector3 dir)
    {
        _timer = 0;
        _isKnockBack = true;
        _moveDir = dir;
    }
}

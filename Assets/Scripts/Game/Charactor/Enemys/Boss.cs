using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;

public class Boss : EnemyBase, IDamage
{
    bool _isBackKnock = false;

    AttackSettings _attack;

    void Start()
    {
        _attack = GetComponent<AttackSettings>();
    }

    void Update()
    {
        SetKnockBack(ref _isBackKnock);

        Tree.Repeater(this);
        Vector3 set = Vector3.Scale(MoveDir * Speed, PhsicsBase.GetVelocity);
        Character.Move(set * Time.deltaTime);
    }

    public void GetDamage(int damage)
    {
        HP -= damage;
        if (HP < 0) base.Dead(gameObject);
    }

    public override void KnockBack(Vector3 dir)
    {
        _isBackKnock = true;
        MoveDir = dir;
    }
}

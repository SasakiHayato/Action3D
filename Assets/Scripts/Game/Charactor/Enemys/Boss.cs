using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;

public class Boss : EnemyBase, IDamage
{
    bool _isBackKnock = false;

    AttackSettings _attack;
    StateMachine _state;

    void Start()
    {
        _attack = GetComponent<AttackSettings>();
        _state = GetComponent<StateMachine>();
    }

    void Update()
    {
        SetKnockBack(ref _isBackKnock);
        Tree.Repeater(this);

        Vector3 move = Vector3.zero;

        if (_state.IsRunning) move = _state.Move;
        else move = MoveDir;
        
        Vector3 set = Vector3.Scale(move * Speed, PhsicsBase.GetVelocity);
        Character.Move(set * Time.deltaTime);
    }

    public void GetDamage(int damage, AttackType type)
    {
        if (!_state.IsRunning) return;
        HP -= damage;
        if (HP < 0) base.Dead(gameObject);
    }

    public override void KnockBack(Vector3 dir)
    {
        _isBackKnock = true;
        MoveDir = dir;
    }
}

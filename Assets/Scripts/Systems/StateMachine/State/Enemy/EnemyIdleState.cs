using UnityEngine;
using StateMachine;
using System;

public class EnemyIdleState : State
{
    [SerializeField] string _animName;
    Animator _anim;
    EnemyBase _enemyBase;

    public override void SetUp(GameObject user)
    {
        _enemyBase = user.GetComponent<EnemyBase>();
        _anim = user.GetComponent<Animator>();
    }

    public override void Entry(Enum before)
    {
        if (_animName != "")
        {
            _anim.Play(_animName);
        }
    }

    public override void Run()
    {
        _enemyBase.MoveDir = Vector3.zero;
    }

    public override Enum Exit()
    {
        return EnemyBase.State.Idle;
    }
}

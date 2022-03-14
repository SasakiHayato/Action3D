using UnityEngine;
using StateMachine;
using System;

public class EnemyKnockBack : State
{
    [SerializeField] string _animName;
    EnemyBase _enemyBase;
    Animator _anim;

    bool _isSetUp = false;
    float _timer;
    float _gravity = Physics.gravity.y * -1;
    float _speed;

    public override void SetUp(GameObject user)
    {
        _enemyBase = user.GetComponent<EnemyBase>();
        _anim = user.GetComponent<Animator>();

        _speed = _enemyBase.Speed;
    }

    public override void Entry(Enum beforeType)
    {
        if (_animName != "") _anim.Play(_animName);

        if (_enemyBase.KnonckForwardPower == 0 && _enemyBase.KnonckUpPower == 0) _isSetUp = false;
        else
        {
            _timer = 0;
            _enemyBase.Speed = 1;
            _isSetUp = true;
        }
    }

    public override void Run()
    {
        _timer += Time.deltaTime;

        _enemyBase.PhsicsBase.SetVelocity = Vector3.one;

        float vY = _enemyBase.KnonckUpPower - _gravity * _timer;
        float vX = _enemyBase.KnonckForwardPower - _gravity * _timer;

        if (vY <= 0 && vX <= 0) _isSetUp = false;

        Vector3 setVec = _enemyBase.KnockDir * _enemyBase.KnonckForwardPower;
        setVec.y = 1;
        setVec.y *= _enemyBase.KnonckUpPower;

        _enemyBase.MoveDir = setVec;
    }

    public override Enum Exit()
    {
        if (!_isSetUp)
        {
            _enemyBase.Speed = _speed;
            return EnemyBase.State.BehaviorTree;
        }
        else return EnemyBase.State.KnockBack;
    }
}
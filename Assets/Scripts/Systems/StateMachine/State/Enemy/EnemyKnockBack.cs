using UnityEngine;
using StateMachine;
using System;
using NewAttacks;

/// <summary>
/// ノックバックした際の制御クラス
/// </summary>

public class EnemyKnockBack : State
{
    [SerializeField] string _animName;
    EnemyBase _enemyBase;
    Animator _anim;

    PhysicsBase _physicsBase;
    AttackSettings _settings;
   
    bool _isSetUp = false;
    float _speed;

    public override void SetUp(GameObject user)
    {
        _enemyBase = user.GetComponent<EnemyBase>();
        _anim = user.GetComponent<Animator>();
        _physicsBase = user.GetComponent<PhysicsBase>();
        _settings = user.GetComponent<AttackSettings>();
       
        _speed = _enemyBase.Speed;
    }

    public override void Entry(Enum beforeType)
    {
        if (_enemyBase.KnonckForwardPower == 0 && _enemyBase.KnonckUpPower == 0) _isSetUp = false;
        else
        {
            _enemyBase.Speed = 1;
            _isSetUp = true;
            _settings?.Cancel();
          
            Impluse();
        }
    }

    public override void Run()
    {
        _enemyBase.MoveDir = Vector3.one;

        if (_physicsBase.CurrentForceType != PhysicsBase.ForceType.Impulse) _isSetUp = false;
    }

    void Impluse()
    {
        float vY = _enemyBase.KnonckUpPower;
        float vX = _enemyBase.KnonckForwardPower;

        Vector3 setVec = _enemyBase.KnockDir * _enemyBase.KnonckForwardPower;
        setVec.y = 1;
        setVec.y *= _enemyBase.KnonckUpPower;

        _physicsBase.Force(PhysicsBase.ForceType.Impulse, setVec, vX + vY);
        if (_physicsBase.ImpulsePower <= 0) _isSetUp = false;
        else
        {
            if (_animName != "")
            {
                _anim.Play(_animName);
            }
        }
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
using UnityEngine;
using NewAttacks;
using StateMachine;
using System;

public class PlayerKnockBack : State
{
    [SerializeField] float _knockTime;
    [SerializeField] float _power;

    AttackSettings _attack;
    PhysicsBase _physics;

    Player _player;

    float _speed;
    bool _isSetUp = false;

    public override void SetUp(GameObject user)
    {
        _attack = user.GetComponent<AttackSettings>();
        _player = user.GetComponent<Player>();
        _physics = user.GetComponent<PhysicsBase>();

        _speed = _player.Speed;
    }

    public override void Entry(Enum beforeType)
    {
        if (_player.KnonckForwardPower == 0 && _player.KnonckUpPower == 0) _isSetUp = false;
        else
        {
            _player.Speed = 1;
            _isSetUp = true;
            _attack.Cancel();

            Impluse();
        }
    }

    public override void Run()
    {
        _player.Move = Vector3.one;

        if (_physics.CurrentForceType != PhysicsBase.ForceType.Impulse) _isSetUp = false;
    }

    void Impluse()
    {
        float vY = _player.KnonckUpPower;
        float vX = _player.KnonckForwardPower;

        Vector3 setVec = _player.KnockDir * _player.KnonckForwardPower;
        setVec.y = 1;
        setVec.y *= _player.KnonckUpPower;
        
        _physics.Force(PhysicsBase.ForceType.Impulse, setVec, vX + vY);
        if (_physics.ImpulsePower <= 0)
        {
            _isSetUp = false;
        }
        else
        {
            _player.AnimController.RequestAnim("Damage_Front_Big_ver_C");
        }
    }

    public override Enum Exit()
    {
        if (_isSetUp) return Player.State.KnockBack;
        else
        {
            _player.Speed = _speed;
            return Player.State.Idle;
        }
    }
}

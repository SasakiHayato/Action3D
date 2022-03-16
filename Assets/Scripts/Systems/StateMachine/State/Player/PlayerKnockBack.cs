using UnityEngine;
using NewAttacks;
using StateMachine;
using System;

public class PlayerKnockBack : State
{
    [SerializeField] float _knockTime;
    [SerializeField] float _power;

    Vector3 _setDir = Vector3.zero;
    float _timer = 0;
    bool _isKnockBack = false;

    Animator _anim = null;
    AttackSettings _attack;

    Player _player;

    public override void SetUp(GameObject user)
    {
        _anim = user.GetComponent<Animator>();
        _attack = user.GetComponent<AttackSettings>();
        _player = user.GetComponent<Player>();
    }

    public override void Entry(Enum beforeType)
    {
        
        _attack.Cancel();
        _anim.Play("Damage_Front_Big_ver_C");

        _isKnockBack = true;
        _setDir = _player.GetKnockDir;
        _setDir.y = 0;
        _timer = 0;
    }

    public override void Run()
    {
        _timer += Time.deltaTime;
        if (_timer > _knockTime) _isKnockBack = false;
        _player.Move = _setDir * _power;
    }

    public override Enum Exit()
    {
        if (_isKnockBack) return Player.State.KnockBack;
        else return Player.State.Idle;
    }
}

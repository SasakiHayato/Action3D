using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockBack : StateMachine.State
{
    [SerializeField] Player _player;
    [SerializeField] float _knockTime;
    [SerializeField] float _power;

    Vector3 _setDir = Vector3.zero;
    float _timer = 0;
    bool _isKnockBack = false;

    public override void Entry(StateMachine.StateType beforeType)
    {
        _isKnockBack = true;
        _setDir = _player.GetKnockDir;
        _timer = 0;
    }

    public override void Run(out Vector3 move)
    {
        _timer += Time.deltaTime;
        if (_timer > _knockTime) _isKnockBack = false;
        move = _setDir * _power;
    }

    public override StateMachine.StateType Exit()
    {
        if (_isKnockBack) return StateMachine.StateType.KnockBack;
        else return StateMachine.StateType.Idle;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossMove : StateMachine.State
{
    [SerializeField] float _towardDist;
    [SerializeField] float _moveRate;

    GameObject _player = null;
    
    public override void Entry(StateMachine.StateType beforeType)
    {
        if (_player == null) _player = GameObject.FindWithTag("Player");
    }

    public override void Run(out Vector3 move)
    {
        Vector3 mPos = Target.transform.position;
        float dist = Vector3.Distance(_player.transform.position, mPos);

        if (_towardDist < dist)
        {
            move = (_player.transform.position - mPos).normalized / _moveRate;
            move.y = 1;
        }
        else
        {
            move = Vector3.zero;
        }
    }

    public override StateMachine.StateType Exit()
    {
        return StateMachine.StateType.Move;
    }
}

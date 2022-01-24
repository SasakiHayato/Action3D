using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossMove : StateMachine.State
{
    [SerializeField] float _towardDist;
    [SerializeField] float _changeStateDist;
    [SerializeField] float _moveRate;
    [SerializeField] float _sideMoveRate;

    enum DirType
    {
        Right = 0,
        Left = 1,

        None = -1,
    }

    DirType _dirType = DirType.None;
    GameObject _player = null;

    StateMachine.StateType _stateType;
    
    public override void Entry(StateMachine.StateType beforeType)
    {
        if (_player == null) _player = GameObject.FindWithTag("Player");
        int setval = UnityEngine.Random.Range(0, 2);
        _dirType = (DirType)Enum.ToObject(typeof(DirType), setval);

        _stateType = beforeType;
        Debug.Log($"Before {beforeType}");
    }

    public override void Run(out Vector3 move)
    {
        Vector3 mPos = Target.transform.position;
        float dist = Vector3.Distance(_player.transform.position, mPos);

        if (_towardDist < dist)
        {
            move = (_player.transform.position - mPos).normalized / _moveRate;
        }
        else
        {
            float moveRate = _moveRate * _sideMoveRate;
            if (_dirType == DirType.Right) move = Target.transform.right / moveRate;
            else move = (Target.transform.right * -1) / moveRate;
        }

        move.y = 1;
    }

    public override StateMachine.StateType Exit()
    {
        Vector3 mPos = Target.transform.position;
        float dist = Vector3.Distance(_player.transform.position, mPos);
        if (_stateType == StateMachine.StateType.Attack)
        {
            return StateMachine.StateType.Avoid;
        }

        if (_changeStateDist > dist)
        {
            return StateMachine.StateType.Attack;
        }
        else return StateMachine.StateType.Move;
    }
}

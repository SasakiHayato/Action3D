using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAvoid : StateMachine.State
{
    [SerializeField] float _avoidTime;
    [SerializeField] float _speed;

    Vector3 _dir;
    float _timer;

    public override void Entry(StateMachine.StateType beforeType)
    {
        _dir = Target.transform.forward;
        _timer = 0;
    }

    public override void Run(out Vector3 move)
    {
        _timer += Time.deltaTime;
        move = (_dir * -1) * _speed;
        move.y = 1;
    }

    public override StateMachine.StateType Exit()
    {
        if (_timer > _avoidTime)
        {
            return StateMachine.StateType.Move;
        }
        else
        {
            return StateMachine.StateType.Avoid;
        }
    }
}

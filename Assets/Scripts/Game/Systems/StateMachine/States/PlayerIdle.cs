using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : StateMachine.State
{
    Vector2 _input;

    public override void Entry(StateMachine.StateType beforeType)
    {
        _input = Vector2.zero;
    }

    public override void Run(out Vector3 move)
    {
        _input = (Vector2)Inputter.GetValue(InputType.PlayerMove);
        move = new Vector3(0, 1, 0);
    }

    public override StateMachine.StateType Exit()
    {
        if (_input != Vector2.zero) return StateMachine.StateType.Move;
        else return StateMachine.StateType.Idle;

    }
}
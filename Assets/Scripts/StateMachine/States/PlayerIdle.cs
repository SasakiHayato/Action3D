using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : StateMachine.State
{
    Vector2 _input = Vector2.zero;

    public override void Entry()
    {
        Debug.Log("EntryIdle");
    }

    public override void Run(out Vector3 move)
    {
        _input = (Vector2)Inputter.GetValue(InputType.PlayerMove);
        Debug.Log(_input);
        Debug.Log("IdleUpdate");
        move = new Vector3(0, 1, 0);
    }

    public override StateMachine.StateType Exit()
    {
        if (_input != Vector2.zero)
        {
            return StateMachine.StateType.Move;
        }
        else
        {
            return StateMachine.StateType.Idle;
        }
    }
}

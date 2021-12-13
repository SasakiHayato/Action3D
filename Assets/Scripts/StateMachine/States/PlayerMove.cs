using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : StateMachine.State
{
    GameObject _mainCm;
    Vector2 _input = Vector2.zero;

    public override void Entry()
    {
        Debug.Log("EntryMove");
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public override void Run(out Vector3 move)
    {
        _input = (Vector2)Inputter.GetValue(InputType.PlayerMove);
        Debug.Log("RunMove");
        Vector3 forward = _mainCm.transform.forward * _input.y;
        Vector3 right = _mainCm.transform.right * _input.x;
        move = new Vector3(forward.x + right.x, 1, right.z + forward.z);
    }

    public override StateMachine.StateType Exit()
    {
        if (_input == Vector2.zero)
        {
            return StateMachine.StateType.Idle;
        }
        else
        {
            return StateMachine.StateType.Move;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : StateMachine.State
{
    [SerializeField] float _dashSpeedRate;

    GameObject _mainCm;
    Vector2 _input = Vector2.zero;

    float _setSpeedRate = 1;

    public override void Entry(StateMachine.StateType beforeType)
    {
        Debug.Log("EntryMove");
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");

        if (beforeType == StateMachine.StateType.Avoid) 
            _setSpeedRate = _dashSpeedRate;
        else 
            _setSpeedRate = 1;
    }

    public override void Run(out Vector3 move)
    {
        _input = (Vector2)Inputter.GetValue(InputType.PlayerMove) * _setSpeedRate;

        Vector3 forward = _mainCm.transform.forward * _input.y;
        Vector3 right = _mainCm.transform.right * _input.x;
        move = new Vector3(forward.x + right.x, 1, right.z + forward.z);
    }

    public override StateMachine.StateType Exit()
    {
        if (_input == Vector2.zero)
            return StateMachine.StateType.Idle;
        else 
            return StateMachine.StateType.Move;
    }
}

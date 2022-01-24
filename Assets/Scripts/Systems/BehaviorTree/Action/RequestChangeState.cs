using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorAI;

public class RequestChangeState : IAction
{
    [SerializeField] StateMachine.StateType _stateType;
    StateMachine _state = null;

    bool _check = false;

    public GameObject Target { get; set; }
    public bool Reset { set { _check = value; } }

    public void Execute()
    {
        if (_state == null) _state = Target.GetComponent<StateMachine>();
        _state.ChangeState(_stateType);
        _check = true;
    }

    public bool End() => _check;
}

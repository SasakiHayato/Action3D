using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAI;

public class RequestChangeState : IAction
{
    [SerializeField] StateMachine.StateType _stateType;
    StateMachine _state = null;

    public GameObject Target { get; set; }
   
    public void SetUp()
    {
        if (_state == null) _state = Target.GetComponent<StateMachine>();
    }

    public bool Execute()
    {
        _state.ChangeState(_stateType);
        return true;
    }
}

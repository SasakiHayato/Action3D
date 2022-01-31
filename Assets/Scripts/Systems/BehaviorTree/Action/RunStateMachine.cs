using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAI;

public class RunStateMachine : IAction
{
    [SerializeField] StateMachine _state;
    [SerializeField] bool _isRun;
    bool _check = false;
    
    public GameObject Target { get; set; }

    public void SetUp()
    { }

    public bool Execute()
    {
        _state.RunRequest(_isRun);
        _state.Base();

        return true;
    }
}

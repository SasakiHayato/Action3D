using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorAI;

public class RunStateMachine : IAction
{
    [SerializeField] StateMachine _state;
    bool _check = false;
    
    public GameObject Target { private get; set; }
    public void Execute()
    {
        _state.RunRequest(true);
        _state.Base();

        _check = true;
    }

    public bool End() => _check;
    public bool Reset { set { _check = value; } }
}

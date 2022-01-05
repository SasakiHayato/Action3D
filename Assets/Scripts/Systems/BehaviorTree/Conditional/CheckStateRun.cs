using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorAI;

public class CheckStateRun : IConditional
{
    bool _check = false;
    StateMachine _state = null;
    public GameObject Target { private get; set; }

    public bool Check()
    {
        if (_state == null) _state = Target.GetComponent<StateMachine>();

        if (_state.IsRunning) _check = false;
        else _check = true;

        return _check;
    }
}

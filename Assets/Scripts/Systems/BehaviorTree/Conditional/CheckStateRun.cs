using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorAI;

public class CheckStateRun : IConditional
{
    [SerializeField] bool _checkBool;
    
    StateMachine _state = null;
    public GameObject Target { private get; set; }

    public bool Check()
    {
        if (_state == null) _state = Target.GetComponent<StateMachine>();

        if (_checkBool)
        {
            if (_state.IsRunning) return true;
            else return false;
        }
        else
        {
            if (_state.IsRunning) return false;
            else return true;
        }
        
    }
}

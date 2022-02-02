using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAI;

public class CheckStateRun : IConditional
{
    [SerializeField] bool _checkBool;
    [SerializeField] StateMachine _state;
    
    public GameObject Target { get; set; }

    public bool Check()
    {
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

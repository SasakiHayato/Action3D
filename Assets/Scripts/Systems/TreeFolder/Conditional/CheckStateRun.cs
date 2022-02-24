using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckStateRun : IConditional
{
    [SerializeField] bool _checkBool;
    [SerializeField] StateMachine _state;
    
    public void SetUp(GameObject user)
    {

    }

    public bool Try()
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

    public void InitParam()
    {

    }
}

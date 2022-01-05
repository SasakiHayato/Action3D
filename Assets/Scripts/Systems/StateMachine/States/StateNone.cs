using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNone : StateMachine.State
{
    [SerializeField] StateMachine.StateType _nextState;
    public override void Entry(StateMachine.StateType beforeType) => Debug.Log("EntryNone");
    public override void Run(out Vector3 move) => move = Vector3.zero;
    public override StateMachine.StateType Exit() => _nextState;
}

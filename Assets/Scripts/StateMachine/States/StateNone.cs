using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNone : StateMachine.State
{
    public override void Entry() => Debug.Log("EntryNone");
    public override void Run(out Vector3 move) => move = Vector3.zero;
    public override StateMachine.StateType Exit() => StateMachine.StateType.Idle;
}

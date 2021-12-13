using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvoid : StateMachine.State
{
    public override void Entry()
    {
        Debug.Log("EntryAvoid");
    }

    public override void Run(out Vector3 move)
    {
        Debug.Log("RunAvoid");
        move = Vector3.zero;
    }

    public override StateMachine.StateType Exit()
    {
        return StateMachine.StateType.None;
    }
}

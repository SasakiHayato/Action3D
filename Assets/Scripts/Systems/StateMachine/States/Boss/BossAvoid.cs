using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAvoid : StateMachine.State
{
    public override void Entry(StateMachine.StateType beforeType)
    {
        Debug.Log("EntryAvoid");
    }

    public override void Run(out Vector3 move)
    {
        move = Vector3.zero;
    }

    public override StateMachine.StateType Exit()
    {
        return StateMachine.StateType.Avoid;
    }
}

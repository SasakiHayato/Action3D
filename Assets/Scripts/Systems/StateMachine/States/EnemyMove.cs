using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : StateMachine.State
{
    public override void Entry(StateMachine.StateType beforeType)
    {

    }

    public override void Run(out Vector3 move)
    {
        
        move = Vector3.right * -1;
    }

    public override StateMachine.StateType Exit()
    {
        return StateMachine.StateType.Move;
    }
}

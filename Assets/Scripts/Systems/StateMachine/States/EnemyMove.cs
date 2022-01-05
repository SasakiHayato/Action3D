using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorAI;

public class EnemyMove : StateMachine.State
{
    BehaviorTree _tree = null;
    IBehavior _behavior;

    public override void Entry(StateMachine.StateType beforeType)
    {
        if (_tree == null)
        {
            _tree = Target.GetComponent<BehaviorTree>();
            _behavior = Target.GetComponent<IBehavior>();
        }
    }

    public override void Run(out Vector3 move)
    {
        _tree.Repeater(_behavior);
        move = Vector3.zero;
    }

    public override StateMachine.StateType Exit()
    {
        return StateMachine.StateType.Move;
    }
}

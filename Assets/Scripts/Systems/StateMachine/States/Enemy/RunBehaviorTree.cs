using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class RunBehaviorTree : StateMachine.State
{
    TreeManager _tree;

    public override void Entry(StateMachine.StateType beforeType)
    {
        _tree = Target.GetComponent<TreeManager>();
        _tree.SetUp();
    }

    public override void Run(out Vector3 move)
    {
        move = Vector3.zero;
        _tree.Run();
    }

    public override StateMachine.StateType Exit()
    {
        return StateMachine.StateType.BehaviorTree;
    }
}

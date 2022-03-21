using UnityEngine;
using BehaviourTree;
using StateMachine;
using System;

public class RunBehaviorTree : State
{
    TreeManager _tree;

    public override void SetUp(GameObject user)
    {
        _tree = user.GetComponent<TreeManager>();
        _tree.SetUp();
    }

    public override void Entry(Enum beforeType)
    {
        _tree.InitParam();
    }

    public override void Run()
    {
        _tree.Update();
    }

    public override Enum Exit()
    {
        return EnemyBase.State.BehaviorTree;
    }
}

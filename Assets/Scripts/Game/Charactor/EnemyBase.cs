using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorAI;

[RequireComponent(typeof(BehaviorTree))]
public class EnemyBase : CharaBase, IBehavior
{
    [SerializeField] BehaviorTree _tree;
    protected BehaviorTree Tree { get => _tree; }

    public GameObject SetTarget() => gameObject;
    public void Call(IAction action) => action.Execute();
}
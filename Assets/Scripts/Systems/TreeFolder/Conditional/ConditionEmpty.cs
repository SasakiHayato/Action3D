using UnityEngine;
using BehaviourTree;

public class ConditionEmpty : IConditional
{
    public void SetUp(GameObject user) { }
    public bool Try() => true;
    public void InitParam()
    { 
    }
}

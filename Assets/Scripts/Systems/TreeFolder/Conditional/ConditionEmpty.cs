using UnityEngine;
using BehaviourTree;

/// <summary>
/// �K���s���������������
/// </summary>

public class ConditionEmpty : IConditional
{
    public void SetUp(GameObject user) { }
    public bool Try() => true;
    public void InitParam()
    { 
    }
}

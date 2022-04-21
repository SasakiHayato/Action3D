using UnityEngine;
using BehaviourTree;

/// <summary>
/// •K‚¸s“®‚³‚¹‚éğŒ•ªŠò
/// </summary>

public class ConditionEmpty : IConditional
{
    public void SetUp(GameObject user) { }
    public bool Try() => true;
    public void InitParam()
    { 
    }
}

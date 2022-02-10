using UnityEngine;
using BehaviourAI;

public class ConditionEmpty : IConditional
{
    public GameObject Target { get; set; }
    public bool Check() => true;
}

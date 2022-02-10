using UnityEngine;
using BehaviourAI;

public class LimitConditional : IConditional
{
    bool _setBool = false;

    public GameObject Target { get; set; }
    public bool Check()
    {
        Debug.Log("LimitCondition ********************");
        if (!_setBool)
        {
            _setBool = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}

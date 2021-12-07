using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoid : StateManage.State
{
    public override void Entry()
    {
        Debug.Log("Avoid");
    }

    public override void Update()
    {
        
    }

    public override StateManage.StateType Exit()
    {
        return StateManage.StateType.None;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : StateManage.State
{
    public override void Entry()
    {
        Debug.Log("Idle");
    }

    public override void Update()
    {
        Debug.Log("IdleUpdate");
    }

    public override StateManage.StateType Exit()
    {
        if (GetInput != Vector2.zero)
        {
            return StateManage.StateType.Move;
        }
        else
        {
            return StateManage.StateType.Idle;
        }
    }
}

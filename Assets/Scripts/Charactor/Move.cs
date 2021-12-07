using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : StateManage.State
{
    GameObject _mainCm;
    
    public override void Entry()
    {
        Debug.Log("Move");
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public override void Update()
    {
        Debug.Log("MoveUpdate");
        Vector3 forward = _mainCm.transform.forward * GetInput.y;
        Vector3 right = _mainCm.transform.right * GetInput.x;
        Vector3 set = new Vector3(forward.x + right.x, 0, right.z + forward.z);
        CC.Move(set * Time.deltaTime);
    }

    public override StateManage.StateType Exit()
    {
        if (GetInput == Vector2.zero)
        {
            return StateManage.StateType.Idle;
        }
        else
        {
            return StateManage.StateType.Move;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttackSetting;

public class PlayerAttack : StateMachine.State
{
    [SerializeField] AttackSettings _attack;
    Vector2 _input = Vector2.zero;

    public override void Entry(StateMachine.StateType beforeType)
    {
        _attack.Request(_attack.ReadAction);
        _input = (Vector2)Inputter.GetValue(InputType.PlayerMove);
    }

    public override void Run(out Vector3 move)
    {
        move = Vector3.zero;
    }

    public override StateMachine.StateType Exit()
    {
        if (_input == Vector2.zero)
        {
            return StateMachine.StateType.None;
        }
        else
        {
            return StateMachine.StateType.Move;
        }
    }
}

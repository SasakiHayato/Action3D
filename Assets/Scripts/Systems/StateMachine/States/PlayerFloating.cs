using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPhysics;

public class PlayerFloating : StateMachine.State
{
    Player _player = null;
    PhysicsBase _physics;

    public override void Entry(StateMachine.StateType beforeType)
    {
        if (_player == null)
        {
            _player = Target.GetComponent<Player>();
            _physics = Target.GetComponent<PhysicsBase>();
        }
        _player.SetAnim("");
    }

    public override void Run(out Vector3 move)
    {

        move = Vector3.zero;
    }

    public override StateMachine.StateType Exit()
    {
        return StateMachine.StateType.Floating;
    }
}

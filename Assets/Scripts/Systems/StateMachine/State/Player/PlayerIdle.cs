using UnityEngine;
using StateMachine;
using System;

public class PlayerIdle : State
{
    Vector2 _input;
    PhysicsBase _physics = null;

    Player _player;
    Animator _anim;

    public override void SetUp(GameObject user)
    {
        _physics = user.GetComponent<PhysicsBase>();
        _player = user.GetComponent<Player>();
        _anim = user.GetComponent<Animator>();
    }

    public override void Entry(Enum beforeType)
    {
        //if (beforeType.ToString() == Player.State.Attack.ToString())
        //{
        //    _physics.Gravity.ResetTimer();
        //}

        _input = Vector2.zero;
        _anim.CrossFade("Idle", 0.1f);
    }

    public override void Run()
    {
        _input = (Vector2)Inputter.GetValue(InputType.PlayerMove);
        _player.Move = new Vector3(0, 1, 0);
    }

    public override Enum Exit()
    {
        if (_input != Vector2.zero) return Player.State.Move;
        else return Player.State.Idle;
    }
}

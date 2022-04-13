using UnityEngine;
using StateMachine;
using System;

/// <summary>
/// âΩÇ‡ÇµÇƒÇ¢Ç»Ç¢ç€ÇÃêßå‰ÉNÉâÉX
/// </summary>

public class PlayerIdle : State
{
    Vector2 _input;
    Player _player;
   
    public override void SetUp(GameObject user)
    {
        _player = user.GetComponent<Player>();
    }

    public override void Entry(Enum beforeType)
    {
        _input = Vector2.zero;
        string animName = "Idle";
        if (beforeType.ToString() == Player.State.Float.ToString())
        {
            animName = "Jump_End";
        }
       
        _player.AnimController.RequestAnim(animName);
    }

    public override void Run()
    {
        _input = (Vector2)Inputter.Instance.GetValue(InputType.PlayerMove);
        _player.Move = new Vector3(0, 1, 0);
    }

    public override Enum Exit()
    {
        if (_input != Vector2.zero) return Player.State.Move;
        else return Player.State.Idle;
    }
}

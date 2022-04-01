using UnityEngine;
using StateMachine;
using System;

public class PlayerFloating : State
{
    [SerializeField] float _nextStateSpan;
    
    Player _player = null;
    
    GameObject _mainCm;
    Vector2 _input = Vector2.zero;

    float _setSpeedRate = 1;
    Vector3 _beforePos;

    GameObject _user;

    float _timer;

    public override void SetUp(GameObject user)
    {
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");
        _player = user.GetComponent<Player>();
        
        _user = user;
    }

    public override void Entry(Enum beforeType)
    {
        string animName = "Double_Jump_Start";
        if (_player.PhsicsBase.GetJumpData.CurrntCount == 0) animName = "Jump_Start";

        _player.AnimController.RequestAnimCallBackEvent(animName, null);

        _timer = 0;
    }

    public override void Run()
    {
        _timer += Time.deltaTime;

        _input = (Vector2)Inputter.Instance.GetValue(InputType.PlayerMove) * _setSpeedRate;

        Vector3 forward = _mainCm.transform.forward * _input.y;
        Vector3 right = _mainCm.transform.right * _input.x;
        _player.Move = new Vector3(forward.x + right.x, 1, right.z + forward.z);

        Rotate();
        if (_player.AnimController.EndAnim)
        {
            _player.AnimController.RequestAnim("Jump_Loop");
        }
    }

    void Rotate()
    {
        Vector3 forward = _user.transform.position - _beforePos;
        _beforePos = _user.transform.position;
        forward.y = 0;
        if (forward.magnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.LookRotation(forward);
            _user.transform.rotation =
                Quaternion.Lerp(_user.transform.rotation, rotation, Time.deltaTime * 8);
        }
    }

    public override Enum Exit()
    {
        if (_player.PhsicsBase.IsGround && _timer > _nextStateSpan) return Player.State.Idle;
        else return Player.State.Float;
    }
}

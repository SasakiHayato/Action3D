using UnityEngine;
using StateMachine;
using System;

public class PlayerMove : State
{
    [SerializeField] float _dashSpeedRate;
    [SerializeField, Range(0, 1)] float _setRunAnimRate;

    Animator _anim = null;
    GameObject _mainCm;
    Vector2 _input = Vector2.zero;

    float _setSpeedRate = 1;
    Vector3 _beforePos;

    GameObject _user;
    Player _player;

    public override void SetUp(GameObject user)
    {
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");
        _anim = user.GetComponent<Animator>();
        _player = user.GetComponent<Player>();

        _user = user;
    }

    public override void Entry(Enum beforeType)
    {
        if (beforeType.ToString() == Player.State.Avoid.ToString())
        {
            _anim.CrossFade("Run", 0.2f);
            _setSpeedRate = _dashSpeedRate;
        }
        else
        {
            _anim.CrossFade("RunNoamal", 0.1f);
            _setSpeedRate = 1;
        }
        if (beforeType.ToString() == Player.State.Float.ToString())
        {
            Debug.Log("aaa");
        }
    }

    public override void Run()
    {
        _input = (Vector2)Inputter.GetValue(InputType.PlayerMove) * _setSpeedRate;

        Vector3 forward = _mainCm.transform.forward * _input.y;
        Vector3 right = _mainCm.transform.right * _input.x;
        _player.Move = new Vector3(forward.x + right.x, 1, right.z + forward.z);

        Rotate();
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
        if (_input == Vector2.zero)
            return Player.State.Idle;
        else 
            return Player.State.Move;
    }
}

using UnityEngine;
using StateMachine;
using System;

public class PlayerFloating : State
{
    [SerializeField] float _dashSpeedRate;
    
    Player _player = null;
    Animator _anim;
    CharacterController _character;

    GameObject _mainCm;
    Vector2 _input = Vector2.zero;

    float _setSpeedRate = 1;
    Vector3 _beforePos;

    bool _isGround = false;

    GameObject _user;

    public override void SetUp(GameObject user)
    {
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");
        _player = user.GetComponent<Player>();
        _anim = user.GetComponent<Animator>();
        _character = user.GetComponent<CharacterController>();

        _user = user;
    }

    public override void Entry(Enum beforeType)
    {
        _isGround = false;
        _player.SetAnim("Jump_Start");
    }

    public override void Run()
    {
        _input = (Vector2)Inputter.GetValue(InputType.PlayerMove) * _setSpeedRate;

        Vector3 forward = _mainCm.transform.forward * _input.y;
        Vector3 right = _mainCm.transform.right * _input.x;
        _player.Move = new Vector3(forward.x + right.x, 1, right.z + forward.z);

        Rotate();
        if (_player.EndAnim)
        {
            _anim.Play("Jump_Loop");
            if (_character.isGrounded) _isGround = true;
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
        if (_isGround) return Player.State.Idle;
        else return Player.State.Float;
    }
}

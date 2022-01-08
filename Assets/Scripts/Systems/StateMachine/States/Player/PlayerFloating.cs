using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPhysics;

public class PlayerFloating : StateMachine.State
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

    public override void Entry(StateMachine.StateType beforeType)
    {
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");
        if (_player == null)
        {
            _player = Target.GetComponent<Player>();
            _anim = Target.GetComponent<Animator>();
            _character = Target.GetComponent<CharacterController>();
        }
        _isGround = false;
        _player.SetAnim("Jump_Start");
    }

    public override void Run(out Vector3 move)
    {
        _input = (Vector2)Inputter.GetValue(InputType.PlayerMove) * _setSpeedRate;

        Vector3 forward = _mainCm.transform.forward * _input.y;
        Vector3 right = _mainCm.transform.right * _input.x;
        move = new Vector3(forward.x + right.x, 1, right.z + forward.z);

        Rotate();
        if (_player.EndAnim)
        {
            _anim.Play("Jump_Loop");
            if (_character.isGrounded) _isGround = true;
        }
    }

    void Rotate()
    {
        Vector3 forward = Target.transform.position - _beforePos;
        _beforePos = Target.transform.position;
        forward.y = 0;
        if (forward.magnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.LookRotation(forward);
            Target.transform.rotation =
                Quaternion.Lerp(Target.transform.rotation, rotation, Time.deltaTime * 8);
        }
    }

    public override StateMachine.StateType Exit()
    {
        if (_isGround) return StateMachine.StateType.Idle;
        else return StateMachine.StateType.Floating;
    }
}

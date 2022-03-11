using UnityEngine;
using System;
using Sounds;
using StateMachine;

public class PlayerAvoid : State
{
    [SerializeField] float _avoidTime;
    [SerializeField] float _defSpeed;
    [SerializeField] float _isLockOnSpeed;
    [SerializeField] float _targetDist;

    GameObject _mainCm;
    Vector2 _input = Vector2.zero;
    
    float _currentTime;
    float _setSpeed;

    Player _player;
    GameObject _user;

    public override void SetUp(GameObject user)
    {
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");
        _player = user.GetComponent<Player>();

        _user = user;
    }

    public override void Entry(Enum beforeType)
    {
        _currentTime = 0;
        
        _input = (Vector2)Inputter.GetValue(InputType.PlayerMove);
        _input = _input.normalized;
        SoundMaster.PlayRequest(_user.transform, "Avoid", 0);
        GameObject t = GameManager.Instance.LockonTarget;
        if (t != null)
        {
            float dist = Vector3.Distance(t.transform.position, _user.transform.position);
            if (dist < _targetDist) _setSpeed = _isLockOnSpeed;
            else _setSpeed = _defSpeed;
        }
        else
        {
            _setSpeed = _defSpeed;
        }

        GetComponent<Animator>().Play("Dodge");
    }

    public override void Run()
    {
        if (_input == Vector2.zero) _input = Vector2.up * -1;
        _currentTime += Time.deltaTime;
        
        Vector3 forward = _mainCm.transform.forward * _input.y * _setSpeed;
        Vector3 right = _mainCm.transform.right * _input.x * _setSpeed;
        _player.Move = new Vector3(forward.x + right.x, 1, right.z + forward.z);
    }

    public override Enum Exit()
    {
        if (_currentTime > _avoidTime)
        {
            return Player.State.Move;
        }
        else
        {
            return Player.State.Avoid;
        }
    }
}

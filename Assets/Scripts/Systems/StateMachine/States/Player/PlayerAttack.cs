using UnityEngine;
using NewAttacks;
using ObjectPhysics;
using DG.Tweening;
using StateMachine;
using System;

public class PlayerAttack : State
{
    [SerializeField] float _moveTime;
    [SerializeField] float _moveSpeed = 1;

    float _timer;

    Player _player;
    PhysicsBase _physics;
    NewAttackSettings _settings;

    GameObject _user;

    public override void SetUp(GameObject user)
    {
        _settings = user.GetComponent<NewAttackSettings>();
        _player = user.GetComponent<Player>();
        _physics = user.GetComponent<PhysicsBase>();

        _user = user;
    }

    public override void Entry(Enum beforeType)
    {
        _timer = 0;

        if (beforeType.ToString() == Player.State.Avoid.ToString())
        {
            if(_player.IsAvoid)
            {
                if (GameManager.Instance.IsLockOn)
                {
                    Transform t = GameManager.Instance.LockonTarget.transform;
                    Vector3 tPos = t.position;
                    Vector3 tForward = t.forward;
                    tPos.y -= 0.5f;
                    _user.transform.DOMove(tPos - tForward, 0.1f).SetEase(Ease.Linear);
                    _settings.Request(NewAttacks.AttackType.Counter, 0);
                }
                else
                {
                    _settings.Request(NewAttacks.AttackType.Counter, 1);
                }
            }
        }
        else
        {
            if (_settings.ReadAttackType != NewAttacks.AttackType.Counter)
            {
                if (beforeType.ToString() == Player.State.Float.ToString() || !_physics.IsGround)
                {
                    _settings.SetAttackType = NewAttacks.AttackType.Float;
                    _settings.Request(NewAttacks.AttackType.Float);
                }
                else
                    _settings.Request(_settings.ReadAttackType);
            }
        }
    }

    public override void Run()
    {
        _timer += Time.deltaTime;
        
        if (_timer > _moveTime) _player.Move = Vector3.zero;
        else _player.Move = _user.transform.forward * _moveSpeed;

        if (GameManager.Instance.IsLockOn) Rotate();
    }

    void Rotate()
    {
        Vector3 t = GameManager.Instance.LockonTarget.transform.position;
        Vector3 forward = t - _user.transform.position;

        forward.y = 0;
        if (forward.magnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.LookRotation(forward);
            _user.transform.rotation = rotation;
        }
    }

    public override Enum Exit()
    {
        if (!_settings.EndCurrentAnim)
        {
            if (_settings.IsNextRequest && _settings.IsSetNextRequest)
            {
                return _player.BaseState.SetEntry(Player.State.Attack);
            }
            
            return Player.State.Attack;
        }
        else
        {
            return Player.State.Idle;
        }
    }
}

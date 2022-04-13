using UnityEngine;
using NewAttacks;
using DG.Tweening;
using StateMachine;
using System;
using System.Linq;

/// <summary>
/// çUåÇÇ∑ÇÈç€ÇÃêßå‰ÉNÉâÉX
/// </summary>

public class PlayerAttack : State
{
    [SerializeField] float _moveTime;
    [SerializeField] float _moveSpeed = 1;
    [SerializeField] float _lockOnDist;

    float _timer;

    Player _player;
    PhysicsBase _physics;
    AttackSettings _settings;

    GameObject _user;
    Transform _target;

    public override void SetUp(GameObject user)
    {
        _settings = user.GetComponent<AttackSettings>();
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

        _target = SetTarget();
    }

    public override void Run()
    {
        _physics.Force(PhysicsBase.ForceType.None);
        _timer += Time.deltaTime;
        
        if (_timer > _moveTime) _player.Move = Vector3.zero;
        else _player.Move = _user.transform.forward * _moveSpeed;

        if (GameManager.Instance.IsLockOn) Rotate(GameManager.Instance.LockonTarget.transform);
        else
        {
            Vector2 input = (Vector2)Inputter.Instance.GetValue(InputType.PlayerMove);
            if (input == Vector2.zero && _target != null) Rotate(_target);
            else
            {
                Vector3 forward = Camera.main.transform.forward * input.y;
                Vector3 right = Camera.main.transform.right * input.x;
                
                Vector3 set = new Vector3(forward.x + right.x, 0, right.z + forward.z);

                if (forward.magnitude > 0.01f)
                {
                    Quaternion rotation = Quaternion.LookRotation(set);
                    _user.transform.rotation = rotation;
                }
            }
        }
    }

    Transform SetTarget()
    {
        var finds = GameObject.FindGameObjectsWithTag("Enemy")
                .Where(e =>
                {
                    float dist = Vector3.Distance(e.transform.position,_user.transform.position);
                    if (dist < _lockOnDist) return e;
                    else return false;
                });

        if (finds.Count() <= 0) return null;

        Vector3 cmForwrad = Camera.main.transform.forward;
        GameObject set = null;
        float saveDist = float.MaxValue;

        foreach (var e in finds)
        {
            float dist = Vector3.Distance(_user.transform.position, e.transform.position);

            if (saveDist > dist)
            {
                CharaBase chara = e.GetComponent<CharaBase>();
                if (chara != null)
                {
                    set = e.GetComponent<CharaBase>().OffSetPosObj;
                    saveDist = dist;
                }
            }
        }

        return set.transform;
    }

    void Rotate(Transform target)
    {
        if (target == null) return;

        Vector3 t = target.position;
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
                return _player.BaseState.ReturnEntry();

            return Player.State.Attack;
        }
        else
        {
            if (_player.PhsicsBase.IsGround) return Player.State.Idle;
            else return Player.State.Float;
        }
    }
}

using UnityEngine;
using NewAttacks;
using ObjectPhysics;
using DG.Tweening;

public class PlayerAttack : StateMachine.State
{
    [SerializeField] float _moveTime;
    [SerializeField] float _moveSpeed = 1;

    float _timer;

    Player _player;
    PhysicsBase _physics;
    NewAttackSettings _settings;
    
    public override void Entry(StateMachine.StateType beforeType)
    {
        if (_player == null)
        {
            _settings = Target.GetComponent<NewAttackSettings>();
            _player = Target.GetComponent<Player>();
            _physics = Target.GetComponent<PhysicsBase>();
        }
        
        _timer = 0;
        if (beforeType == StateMachine.StateType.Avoid)
        {
            if(_player.IsAvoid)
            {
                if (GameManager.Instance.IsLockOn)
                {
                    Transform t = GameManager.Instance.LockonTarget.transform;
                    Vector3 tPos = t.position;
                    Vector3 tForward = t.forward;
                    tPos.y -= 0.5f;
                    Target.transform.DOMove(tPos - tForward, 0.1f).SetEase(Ease.Linear);
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
                if (beforeType == StateMachine.StateType.Floating || !_physics.IsGround)
                {
                    _settings.SetAttackType = NewAttacks.AttackType.Float;
                    _settings.Request(NewAttacks.AttackType.Float);
                }
                else
                    _settings.Request(_settings.ReadAttackType);
            }
        }
    }

    public override void Run(out Vector3 move)
    {
        _timer += Time.deltaTime;
        
        if (_timer > _moveTime) move = Vector3.zero;
        else move = Target.transform.forward * _moveSpeed;

        if (GameManager.Instance.IsLockOn) Rotate();
    }

    void Rotate()
    {
        Vector3 t = GameManager.Instance.LockonTarget.transform.position;
        Vector3 forward = t - Target.transform.position;

        forward.y = 0;
        if (forward.magnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.LookRotation(forward);
            Target.transform.rotation = rotation;
        }
    }

    public override StateMachine.StateType Exit()
    {
        if (!_settings.EndCurrentAnim)
        {
            if (_settings.IsNextRequest && _settings.IsSetNextRequest)
            {
                return Target.GetComponent<StateMachine>().RetuneState(StateMachine.StateType.Attack);
            }

            return StateMachine.StateType.Attack;
        }
        else
        {
            return StateMachine.StateType.Idle;
        }
    }
}

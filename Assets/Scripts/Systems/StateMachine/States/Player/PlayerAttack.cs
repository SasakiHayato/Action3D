using UnityEngine;
using AttackSetting;
using ObjectPhysics;
using DG.Tweening;

public class PlayerAttack : StateMachine.State
{
    [SerializeField] float _moveTime;
    [SerializeField] float _moveSpeed = 1;

    float _timer;

    Player _player;
    AttackSettings _attack = null;
    PhysicsBase _physics;
    
    public override void Entry(StateMachine.StateType beforeType)
    {
        if (_attack == null)
        {
            _attack = Target.GetComponent<AttackSettings>();
            _player = Target.GetComponent<Player>();
            _physics = Target.GetComponent<PhysicsBase>();
        }
        
        _timer = 0;
        if (beforeType == StateMachine.StateType.Avoid)
        {
            if(GameManager.Instance.IsLockOn && _player.IsAvoid)
            {
                Transform t = GameManager.Instance.LockonTarget.transform;
                Vector3 tPos = t.position;
                Vector3 tForward = t.forward;
                tPos.y -= 0.5f;
                Target.transform.DOMove(tPos - tForward * 1.2f, 0.1f).SetEase(Ease.Linear);
                _attack.Request(ActionType.Counter);
            }
        }
        else
        {
            if (beforeType == StateMachine.StateType.Floating || !_physics.IsGround)
                _attack.Request(ActionType.Float);
            else
                _attack.Request(_attack.ReadAction);
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
        if (!_attack.EndCurrentAnim)
        {
            return StateMachine.StateType.Attack;
        }
        else
        {
            if (_attack.IsNextRequest)
            {
                return Target.GetComponent<StateMachine>().RetuneState(StateMachine.StateType.Attack);
            }
            else
            {
                return StateMachine.StateType.Idle;
            }
        }
    }
}

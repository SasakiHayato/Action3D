using UnityEngine;
using AttackSetting;
using ObjectPhysics;
using DG.Tweening;
using System.Linq;

public class PlayerAttack : StateMachine.State
{
    [SerializeField] float _moveTime;
    [SerializeField] float _moveSpeed = 1;
    [SerializeField] float _lockOnDist;

    float _timer;

    Player _player;
    AttackSettings _attack = null;
    PhysicsBase _physics;

    Transform _setTarget;
    
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
            if(_player.IsAvoid)
            {
                if (GameManager.Instance.IsLockOn)
                {
                    Transform t = GameManager.Instance.LockonTarget.transform;
                    Vector3 tPos = t.position;
                    Vector3 tForward = t.forward;
                    tPos.y -= 0.5f;
                    Target.transform.DOMove(tPos - tForward, 0.1f).SetEase(Ease.Linear);
                    _attack.RequestAt(ActionType.Counter, 0);
                }
                else
                {
                    _attack.RequestAt(ActionType.Counter, 1);
                }
            }
        }
        else
        {
            if (beforeType == StateMachine.StateType.Floating || !_physics.IsGround)
                _attack.Request(ActionType.Float);
            else
                _attack.Request(_attack.ReadAction);
        }

        _setTarget = SetTarget();
    }

    public override void Run(out Vector3 move)
    {
        _timer += Time.deltaTime;
        
        if (_timer > _moveTime) move = Vector3.zero;
        else move = Target.transform.forward * _moveSpeed;

        if (GameManager.Instance.IsLockOn) Rotate(GameManager.Instance.LockonTarget.transform.position);
        else
        {
            Vector2 get = (Vector2)Inputter.GetValue(InputType.PlayerMove);
            if (get == Vector2.zero && _setTarget != null) Rotate(_setTarget.position);
            else
            {
                Vector3 forward = Camera.main.transform.forward * get.y;
                Vector3 right = Camera.main.transform.right * get.x;
                Vector3 set = new Vector3(forward.x + right.x, 0, right.z + forward.z);
                
                if (forward.magnitude > 0.01f)
                {
                    Quaternion rotation = Quaternion.LookRotation(set);
                    Target.transform.rotation = rotation;
                }
            }
        }
    }

    Transform SetTarget()
    {
        var finds = GameObject.FindGameObjectsWithTag("Enemy")
                .Where(e =>
                {
                    float dist = Vector3.Distance(e.transform.position, Target.transform.position);
                    if (dist < _lockOnDist) return e;
                    else return false;
                });

        if (finds.Count() <= 0) return null;

        Vector3 cmForwrad = Camera.main.transform.forward;
        GameObject set = null;
        float saveDist = float.MaxValue;

        foreach (var e in finds)
        {
            float dist = Vector3.Distance(Target.transform.position, e.transform.position);
           
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

    void Rotate(Vector3 target)
    {
        Vector3 forward = target - Target.transform.position;

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

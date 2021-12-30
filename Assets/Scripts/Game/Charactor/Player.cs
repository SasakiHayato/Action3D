using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using AttackSetting;

public class Player : CharaBase, IDamage
{
    [SerializeField] int _hp;
    [SerializeField] float _masterSpeed;
    [SerializeField] float _lockOnDist;
    StateMachine _state;

    bool _isAvoid = false;
    public bool IsAvoid => _isAvoid;

    bool _isLockon = false;
    public bool IsLockon => _isLockon;

    public int GetHP => _hp;
    public Vector3 GetKnockDir { get; private set; }
    
    void Start()
    {
        _state = GetComponent<StateMachine>();

        Inputter.Instance.Inputs.Player.Fire.started += context
        => _state.ChangeState(StateMachine.StateType.Avoid);

        Inputter.Instance.Inputs.Player
            .Jump.started += context => Jump();

        Inputter.Instance.Inputs.Player
            .WeakAttack.started += context => WeakAttack();

        Inputter.Instance.Inputs.Player
            .StrengthAttack.started += context => StrengthAttack();

        Inputter.Instance.Inputs.Player
            .RockOn.started += context => SetLockon();
    }

    void Update()
    {
        _state.Base();
        if (_state.GetCurrentState != StateMachine.StateType.Avoid) _isAvoid = false;

        Vector3 set = Vector3.Scale(_state.Move * _masterSpeed, PhsicsBase.GetVelocity);
        Character.Move(set * Time.deltaTime);
    }

    void Jump()
    {
        _state.ChangeState(StateMachine.StateType.Floating);
        PhsicsBase.SetJump();
    }

    void WeakAttack()
    {
        GetComponent<AttackSettings>().SetAction = ActionType.WeakGround;
        _state.ChangeState(StateMachine.StateType.Attack);
    }

    void StrengthAttack()
    {
        GetComponent<AttackSettings>().SetAction = ActionType.StrengthGround;
        _state.ChangeState(StateMachine.StateType.Attack);
    }

    void SetLockon()
    {
        if (!GameManager.Instance.IsLockOn)
        {
            var finds = GameObject.FindGameObjectsWithTag("Enemy")
                .Where(e => 
                {
                      float dist = Vector3.Distance(e.transform.position, transform.position);
                      if (dist < _lockOnDist) return e;
                      else return false;
                });
            
            if (finds.Count() <= 0) return;
            GameManager.Instance.IsLockOn = true;

            Vector3 cmForwrad = Camera.main.transform.forward;
            GameObject set = null;
            float saveAngle = float.MinValue;
            foreach (var e in finds)
            {
                Vector3 dir = transform.position - e.transform.position;
                float angle = Mathf.Abs(Vector3.Dot(cmForwrad, dir.normalized) * Mathf.Rad2Deg);
                if (saveAngle < angle)
                {
                    set = e;
                    saveAngle = angle;
                }
            }

            GameManager.Instance.LockonTarget = set;
            UIManager.CallBack(UIType.Player, 2, new object[] { set });
        }
        else
        {
            GameManager.Instance.IsLockOn = false;
        }
    }

    public void GetDamage(int damage)
    {
        if (_state.GetCurrentState == StateMachine.StateType.Avoid)
        {
            if (_isAvoid) return;
            _isAvoid = true;

            UIManager.CallBack(UIType.Player, 1);
            FieldManager.FieldTimeRate(UIManager.CallBack, UIType.Player, 1);
            
            return;
        }

        _hp -= damage;
        if (_hp <= 0)
        {
            Destroy(gameObject);
            GameManager.End();
        }
    }

    public void KnockBack(Vector3 dir)
    {
        if (_state.GetCurrentState == StateMachine.StateType.KnockBack) return;
        if (_state.GetCurrentState == StateMachine.StateType.Avoid) return;
        GetKnockDir = dir;
        _state.ChangeState(StateMachine.StateType.KnockBack);
    }
}
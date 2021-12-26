using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttackSetting;

public class Player : CharaBase, IDamage
{
    [SerializeField] float _speed;
    StateMachine _state;

    bool _isAvoid = false;

    bool _isLockon = false;
    public bool IsLockon => _isLockon;

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

        Vector3 set = Vector3.Scale(_state.Move * _speed, PhsicsBase.GetVelocity);
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
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null)
        {
            _isLockon = true;
            UIManager.CallBack(UIType.Player, 2, new object[] { enemy});
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

        Debug.Log("Damage");
    }

    public void KnockBack(Vector3 dir)
    {
        if (_state.GetCurrentState == StateMachine.StateType.KnockBack) return;
        if (_state.GetCurrentState == StateMachine.StateType.Avoid) return;
        GetKnockDir = dir;
        _state.ChangeState(StateMachine.StateType.KnockBack);
    }
}
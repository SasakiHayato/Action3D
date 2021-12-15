using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttackSetting;

public class Player : CharaBase, IDamage
{
    [SerializeField] float _speed;
    StateMachine _state;
    
    void Start()
    {
        _state = GetComponent<StateMachine>();

        Inputter.Instance.Inputs.Player.Fire.started += context
            => _state.ChangeState(StateMachine.StateType.Avoid);

        Inputter.Instance.Inputs.Player.Jump.started += context => Jump();

        Inputter.Instance.Inputs.Player.WeakAttack.started += context
            => WeakAttack();

        Inputter.Instance.Inputs.Player.StrengthAttack.started += context
            => StrengthAttack();
    }

    void Update()
    {
        _state.Base();
        Vector3 set = Vector3.Scale(_state.Move * _speed, Gravity.Velocity);
        Character.Move(set * Time.deltaTime);
    }

    void Jump() => Debug.Log("Jump");

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

    public float AddDamage() => 1;
    public void GetDamage(float damage)
    {
        if (_state.GetCurrentState == StateMachine.StateType.Avoid)
        {
            FieldManager.FieldTimeRate(5, 0.5f);
            Debug.Log("Avoid");
            return;
        }

        Debug.Log("Damage");
    }
}
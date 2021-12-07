using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttackSetting;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] float _speed;

    AttackSettings _attack;
    Gravity _gravity;
    StateManage _state;
    
    void Start()
    {
        _state = GetComponent<StateManage>();
        _attack = GetComponent<AttackSettings>();
        _gravity = GetComponent<Gravity>();
        InputSetUp();
    }

    void InputSetUp()
    {
        Inputter.Instance.Inputs.Player.Fire.started += context => _state.ChangeState(StateManage.StateType.Avoid);
        Inputter.Instance.Inputs.Player.Jump.started += context => Jump();
        Inputter.Instance.Inputs.Player.Attack.started += context => Attack();
    }

    void Update()
    {
        _state.SetInput((Vector2)Inputter.GetValue(InputType.PlayerMove));
        _state.StateUpdate();
    }

    void Jump() => _gravity.Force();
    void Attack() => _attack.Request(ActionType.Ground);
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;

public class PlayerAttack : StateMachine.State
{
    [SerializeField] float _moveTime;
    [SerializeField] float _moveSpeed = 1;

    float _timer;
    AttackSettings _attack = null;
    Vector3 _beforePos;

    public override void Entry(StateMachine.StateType beforeType)
    {
        if (_attack == null) _attack = Target.GetComponent<AttackSettings>();

        _timer = 0;
        if (beforeType == StateMachine.StateType.Avoid)
        {
            if(GameManager.Instance.IsLockOn)
            {
                Debug.Log("Counter");
                _attack.Request(ActionType.Counter);
            }
        }
        else
        {
            _attack.Request(_attack.ReadAction);
        }
    }

    public override void Run(out Vector3 move)
    {
        _timer += Time.deltaTime;
        if (_timer > _moveTime) move = Vector3.zero;
        else move = Target.transform.forward * _moveSpeed;

        //Rotate();
    }

    void Rotate()
    {
        Vector3 forward = Target.transform.position - _beforePos;
        _beforePos = Target.transform.position;
        forward.y = 0;
        if (forward.magnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.LookRotation(forward);
            Target.transform.rotation =
                Quaternion.Lerp(Target.transform.rotation, rotation, Time.deltaTime * 100);
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
            return StateMachine.StateType.None;
        }
    }
}

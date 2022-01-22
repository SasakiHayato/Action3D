using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;

public class BossAttack : StateMachine.State
{
    [SerializeField] float _moveTime;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _farDist;

    float _timer;

    AttackSettings _attack = null;
    GameObject _player = null;

    public override void Entry(StateMachine.StateType beforeType)
    {
        if (_attack == null)
        {
            _attack = Target.GetComponent<AttackSettings>();
            _player = GameObject.FindWithTag("Player");
        }

         _attack.Request(ActionType.WeakGround);
        _timer = 0;
    }

    public override void Run(out Vector3 move)
    {
        _timer += Time.deltaTime;

        if (_timer > _moveTime) move = Vector3.zero;
        else move = Target.transform.forward * _moveSpeed;

        _attack.NextRequest();
    }

    public override StateMachine.StateType Exit()
    {
        Vector3 mPos = Target.transform.position;
        float dist = Vector3.Distance(_player.transform.position, mPos);

        if (!_attack.EndCurrentAnim)
        {
            return StateMachine.StateType.Attack;
        }
        else
        {
            if (_attack.IsNextRequest)
            {
                Debug.Log("IsNextRequest");
                return Target.GetComponent<StateMachine>().RetuneState(StateMachine.StateType.Attack);
            }
            else
            {
                return StateMachine.StateType.Move;
            }
        }
    }
}

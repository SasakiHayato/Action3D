using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorAI;
using AttackSetting;

public class ShortDistanceAttack : IAction
{
    EnemyBase _enemyBase = null;
    AttackSettings _attack = null;

    bool _check = false;
    public GameObject Target { private get; set; } 

    public void Execute()
    {
        if (_enemyBase == null)
        {
            _enemyBase = Target.GetComponent<EnemyBase>();
            _attack = Target.GetComponent<AttackSettings>();
        }
        if (_attack.EndCurrentAnim)
        {
            _attack.Request(ActionType.WeakGround);
            _check = true;
        }
        
        _enemyBase.MoveDir = Vector3.zero;
    }

    public bool End() => _check;
    public bool Reset { set { _check = value; } }
}

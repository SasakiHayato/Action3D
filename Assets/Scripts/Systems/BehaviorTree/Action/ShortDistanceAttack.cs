using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAI;
using AttackSetting;

public class ShortDistanceAttack : IAction
{
    EnemyBase _enemyBase = null;
    AttackSettings _attack = null;

    bool _check = false;
    public GameObject Target { get; set; } 

    public void SetUp()
    {
        if (_enemyBase == null)
        {
            _enemyBase = Target.GetComponent<EnemyBase>();
            _attack = Target.GetComponent<AttackSettings>();
        }
    }

    public bool Execute()
    {
        
        if (_attack.EndCurrentAnim)
        {
            _attack.Request(ActionType.WeakGround);
            return  true;
        }
        
        _enemyBase.MoveDir = Vector3.zero;

        return false;
    }
}

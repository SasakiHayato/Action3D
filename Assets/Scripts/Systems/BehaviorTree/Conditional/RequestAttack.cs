using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAI;
using AttackSetting;

public class RequestAttack : IConditional
{
    public GameObject Target { get; set; }

    AttackSettings _attack = null;
    EnemyAttackSystem _enemySystem = null;

    public bool Check()
    {
        if (_attack == null)
        {
            _attack = Target.GetComponent<AttackSettings>();
            _enemySystem = Target.GetComponent<EnemyAttackSystem>();
        }




        return false;
    }
}

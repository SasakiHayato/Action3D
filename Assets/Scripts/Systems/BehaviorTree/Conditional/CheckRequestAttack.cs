using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAI;
using AttackSetting;

public class CheckRequestAttack : IConditional
{
    enum RequestType
    {
        Master,
        Individual,
    }

    [SerializeField] RequestType _type;

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

        switch (_type)
        {
            case RequestType.Master:
                return _enemySystem.MasterRequest;
            case RequestType.Individual:
                return _enemySystem.Request;
            default:
                return false;
        }
    }
}

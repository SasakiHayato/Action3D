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
    [SerializeField] bool _checkBool;

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
                return Set(_enemySystem.MasterRequest);
            case RequestType.Individual:
                return Set(_enemySystem.Request);
            default:
                Debug.Log("None");
                return false;
        }
    }

    bool Set(bool check)
    {
        if (_checkBool) return check ? true : false;
        else return !check ? true : false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
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

    AttackSettings _attack = null;
    EnemyAttackSystem _enemySystem = null;

    GameObject _user;

    public void SetUp(GameObject user)
    {
        _user = user;
    }

    public bool Try()
    {
        if (_attack == null)
        {
            _attack = _user.GetComponent<AttackSettings>();
            _enemySystem = _user.GetComponent<EnemyAttackSystem>();
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

    public void InitParam()
    { }
}

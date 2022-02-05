using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;
using BehaviourAI;

public class RequestEAS : IAction
{
    enum RequestType
    {
        Master,
        Individual,
    }

    [SerializeField] RequestType _type;
    [SerializeField] bool _checkBool;
    EnemyAttackSystem _attackSystem = null;

    public GameObject Target { get; set; }

    public void SetUp()
    {
        if (_attackSystem == null)
        {
            _attackSystem = Target.GetComponent<EnemyAttackSystem>();
        }
    }

    public bool Execute()
    {
        switch (_type)
        {
            case RequestType.Master:
                _attackSystem.SetMasterRequest(_checkBool);
                break;
            case RequestType.Individual:
                _attackSystem.SetRequest(_checkBool);
                break;
        }

        return true;
    }
}

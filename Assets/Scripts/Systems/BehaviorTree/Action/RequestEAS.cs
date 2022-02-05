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
                _attackSystem.SetMasterRequest(true);
                break;
            case RequestType.Individual:
                _attackSystem.SetRequest(true);
                break;
        }

        return true;
    }
}

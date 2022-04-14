using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewAttacks;
using BehaviourTree;

/// <summary>
/// çUåÇÇ∑ÇÈÇΩÇﬂÇÃConditionalÇ…ê\êøÇ∑ÇÈAIçsìÆ
/// </summary>

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

    public void SetUp(GameObject user)
    {
        _attackSystem = user.GetComponent<EnemyAttackSystem>();
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

    public void InitParam()
    {

    }
}

using UnityEngine;
using BehaviourTree;
using NewAttacks;

/// <summary>
/// RequestEAS‚©‚ç“`‚¦‚ç‚ê‚½ğŒ‚ğ’²‚×‚éğŒ•ªŠò
/// </summary>

public class CheckRequestAttack : IConditional
{
    enum RequestType
    {
        Master,
        Individual,
    }

    [SerializeField] RequestType _type;
    [SerializeField] bool _checkBool;

    EnemyAttackSystem _enemySystem = null;

    public void SetUp(GameObject user)
    {
        _enemySystem = user.GetComponent<EnemyAttackSystem>();
    }

    public bool Try()
    {
        switch (_type)
        {
            case RequestType.Master:
                return Set(_enemySystem.MasterRequest);
            case RequestType.Individual:
                return Set(_enemySystem.Request);
            default:
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

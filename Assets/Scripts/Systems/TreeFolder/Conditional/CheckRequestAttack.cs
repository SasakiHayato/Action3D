using UnityEngine;
using BehaviourTree;
using NewAttacks;

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

    GameObject _user;

    public void SetUp(GameObject user)
    {
        _user = user;
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

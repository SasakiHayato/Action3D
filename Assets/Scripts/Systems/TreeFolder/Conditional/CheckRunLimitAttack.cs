using UnityEngine;
using BehaviourTree;
using NewAttacks;

/// <summary>
/// SetRunLimitAttackから伝えられた情報を調べる条件分岐
/// </summary>

public class CheckRunLimitAttack : IConditional
{
    [SerializeField] bool _checkBool;
    EnemyAttackSystem _attackSystem;

    public void SetUp(GameObject user)
    {
        _attackSystem = user.GetComponent<EnemyAttackSystem>();
    }

    public bool Try()
    {
        return Check();
    }

    bool Check()
    {
        if (_checkBool)
        {
            if (_attackSystem.RunLimitAttack) return true;
            else return false;
        }
        else
        {
            if (_attackSystem.RunLimitAttack) return false;
            else return true;
        }
    }

    public void InitParam()
    {

    }
}

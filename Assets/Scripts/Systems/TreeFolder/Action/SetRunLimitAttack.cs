using UnityEngine;
using BehaviourTree;
using NewAttacks;

/// <summary>
/// 特殊行動を行い続ける際にConditionalに情報を伝えるAI行動
/// </summary>

public class SetRunLimitAttack : IAction
{
    [SerializeField] bool _setBool;
    EnemyAttackSystem _attackSystem;

    public void SetUp(GameObject user)
    {
        _attackSystem = user.GetComponent<EnemyAttackSystem>();
    }

    public bool Execute()
    {
        _attackSystem.SetRunLimitAttack(_setBool);
        return true;
    }

    public void InitParam()
    {

    }
}

using UnityEngine;
using BehaviourTree;
using NewAttacks;

/// <summary>
/// ����s�����s��������ۂ�Conditional�ɏ���`����AI�s��
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

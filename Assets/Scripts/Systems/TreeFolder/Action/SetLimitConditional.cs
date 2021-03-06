using UnityEngine;
using BehaviourTree;

/// <summary>
/// 特殊行動をするためのConditionalに情報を伝えるAI行動
/// </summary>

public class SetLimitConditional : IAction
{
    [SerializeField] int _setID;
    [SerializeField] bool _setBool;

    EnemyBase _enemyBase = null;

    public void SetUp(GameObject user)
    {
        _enemyBase = user.GetComponent<EnemyBase>();
    }

    public bool Execute()
    {
        _enemyBase.GetEnemyConditionalDatas[_setID - 1].SetBool(_setBool);
        return true;
    }

    public void InitParam()
    {

    }
}

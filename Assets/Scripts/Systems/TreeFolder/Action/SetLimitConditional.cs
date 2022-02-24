using UnityEngine;
using BehaviourTree;

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

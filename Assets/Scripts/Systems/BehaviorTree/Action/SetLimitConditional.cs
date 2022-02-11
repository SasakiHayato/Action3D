using UnityEngine;
using BehaviourAI;

public class SetLimitConditional : IAction
{
    [SerializeField] int _setID;
    [SerializeField] bool _setBool;

    EnemyBase _enemyBase = null;

    public GameObject Target { get; set; }
    public void SetUp()
    {
        if (_enemyBase == null) _enemyBase = Target.GetComponent<EnemyBase>();
    }

    public bool Execute()
    {
        _enemyBase.GetEnemyConditionalDatas[_setID - 1].SetBool(_setBool);
        return true;
    }
}

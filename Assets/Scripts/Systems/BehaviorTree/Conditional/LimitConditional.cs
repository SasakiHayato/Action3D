using UnityEngine;
using BehaviourAI;

public class LimitConditional : IConditional
{
    [SerializeField] int _checkID;
    [SerializeField] bool _checkBool;
    EnemyBase _enemyBase = null;

    public GameObject Target { get; set; }
    public bool Check()
    {
        if (_enemyBase == null) _enemyBase = Target.GetComponent<EnemyBase>();
        if (_checkBool)
        {
            if (_enemyBase.GetEnemyConditionalDatas[_checkID - 1].Check) return true;
            else return false;
        }
        else
        {
            if (!_enemyBase.GetEnemyConditionalDatas[_checkID - 1].Check) return true;
            else return false;
        }
    }
}

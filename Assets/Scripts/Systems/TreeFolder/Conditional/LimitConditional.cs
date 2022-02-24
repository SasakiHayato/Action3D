using UnityEngine;
using BehaviourTree;

public class LimitConditional : IConditional
{
    [SerializeField] int _checkID;
    [SerializeField] bool _checkBool;
    EnemyBase _enemyBase = null;

    GameObject _user;

    public void SetUp(GameObject user)
    {
        _user = user;
    }

    public bool Try()
    {
        if (_enemyBase == null) _enemyBase = _user.GetComponent<EnemyBase>();
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

    public void InitParam()
    {

    }
}

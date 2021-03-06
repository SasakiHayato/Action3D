using UnityEngine;
using BehaviourTree;

/// <summary>
/// ダメージを受けたかどうかを判定する
/// </summary>

public class CheckGetDamage : IConditional
{
    CharaBase _charaBase;

    public void SetUp(GameObject user)
    {
        _charaBase = user.GetComponent<CharaBase>();
    }

    public bool Try()
    {
        bool check = false;

        if (_charaBase.IsGetDamage)
        {
            check = true;
            _charaBase.IsGetDamage = false;
        }

        return check;
    }

    public void InitParam()
    {
        
    }
}

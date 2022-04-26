using UnityEngine;
using BehaviourTree;

/// <summary>
/// ƒ_ƒ[ƒW‚ğó‚¯‚½‚©‚Ç‚¤‚©‚ğ”»’è‚·‚é
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

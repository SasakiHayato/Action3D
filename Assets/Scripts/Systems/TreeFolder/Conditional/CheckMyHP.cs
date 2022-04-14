using UnityEngine;
using BehaviourTree;

/// <summary>
/// ©g‚Ì‘Ì—Í‚ğ’²‚×‚éğŒ•ªŠò
/// </summary>

public class CheckMyHP : IConditional
{
    enum CheckType
    {
        FixedNumber,
        Percent,
    }

    [SerializeField] CheckType _checkType;
    [SerializeField] float _effect;

    CharaBase _charaBase = null;
    GameObject _user;
    
    public void SetUp(GameObject user)
    {
        _user = user;
    }

    public bool Try()
    {
        if (_charaBase == null) _charaBase = _user.GetComponent<CharaBase>();

        if (_checkType == CheckType.FixedNumber)
        {
            if (_charaBase.HP <= _effect) return true;
            else return false;
        }
        else
        {
            float rateHp = Mathf.Lerp(0, _charaBase.MaxHP, _effect);
            if (_charaBase.HP <= rateHp) return true;
            else return false;
        }
    }

    public void InitParam()
    {

    }
}

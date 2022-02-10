using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAI;

public class CheckMyHP : IConditional
{
    enum CheckType
    {
        FixedNumber,
        Percent,
    }

    [SerializeField] CheckType _checkType;
    [SerializeField] int _effect;

    CharaBase _charaBase = null;
    
    public bool Check()
    {
        if (_charaBase == null) _charaBase = Target.GetComponent<CharaBase>();

        if (_checkType == CheckType.FixedNumber)
        {
            if (_charaBase.HP < _effect) return true;
            else return false;
        }
        else
        {
            float rateHp = _charaBase.HP / _effect;
            if (_charaBase.MaxHP > rateHp) return true;
            else return false;
        }
    }

    public GameObject Target { get; set; }
}

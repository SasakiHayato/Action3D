using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAI;

public class CheckMyHP : IConditional
{
    [SerializeField] int _effectHP;
    
    CharaBase _charaBase = null;
    bool _isCalled = false;

    public bool Check()
    {
        if (_charaBase == null) _charaBase = Target.GetComponent<CharaBase>();

        if (_charaBase.HP < _effectHP && !_isCalled)
        {
            
            _isCalled = true;
            return true;
        }
        else return false;
    }

    public GameObject Target { get; set; }
}

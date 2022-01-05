using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorAI;

public class CheckMyHP : IConditional
{
    [SerializeField] int _effectHP;
    bool _check = false;
    CharaBase _charaBase = null;

    public bool Check()
    {
        if (_charaBase == null) _charaBase = Target.GetComponent<CharaBase>();

        if (_charaBase.HP < _effectHP) _check = true;
        else _check = false;

        return _check;
    }

    public GameObject Target { private get; set; }
}

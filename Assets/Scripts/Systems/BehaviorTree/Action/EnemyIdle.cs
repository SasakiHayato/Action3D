using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorAI;

public class EnemyIdle : IAction
{
    [SerializeField] string _idleAnimName;
    [SerializeField] bool _useGravity;
    bool _check = false;
    Animator _anim = null;
    EnemyBase _enemyBase;

    public GameObject Target { private get; set; }
    public void Execute()
    {
        if (_anim == null)
        {
            _anim = Target.GetComponent<Animator>();
            _enemyBase = Target.GetComponent<EnemyBase>();
        }

        if (_idleAnimName != "") _anim.Play(_idleAnimName);

        if (_useGravity) _enemyBase.MoveDir = new Vector3(0, 1, 0);
        else _enemyBase.MoveDir = Vector3.zero;
        
        _check = true;
    }

    public bool End() => _check;
    public bool Reset { set { _check = value; } }
}

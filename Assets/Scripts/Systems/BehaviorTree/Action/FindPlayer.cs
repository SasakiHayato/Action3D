using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorAI;

public class FindPlayer : IAction
{
    [SerializeField] string _idleAnimName;
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
        _anim.Play(_idleAnimName);
        _enemyBase.MoveDir = Vector3.zero;
        _check = true;
    }

    public bool End() => _check;
    public bool Reset { set { _check = value; } }
}

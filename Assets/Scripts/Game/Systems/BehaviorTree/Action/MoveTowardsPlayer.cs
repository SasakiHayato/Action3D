using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorAI;

public class MoveTowardsPlayer : IAction
{
    [SerializeField] string _animName;

    bool _check = false;
    Animator _anim = null;
    EnemyBase _enemyBase = null;

    GameObject _player = null;

    public void Execute()
    {
        if (_enemyBase == null)
        {
            _enemyBase = Target.GetComponent<EnemyBase>();
            _anim = _enemyBase.gameObject.GetComponent<Animator>();
            _player = GameObject.FindWithTag("Player");
        }

        Vector3 dir = (_player.transform.position - Target.transform.position).normalized;
        dir.y = 1;
        _anim.Play(_animName);
        _enemyBase.MoveDir = dir;
    }

    public bool End() => _check;
    public bool Reset { set { _check = false; } }

    public GameObject Target { private get; set; }
}

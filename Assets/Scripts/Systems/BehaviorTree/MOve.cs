using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewBehaviourTree;

public class MOve : IAction
{
    [SerializeField] string _animName;
    [SerializeField] bool _applyYPos;
    [SerializeField] bool _toWardsPlayer;

    Animator _anim = null;
    EnemyBase _enemyBase = null;
    
    GameObject _player = null;

    public void SetUp()
    {
        if (_enemyBase == null)
        {
            _enemyBase = Target.GetComponent<EnemyBase>();

            _anim = Target.gameObject.GetComponent<Animator>();
            _player = GameObject.FindWithTag("Player");
        }
    }

    public bool Execute()
    {
        Vector3 dir = Vector3.zero;
        if (_toWardsPlayer)
            dir = (_player.transform.position - Target.transform.position).normalized;
        else
        {
            dir = (Target.transform.position - _player.transform.position).normalized;
            dir.y /= 3;
        }

        if (!_applyYPos) dir.y = 1;
        if (_animName != "") _anim.Play(_animName);

        _enemyBase.MoveDir = dir;
        LookPlayer(dir);
        return true;
    }

    void LookPlayer(Vector3 dir)
    {
        Vector3 forward = new Vector3(dir.x, 0, dir.z);
        Target.transform.rotation = Quaternion.LookRotation(forward);
    }

    public GameObject Target { get; set; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using NewAttacks;

/// <summary>
/// AttackSettingsコンポーネントをもつEnemyが攻撃するためのAI行動
/// </summary>

public class ShortDistanceAttack : IAction
{
    [SerializeField] NewAttacks.AttackType _type = NewAttacks.AttackType.Weak;

    EnemyBase _enemyBase = null;
    AttackSettings _attack = null;
    GameObject _player = null;

    GameObject _user;

    public void SetUp(GameObject user)
    {
        _user = user;
        _enemyBase = user.GetComponent<EnemyBase>();
        _attack = user.GetComponent<AttackSettings>();
        _player = GameObject.FindWithTag("Player");
    }

    public bool Execute()
    {
        _enemyBase.MoveDir = Vector3.zero;
        
        Rotate();
        _attack.Request(_type);

        return true;
    }

    void Rotate()
    {
        Vector3 diff = _player.transform.position - _user.transform.position;
        diff.y = 0;
        _user.transform.localRotation = Quaternion.LookRotation(diff);
    }

    public void InitParam()
    {
        
    }
}

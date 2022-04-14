using UnityEngine;
using BehaviourTree;

/// <summary>
/// Ç»Ç…Ç‡ÇµÇ»Ç¢AIçsìÆ
/// </summary>

public class EnemyIdle : IAction
{
    [SerializeField] string _idleAnimName;
    [SerializeField] bool _useGravity;

    Animator _anim = null;
    EnemyBase _enemyBase;

    public void SetUp(GameObject user)
    {
        _anim = user.GetComponent<Animator>();
        _enemyBase = user.GetComponent<EnemyBase>();
    }

    public bool Execute()
    {
        if (_idleAnimName != "") _anim.Play(_idleAnimName);

        if (_useGravity) _enemyBase.MoveDir = new Vector3(0, 1, 0);
        else _enemyBase.MoveDir = Vector3.zero;

        return true;
    }

    public void InitParam()
    {

    }
}

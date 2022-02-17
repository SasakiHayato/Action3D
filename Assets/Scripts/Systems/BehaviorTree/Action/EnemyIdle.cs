using UnityEngine;
using BehaviourAI;

public class EnemyIdle : IAction
{
    [SerializeField] string _idleAnimName;
    [SerializeField] bool _useGravity;

    Animator _anim = null;
    EnemyBase _enemyBase;

    public GameObject Target { get; set; }

    public void SetUp()
    {
        if (_anim == null)
        {
            _anim = Target.GetComponent<Animator>();
            _enemyBase = Target.GetComponent<EnemyBase>();
        }
    }

    public bool Execute()
    {
        if (_idleAnimName != "") _anim.Play(_idleAnimName);

        if (_useGravity) _enemyBase.MoveDir = new Vector3(0, 1, 0);
        else _enemyBase.MoveDir = Vector3.zero;

        return true;
    }
}

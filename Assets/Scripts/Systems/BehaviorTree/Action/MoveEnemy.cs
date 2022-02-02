using UnityEngine;
using BehaviourAI;
using AttackSetting;

public class MoveEnemy : IAction
{
    [SerializeField] string _animName;
    [SerializeField] bool _applyYPos;
    [SerializeField] bool _toWardsPlayer;

    Animator _anim = null;
    EnemyBase _enemyBase = null;
    AttackSettings _attack;

    GameObject _player = null;

    public void SetUp()
    {
        if (_enemyBase == null)
        {
            _enemyBase = Target.GetComponent<EnemyBase>();
            _attack = Target.GetComponent<AttackSettings>();
            _anim = Target.gameObject.GetComponent<Animator>();
            _player = GameObject.FindWithTag("Player");
        }
    }

    public bool Execute()
    {
        if (_attack != null)
        {
            if (!_attack.EndCurrentAnim) return true;
        }

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

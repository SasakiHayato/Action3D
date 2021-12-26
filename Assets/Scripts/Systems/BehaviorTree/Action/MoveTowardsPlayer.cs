using UnityEngine;
using BehaviorAI;
using AttackSetting;

public class MoveTowardsPlayer : IAction
{
    [SerializeField] string _animName;

    bool _check = false;
    Animator _anim = null;
    EnemyBase _enemyBase = null;
    AttackSettings _attack;

    GameObject _player = null;

    public void Execute()
    {
        if (_enemyBase == null)
        {
            _enemyBase = Target.GetComponent<EnemyBase>();
            _attack = Target.GetComponent<AttackSettings>();
            _anim = Target.gameObject.GetComponent<Animator>();
            _player = GameObject.FindWithTag("Player");
        }

        if (_attack != null)
        {
            if (!_attack.EndCurrentAnim) return;
        }

        Vector3 dir = (_player.transform.position - Target.transform.position).normalized;
        dir.y = 1;
        _anim.Play(_animName);
        _enemyBase.MoveDir = dir;

        LookPlayer(dir);

        _check = true;
    }

    void LookPlayer(Vector3 dir)
    {
        Vector3 forward = new Vector3(dir.x, 0, dir.z);
        Target.transform.rotation = Quaternion.LookRotation(forward);
    }

    public bool End() => _check;
    public bool Reset { set { _check = false; } }

    public GameObject Target { private get; set; }
}

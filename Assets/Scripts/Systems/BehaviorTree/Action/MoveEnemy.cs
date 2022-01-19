using UnityEngine;
using BehaviorAI;
using AttackSetting;

public class MoveEnemy : IAction
{
    [SerializeField] string _animName;
    [SerializeField] bool _applyYPos;
    [SerializeField] bool _toWardsPlayer;

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

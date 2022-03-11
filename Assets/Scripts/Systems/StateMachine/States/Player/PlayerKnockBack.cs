using UnityEngine;
using NewAttacks;

public class PlayerKnockBack : StateMachine.State
{
    [SerializeField] Player _player;
    [SerializeField] float _knockTime;
    [SerializeField] float _power;

    Vector3 _setDir = Vector3.zero;
    float _timer = 0;
    bool _isKnockBack = false;

    Animator _anim = null;
    NewAttackSettings _attack;

    public override void Entry(StateMachine.StateType beforeType)
    {
        if (_anim == null)
        {
            _anim = Target.GetComponent<Animator>();
            _attack = Target.GetComponent<NewAttackSettings>();
        }
        _attack.Cancel();
        _anim.Play("Damage_Front_Big_ver_C");

        _isKnockBack = true;
        _setDir = _player.GetKnockDir;
        _setDir.y = 0;
        _timer = 0;
    }

    public override void Run(out Vector3 move)
    {
        _timer += Time.deltaTime;
        if (_timer > _knockTime) _isKnockBack = false;
        move = _setDir * _power;
    }

    public override StateMachine.StateType Exit()
    {
        if (_isKnockBack) return StateMachine.StateType.KnockBack;
        else return StateMachine.StateType.Idle;
    }
}

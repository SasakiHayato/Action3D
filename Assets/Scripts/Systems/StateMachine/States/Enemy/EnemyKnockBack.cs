using UnityEngine;

public class EnemyKnockBack : StateMachine.State
{
    [SerializeField] string _animName;
    EnemyBase _enemyBase;

    bool _isSetUp = false;
    float _timer;
    float _gravity = Physics.gravity.y * -1;

    public override void Entry(StateMachine.StateType beforeType)
    {
        _enemyBase = Target.GetComponent<EnemyBase>();

        if (_animName != "") Target.GetComponent<Animator>().Play(_animName);

        if (_enemyBase.KnonckForwardPower == 0 && _enemyBase.KnonckUpPower == 0) _isSetUp = false;
        else
        {
            _timer = 0;
            _isSetUp = true;
        }
    }

    public override void Run(out Vector3 move)
    {
        _timer += Time.deltaTime;

        float vY = _enemyBase.KnonckUpPower - _gravity * _timer;
        float vX = _enemyBase.KnonckForwardPower - _gravity * _timer;

        if (vY <= 0 && vX <= 0) _isSetUp = false;

        Vector3 setVec = _enemyBase.KnockDir * _enemyBase.KnonckForwardPower;
        setVec.y = 1;
        setVec.y *= _enemyBase.KnonckUpPower;

        move = setVec;
    }

    public override StateMachine.StateType Exit()
    {
        if (!_isSetUp) return StateMachine.StateType.BehaviorTree;
        else return StateMachine.StateType.KnockBack;
    }
}
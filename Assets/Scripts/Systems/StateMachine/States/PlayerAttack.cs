using UnityEngine;
using AttackSetting;

public class PlayerAttack : StateMachine.State
{
    [SerializeField] float _moveTime;
    [SerializeField] float _moveSpeed = 1;

    float _timer;

    Player _player;
    AttackSettings _attack = null;
    CharacterController _character;
    
    public override void Entry(StateMachine.StateType beforeType)
    {
        if (_attack == null)
        {
            _attack = Target.GetComponent<AttackSettings>();
            _player = Target.GetComponent<Player>();
            _character = Target.GetComponent<CharacterController>();
        }

        _timer = 0;
        if (beforeType == StateMachine.StateType.Avoid)
        {
            if(GameManager.Instance.IsLockOn && _player.IsAvoid)
                _attack.Request(ActionType.Counter);
        }
        else
        {
            if (beforeType == StateMachine.StateType.Floating || !_character.isGrounded)
                _attack.Request(ActionType.Float);
            else
                _attack.Request(_attack.ReadAction);
        }
    }

    public override void Run(out Vector3 move)
    {
        _timer += Time.deltaTime;
        
        if (_timer > _moveTime) move = Vector3.zero;
        else move = Target.transform.forward * _moveSpeed;

        if (GameManager.Instance.IsLockOn) Rotate();
    }

    void Rotate()
    {
        Vector3 t = GameManager.Instance.LockonTarget.transform.position;
        Vector3 forward = t - Target.transform.position;

        forward.y = 0;
        if (forward.magnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.LookRotation(forward);
            Target.transform.rotation = rotation;
        }
    }

    public override StateMachine.StateType Exit()
    {
        if (!_attack.EndCurrentAnim)
        {
            return StateMachine.StateType.Attack;
        }
        else
        {
            if (_attack.IsNextRequest)
            {
                return Target.GetComponent<StateMachine>().RetuneState(StateMachine.StateType.Attack);
            }
            else
            {
                return StateMachine.StateType.None;
            }
        }
    }
}

using UnityEngine;
using Sounds;

public class PlayerAvoid : StateMachine.State
{
    [SerializeField] float _avoidTime;
    [SerializeField] float _speed;

    GameObject _mainCm;
    Vector2 _input = Vector2.zero;
    
    float _currentTime;

    public override void Entry(StateMachine.StateType beforeType)
    {
        _currentTime = 0;
        Target.GetComponent<AttackSetting.AttackSettings>().Cansel();
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");
        _input = (Vector2)Inputter.GetValue(InputType.PlayerMove);
        _input = _input.normalized;
        SoundMaster.Request(Target.transform, "Avoid", 0);
    }

    public override void Run(out Vector3 move)
    {
        Target.GetComponent<Animator>().Play("Dodge");
        if (_input == Vector2.zero) _input = Vector2.up * -1;
        _currentTime += Time.deltaTime;
        
        Vector3 forward = _mainCm.transform.forward * _input.y * _speed;
        Vector3 right = _mainCm.transform.right * _input.x * _speed;

        move = new Vector3(forward.x + right.x, 1, right.z + forward.z);
    }

    public override StateMachine.StateType Exit()
    {
        if (_currentTime > _avoidTime)
        {
            return StateMachine.StateType.Move;
        }
        else
        {
            return StateMachine.StateType.Avoid;
        }
    }
}

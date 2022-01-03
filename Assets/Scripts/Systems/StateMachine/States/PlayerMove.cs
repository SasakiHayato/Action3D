
using UnityEngine;

public class PlayerMove : StateMachine.State
{
    [SerializeField] float _dashSpeedRate;

    Animator _anim = null;
    GameObject _mainCm;
    Vector2 _input = Vector2.zero;

    float _setSpeedRate = 1;
    Vector3 _beforePos;
    
    public override void Entry(StateMachine.StateType beforeType)
    {
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");
        if (_anim == null) _anim = Target.GetComponent<Animator>();
        
        if (beforeType == StateMachine.StateType.Avoid)
        {
            _anim.Play("Run");
            _setSpeedRate = _dashSpeedRate;
        }
        else
        {
            _anim.Play("RunNoamal");
            _setSpeedRate = 1;
        }
            
    }

    public override void Run(out Vector3 move)
    {
        _input = (Vector2)Inputter.GetValue(InputType.PlayerMove) * _setSpeedRate;

        Vector3 forward = _mainCm.transform.forward * _input.y;
        Vector3 right = _mainCm.transform.right * _input.x;
        move = new Vector3(forward.x + right.x, 1, right.z + forward.z);

        Rotate();
    }

    void Rotate()
    {
        Vector3 forward = Target.transform.position - _beforePos;
        _beforePos = Target.transform.position;
        forward.y = 0;
        if (forward.magnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.LookRotation(forward);
            Target.transform.rotation = 
                Quaternion.Lerp(Target.transform.rotation, rotation, Time.deltaTime * 8);
        }
    }

    public override StateMachine.StateType Exit()
    {
        if (_input == Vector2.zero)
            return StateMachine.StateType.Idle;
        else 
            return StateMachine.StateType.Move;
    }
}

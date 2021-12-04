using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttackSetting;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    enum GroundState
    {
        Idle,
        Walk,
        Dash,
        Avoid,

        None,
    }

    GroundState _state = GroundState.Idle;

    [SerializeField] float _walkSpeed = 1f;
    [SerializeField] float _dashSpeed = 1f;
    [SerializeField] float _avoidSpeed = 1f;
    [SerializeField] float _avoidTime = 1f;
    [SerializeField] float _rotateSpeed;

    AttackSettings _attack;
    Gravity _g;

    CharacterController _cc;

    GameObject _mainCm;

    bool _isDash = false;
    float _runTime;
    
    void Start()
    {
        _attack = GetComponent<AttackSettings>();
        _cc = GetComponent<CharacterController>();
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");
        _g = GetComponent<Gravity>();
        InputSetUp();
    }

    void InputSetUp()
    {
        Inputter.Instance.Inputs.Player.Fire.started += context => Avoid();
        Inputter.Instance.Inputs.Player.Jump.started += context => Jump();
        Inputter.Instance.Inputs.Player.Attack.started += context => Attack();
    }

    void Update()
    {
        float speed = 0;

        switch (_state)
        {
            case GroundState.Idle:
                speed = 0;
                break;
            case GroundState.Walk:
                speed = _walkSpeed;
                break;
            case GroundState.Dash:
                speed = _dashSpeed;
                break;
            case GroundState.Avoid:
                speed = _avoidSpeed;
                break;
            case GroundState.None:
                break;
        }

        Vector3 move = MovePlayer(speed);
        
        if (move != Vector3.zero)
        {
            if (!_isDash) _state = GroundState.Walk;
            Rotate();
        }
        else
        {
            _state = GroundState.Idle;
            _runTime = 0;
        }
        
        float max = float.MinValue;
        if (Mathf.Abs(move.x) >= Mathf.Abs(move.y)) max = move.x;
        else max = move.z;

        if (Mathf.Abs(max) < 0.4f) _isDash = false;
        Vector3 velocity = Vector3.Scale(move, _g.Velocity);
        _cc.Move(velocity * Time.deltaTime);
    }

    void Rotate()
    {
        float rotateY = _mainCm.transform.localRotation.eulerAngles.y;
        Quaternion look = Quaternion.Euler(0, rotateY, 0);
        Quaternion rotate = Quaternion.Slerp
            (transform.localRotation, look, _rotateSpeed * Time.deltaTime);
        
        transform.localRotation = rotate;
    }

    Vector3 MovePlayer(float speed)
    {
        Vector2 move = (Vector2)Inputter.GetValue(InputType.PlayerMove);
        Vector3 foward = _mainCm.transform.forward * move.y * speed;
        Vector3 right = _mainCm.transform.right * move.x * speed;
        
        return new Vector3(foward.x + right.x, 1, foward.z + right.z);
    }

    void Jump() => _g.Force();
    void Attack() => _attack.Request(ActionType.Ground);
    void Avoid() => StartCoroutine(GoAvoid());

    IEnumerator GoAvoid()
    {
        float time = 0;
        while (time < _avoidTime)
        {
            time += Time.deltaTime;
            _isDash = true;
            _state = GroundState.Avoid;

            yield return null;
        }
     
        _state = GroundState.Dash;
    }
}
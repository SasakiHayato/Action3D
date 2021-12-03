using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttackSetting;

[RequireComponent(typeof(Rigidbody))]
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

    enum ActionState
    { 
        Avoid,

        None,
    }

    enum FloatState
    {
        Jump,

        None,
    }

    GroundState _state = GroundState.None;

    [SerializeField] float _walkSpeed = 1f;
    [SerializeField] float _dashSpeed = 1f;
    [SerializeField] float _avoidSpeed = 1f;
    [SerializeField] float _avoidTime = 1f;
    [SerializeField] float _cmSpeed;
    [SerializeField] float _rotateSpeed;

    Rigidbody _rb;
    CmCotrol _cm;
    AttackSettings _attack;

    GameObject _core;
    GameObject _mainCm;

    bool _isDash = false;

    private void Awake()
    {
        _core = new GameObject("Core");
        _core.AddComponent<Rigidbody>().useGravity = false;
        _core.transform.position = transform.position;
        _cm = GameObject.Find("3drParsonCm").GetComponent<CmCotrol>();
        _cm.SetUp();
    }

    void Start()
    {
        _state = GroundState.Idle;
        _rb = GetComponent<Rigidbody>();
        _attack = GetComponent<AttackSettings>();
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");
        Inputter.Instance.Inputs.Player.Fire.started += context => Avoid();
        Inputter.Instance.Inputs.Player.Jump.started += context => Jump();
        Inputter.Instance.Inputs.Player.Attack.started += context => Attack();
    }

    void Update()
    {
        //Debug.Log(_state);
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

        _cm.Move(_cmSpeed);

        Vector2 move = MovePlayer();
        
        if (move != Vector2.zero)
        {
            if (!_isDash) _state = GroundState.Walk;
            Rotate();
        }
        else
        {
            _state = GroundState.Idle;
        }
        
        float max = float.MinValue;
        if (Mathf.Abs(move.x) >= Mathf.Abs(move.y)) max = move.x;
        else max = move.y;

        if (Mathf.Abs(max) < 0.4f) _isDash = false;
        

        _rb.velocity = new Vector3
            (move.x * speed, _rb.velocity.y, move.y * speed);
        
        _core.GetComponent<Rigidbody>().MovePosition(transform.position);
    }

    void Rotate()
    {
        float rotateY = _mainCm.transform.localRotation.eulerAngles.y;
        Quaternion look = Quaternion.Euler(0, rotateY, 0);
        Quaternion rotate = Quaternion.Slerp
            (transform.localRotation, look, _rotateSpeed * Time.deltaTime);
        
        transform.localRotation = rotate;
    }

    Vector2 MovePlayer()
    {
        Vector2 move = (Vector2)Inputter.GetValue(InputType.PlayerMove);
        Vector3 foward = _mainCm.transform.forward * move.y;
        Vector3 right = _mainCm.transform.right * move.x;

        return new Vector2(foward.x + right.x, foward.z + right.z);
    }

    float Jump()
    {
        Debug.Log("ss");
        return 0;
    }

    void Attack()
    {
        _attack.Request(ActionType.Ground);
    }

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    enum State
    {
        Idle,
        Walk,
        Dash,
        Avoid,

        None,
    }

    State _state = State.None;

    [SerializeField] float _walkSpeed = 1f;
    [SerializeField] float _dashSpeed = 1f;
    [SerializeField] float _avoidSpeed = 1f;
    [SerializeField] float _cmSpeed;
    [SerializeField] float _rotateSpeed;

    Rigidbody _rb;
    CmCotrol _cm;

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
        _state = State.Idle;
        _rb = GetComponent<Rigidbody>();
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");
        Inputter.Instance.Inputs.Player.Fire.started += context => Avoid();
    }

    void Update()
    {
        Debug.Log(_state);
        float speed = 0;

        switch (_state)
        {
            case State.Idle:
                speed = 0;
                break;
            case State.Walk:
                speed = _walkSpeed;
                break;
            case State.Dash:
                speed = _dashSpeed;
                break;
            case State.Avoid:
                speed = _avoidSpeed;
                break;
            case State.None:
                break;
        }

        _cm.Move(_cmSpeed);

        Vector2 move = MovePlayer();
        if (move != Vector2.zero)
        {
            if (!_isDash) _state = State.Walk;
            Rotate();
        }
        else
        {
            _state = State.Idle;
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

    void Avoid()
    {
        StartCoroutine(GoAvoid());
    }

    IEnumerator GoAvoid()
    {
        _isDash = true;
        _state = State.Avoid;
        yield return new WaitForSeconds(1f);
        _state = State.Dash;
    }
}
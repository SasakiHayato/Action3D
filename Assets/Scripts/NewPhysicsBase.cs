using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPhysicsBase : MonoBehaviour
{
    [SerializeField] Transform _offSet;
    [SerializeField] Vector3 _ray = Vector3.zero;
    [SerializeField] LayerMask _hitLayer;
    [SerializeField] float _gravityScale = 1;
    
    public Vector3 Gravity { get => _gravity; private set { _gravity = value; } }
    Vector3 _gravity = Vector3.one;

    float _physicsGravity = Physics.gravity.y;
    float _timer;

    void Update()
    {
        if (GroundCheck())
        {
            _timer = 0;
            _gravity = Vector3.one;
            _gravity.y = -1;

            Debug.Log("IsGround");
        }
        else
        {
            Debug.Log("NotGround");
        }
    }

    void FixedUpdate()
    {
        SetGravity();
    }

    void SetGravity()
    {
        _timer += Time.fixedDeltaTime;
        _gravity.y = _timer * _physicsGravity * _gravityScale;
    }

    bool GroundCheck()
    {
        bool check = Physics.Raycast(_offSet.position, _ray, _hitLayer);

        return check;
    }
}

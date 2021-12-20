using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public Vector3 GetVelocity { get => _vel; }

    public float SetScale { set { _scale = value; } }
    public CharacterController SetCharactor { set { _charactor = value; } }
    
    Vector3 _vel = Vector3.one;

    float _scale = 1f;
    CharacterController _charactor;
    JumpSettings _jumpSettings;

    float _currentFallTime;
    float _g = Physics.gravity.y;
    bool _isFloating = false;

    void Start()
    {
        _jumpSettings = GetComponent<JumpSettings>();
    }

    private void FixedUpdate()
    {
        if(_isFloating)
        {
            Debug.Log("aaa");
            _vel.y = _jumpSettings.UpdateMovePosY();
            //if (_charactor.isGrounded) _isFloating = false;
        }
        else
        {
            if (_charactor.isGrounded) _currentFallTime = 0;
            else _vel.y = IsGravity();
        }
    }

    float IsGravity()
    {   
        _currentFallTime += Time.fixedDeltaTime;
        float velY = _g * _currentFallTime * _scale;

        return velY;
    }

    public void Floating()
    {
        if (_jumpSettings == null) return;

        _isFloating = true;
        _jumpSettings.SetJump();
    }
}

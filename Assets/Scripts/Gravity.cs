using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] GameObject _parent;

    [SerializeField] float _fallSpeed = 1f;
    [SerializeField] float _forceSpeed = 1f;
    [SerializeField] float _forceHeight = 5f;
    [SerializeField] float _forceTime = 0.5f;

    CharacterController _charaCtrl;

    Vector3 _vel = Vector3.one;
    public Vector3 Velocity { get => _vel; }

    float _currentFallTime;
    float _currentForceTime;
    float _initialVel;

    bool _isforce = false;

    void Start()
    {
        _charaCtrl = _parent.GetComponent<CharacterController>();
        // ‰‘¬“x
        float g = Physics.gravity.y * -1;
        float t = _forceTime;
        _initialVel = _forceHeight / t + (g * t / 2);
    }

    void Init()
    {
        _vel.y = 0;
        _currentFallTime = 0;
    }

    private void FixedUpdate()
    {
        _vel.y = 0;

        if (!_charaCtrl.isGrounded && !_isforce) _vel.y += IsGravity();
        else Init();

        if (_isforce) _vel.y += ChangeOfPosY();
    }

    float IsGravity()
    {   
        _currentFallTime += Time.fixedDeltaTime;
        float velY = Physics.gravity.y * _currentFallTime * _fallSpeed;

        return velY;
    }

    float ChangeOfPosY()
    {
        float g = Physics.gravity.y * -1;

        _currentForceTime += Time.fixedDeltaTime * _forceSpeed;
        float t = _currentForceTime;
        float v0 = _initialVel;
        float y = (v0 * t) - (g * t * t / 2);
        if (y > _forceHeight)
        {
            _isforce = false;
            _currentForceTime = 0;
            return 0;
        }
        else return y;
    }

    public void Force() => _isforce = true;
}

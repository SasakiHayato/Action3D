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

    float _g = Physics.gravity.y;

    bool _isforce = false;

    void Start()
    {
        
        // èâë¨ìx
        _initialVel = _forceHeight / _forceTime + (_g * _forceTime / 2);
    }

    void Init()
    {
        _vel.y = 0;
        _currentFallTime = 0;
        _currentForceTime = 0;
    }

    private void FixedUpdate()
    {
        _vel.y = 0;

        if (_isforce) _vel.y += ChangeOfPosY();
        else
        {
            _vel.y += IsGravity();
        }
    }

    float IsGravity()
    {   
        _currentFallTime += Time.fixedDeltaTime;
        float velY = _g * _currentFallTime /** _fallSpeed*/;

        return velY;
    }

    float ChangeOfPosY()
    {
        float g = _g * -1;

        _currentForceTime += Time.fixedDeltaTime * _forceSpeed;
        float t = _currentForceTime;
        float v0 = _initialVel;
        float y = (v0 * t) - (g * t * t / 2);

        float v = v0 - g * _currentForceTime;
        if (v < 0)
        {
            _isforce = false;
            Init();
        }
        Debug.Log(v);
        return y;
    }

    public void Force() => _isforce = true;
}

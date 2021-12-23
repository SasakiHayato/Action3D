using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSettings : MonoBehaviour
{
    [SerializeField] int _count;
    [SerializeField] float _speed;
    [SerializeField] float _InitialVelo;
    [SerializeField] float _y;

    float _gravityY = Physics.gravity.y;
    bool _isJump = false;
    float _currentTime = 0;

    float _saveY;
    
    private void Start()
    {
        _saveY = _y;
        _gravityY *= -1;
    }

    public void SetJump()
    {
        if (_isJump) return;

        //_isJump = true;
        _count--;
        _currentTime = 0;
    }

    public float UpdateMovePosY()
    {
        _currentTime += Time.deltaTime * _speed;
        float y = (_InitialVelo * _currentTime) - ((_gravityY * _currentTime * _currentTime) * 0.5f);

        return y;
    }
}

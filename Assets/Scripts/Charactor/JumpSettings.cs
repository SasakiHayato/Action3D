using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSettings : MonoBehaviour
{
    [SerializeField] int _count;
    [SerializeField] float _speed;
    [SerializeField] float _InitialVelo;

    float _gravityY = Physics.gravity.y;
    float _currentTime = 0;
    int _saveCount;

    private void Start()
    {
        _gravityY *= -1;
        _saveCount = _count;
    }

    public void SetJump()
    {
        if (_count == 0) return;

        _count--;
        _currentTime = 0;
    }

    public float UpdateMovePosY()
    {
        _currentTime += Time.deltaTime * _speed;
        float y = (_InitialVelo * _currentTime) - ((_gravityY * _currentTime * _currentTime) * 0.5f);

        return y;
    }

    public void ResetCount()
    {
        _count = _saveCount;
    }
}

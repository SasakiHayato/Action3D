using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSettings : MonoBehaviour
{
    [SerializeField] int _count;
    [SerializeField] float _speed;
    [SerializeField] float _v0;
    [SerializeField] float _y;

    float _g = Physics.gravity.y;
    bool _isJump = false;
    float _t = 0;

    float _saveY;
    
    private void Start()
    {
        _saveY = _y;
        _g *= -1;
    }

    public void SetJump()
    {
        if (_isJump) return;

        _isJump = true;
        _count--;
        _t = 0;
    }

    public float UpdateMovePosY()
    {
        _t += Time.deltaTime * _speed;
        float y = (_v0 * _t) - ((_g * _t * _t) * 0.5f);

        return y;
    }
}

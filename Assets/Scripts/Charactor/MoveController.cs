using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] float _maxSpeed;
    
    float _speed = 0;
    public float GetSpeed { get => _speed; }

    Vector2 _saveInput = Vector2.zero;

    public void GetInput(Vector2 input)
    {
        float diff = Vector2.Distance(_saveInput, input);
        
        SetParam();
        _saveInput = input;
    }

    void SetParam()
    {
        if (_saveInput == Vector2.zero) _speed = 0;
        else _speed = _maxSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvoid : StateMachine.State
{
    [SerializeField] float _avoidTime;
    [SerializeField] float _speed;

    GameObject _mainCm;
    Vector2 _input = Vector2.zero;
    
    float _currentTime;

    public override void Entry(StateMachine.StateType beforeType)
    {
        Debug.Log("EntryAvoid");
        _currentTime = 0;
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");
        _input = (Vector2)Inputter.GetValue(InputType.PlayerMove);
    }

    public override void Run(out Vector3 move)
    {
        _currentTime += Time.deltaTime;
        
        Vector3 forward = _mainCm.transform.forward * _input.y * _speed;
        Vector3 right = _mainCm.transform.right * _input.x * _speed;
        move = new Vector3(forward.x + right.x, 1, right.z + forward.z);
    }

    public override StateMachine.StateType Exit()
    {
        if (_currentTime > _avoidTime)
        {
            return StateMachine.StateType.Move;
        }
        else
        {
            return StateMachine.StateType.Avoid;
        }
    }
}

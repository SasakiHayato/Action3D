using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManage : MonoBehaviour
{
    public enum StateType
    {
        Idle,
        Move,
        Avoid,

        None,
    }

    public abstract class State
    {
        public Vector2 GetInput { get; set; }
        public CharacterController CC { get; set; }
        public abstract void Entry();
        public abstract void Update();
        public abstract StateType Exit();
    }

    StateType _type = StateType.None;
    StateType _saveType;

    Move _move;
    Idle _idle;
    Avoid _avoid;

    State _state = null;

    void Start()
    {
        _move = new Move();
        _idle = new Idle();
        _avoid = new Avoid();

        _saveType = _type;
    }

    public void StateUpdate()
    {
        switch (_type)
        {
            case StateType.Idle:
                _state = _idle;
                break;
            case StateType.Move:
                _state = _move;
                break;
            case StateType.Avoid:
                _state = _avoid;
                break;
            case StateType.None:
                _type = StateType.Idle;
                _state = _idle;
                break;
        }

        if (_type != _saveType)
        {
            _saveType = _type;
            _state.CC = GetComponent<CharacterController>();
            _state.Entry();
        }
        
        _state.Update();
        _type = _state.Exit();
    }

    public void SetInput(Vector2 get)
    {
        if (_state == null) return;
        _state.GetInput = get;
    }

    public void ChangeState(StateType type)
    {
        Debug.Log($"StateChange. Next:{type}");
        _type = type;
    }
}

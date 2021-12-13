using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum StateType
    {
        Idle = 0,
        Move = 1,
        Avoid = 2,
        Attack = 3,

        None = -1,
    }

    [System.Serializable]
    public abstract class State
    {
        [SerializeField] public StateType StateID;

        public abstract void Entry();
        public abstract void Run(out Vector3 move);
        public abstract StateType Exit();
    }

    [SerializeReference, SubclassSelector]
    List<State> _stateList = new List<State>();
    
    StateType _type = StateType.None;
    StateType _saveType;
    
    State _state = null;

    Vector3 _move;
    public Vector3 Move { get => _move; }

    void Start()
    {
        _saveType = _type;
    }

    public void StateUpdate()
    {
        foreach (State state in _stateList)
            if (state.StateID == _type)
                if (state.StateID == StateType.None)
                {
                    _type = StateType.Idle;
                    return;
                }
                else
                {
                    _state = state;
                }

        if (_type != _saveType)
        {
            _saveType = _type;
            _state.Entry();
        }
        Debug.Log(_state);
        _state.Run(out _move);
        _type = _state.Exit();
    }

    public void SetInput(Vector2 input)
    {

    }

    public void ChangeState(StateType type)
    {
        Debug.Log($"StateChange. Next:{type}");
        _type = type;
    }
}

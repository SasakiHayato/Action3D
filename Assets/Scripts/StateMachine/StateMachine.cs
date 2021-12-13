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

        public abstract void Entry(StateType beforeType);
        public abstract void Run(out Vector3 move);
        public abstract StateType Exit();
    }

    [SerializeReference, SubclassSelector]
    List<State> _stateList = new List<State>();
    
    StateType _type = StateType.None;
    StateType _saveType = StateType.None;
    
    State _state = null;

    Vector3 _move;
    public Vector3 Move { get => _move; }

    public void Base()
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
            _state.Entry(_saveType);
            _saveType = _type;
        }
        
        _state.Run(out _move);
        _type = _state.Exit();
    }

    public void ChangeState(StateType type)
    {
        _state.Exit();
        _type = type;
    }
}

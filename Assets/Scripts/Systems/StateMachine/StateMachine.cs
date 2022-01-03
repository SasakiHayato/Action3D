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
        Floating = 4,
        KnockBack = 5,

        None = -1,
        RetuneNext = -2,
    }

    [System.Serializable]
    public abstract class State
    {
        [SerializeField] public StateType StateID;
        
        public GameObject Target { get; set; }
        public abstract void Entry(StateType beforeType);
        public abstract void Run(out Vector3 move);
        public abstract StateType Exit();
    }

    [SerializeReference, SubclassSelector]
    List<State> _stateList = new List<State>();
    
    StateType _type = StateType.None;
    public StateType GetCurrentState { get => _type; }

    StateType _saveType = StateType.None;
    
    State _state = null;

    Vector3 _move;
    public Vector3 Move { get => _move; }

    public void Base()
    {
        if (StateType.RetuneNext != _saveType)
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
        }

        if (_type != _saveType)
        {
            _state.Target = gameObject;
            _state.Entry(_saveType);
            _saveType = _type;
        }
        
        _state.Run(out _move);
        _type = _state.Exit();
    }

    public void ChangeState(StateType type)
    {
        if (_state == null) return;
        _state.Exit();
        _type = type;
    }

    public StateType RetuneState(StateType type)
    {
        if (_state == null) return StateType.None;
        _saveType = StateType.RetuneNext;
        _type = type;
        return type;
    }
}

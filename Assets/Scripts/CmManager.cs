using UnityEngine;
using StateMachine;
using System.Collections.Generic;

public class CmManager : MonoBehaviour
{
    public class CmData
    {
        public class Data
        {
            public State State;
            public Vector3 Pos;
        }

        private static CmData _instance = new CmData();
        public static CmData Instance => _instance;

        public State CurrentState;
        public State NextState;

        public Transform User;
        public Transform NextTarget;

        List<Data> _datas = new List<Data>();

        public void AddData(State state, Vector3 pos)
        {
            Data data = new Data();
            data.State = state;
            data.Pos = pos;

            _datas.Add(data);
        }

        public Data GetData(State state)
        {
            foreach (Data data in _datas)
            {
                if (state == data.State) return data;
            }

            return null;
        }
    }

    public enum State
    {
        Normal,
        Lockon,
        Shake,
        Transition,
    }

    [SerializeField] Transform _user;
    [SerializeField] StateManager _state = new StateManager();

    void Start()
    {
        CmData.Instance.User = _user;

        _state.SetUp(gameObject)
            .AddState(State.Normal, "Normal")
            .AddState(State.Lockon, "Lockon")
            .AddState(State.Transition, "Transition")
            .RunRequest(State.Normal);
    }

    void Update()
    {
        _state.Update();
    }
}

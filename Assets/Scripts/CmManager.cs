using UnityEngine;
using StateMachine;
using System;

public class CmManager : MonoBehaviour
{
    public enum State
    {
        Normal,
        Lockon,
        Shake,
    }

    [SerializeField] StateManager _state = new StateManager();

    void Start()
    {
        Inputter inputter = new Inputter();
        Inputter.SetInstance(inputter, inputter);
        Inputter.Instance.Load();

        _state.SetUp(gameObject)
            .AddState(State.Normal, "Normal")
            .AddState(State.Lockon, "Lockon")
            .RunRequest(State.Normal);
    }

    void Update()
    {
        _state.Update();
    }
}

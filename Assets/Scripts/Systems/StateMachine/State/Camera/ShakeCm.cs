using UnityEngine;
using StateMachine;
using System;

public class ShakeCm : State
{
    [SerializeField] float _shakeTime;
    [SerializeField] float _shakePower;
    
    Transform _user;
    Transform _cm;

    float _timer;
    bool _shakeEnd = false;

    public override void SetUp(GameObject user)
    {
        _user = CmManager.CmData.Instance.User;
        _cm = user.transform;
    }

    public override void Entry(Enum before)
    {
        _timer = 0;
        _shakeEnd = false;
    }

    public override void Run()
    {
        _timer += Time.deltaTime;



        if (_timer > _shakeTime) _shakeEnd = true;
    }

    public override Enum Exit()
    {
        if (_shakeEnd) return CmManager.CmData.Instance.CurrentState;
        else return CmManager.State.Shake;
    }
}

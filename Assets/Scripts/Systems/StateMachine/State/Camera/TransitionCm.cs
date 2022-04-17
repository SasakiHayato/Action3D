using UnityEngine;
using StateMachine;
using System;

/// <summary>
/// カメラステートの遷移を管理するクラス
/// </summary>

public class TransitionCm : State
{
    [SerializeField] float _transitionSpeed;
    [SerializeField] float _viewDelay;

    Transform _user;
    Transform _cm;

    Vector3 _currentPos;
    Vector3 _nextPos;

    float _rotateTimer;

    float _timer;
    bool _isTransition = false;

    public override void SetUp(GameObject user)
    {
        _user = CmManager.CmData.Instance.User;
        _cm = user.transform;
    }

    public override void Entry(Enum before)
    {
        _isTransition = false;
        _timer = 0;

        CmManager.CmData.Data currntData = CmManager.CmData.Instance.GetData(CmManager.CmData.Instance.CurrentState);
        CmManager.CmData.Data nextData = CmManager.CmData.Instance.GetData(CmManager.CmData.Instance.NextState);

        _currentPos = currntData.IEntry.ResponsePos() + _user.position;
        _nextPos = nextData.IEntry.ResponsePos() + _user.position;
    }

    public override void Run()
    {
        _timer += Time.deltaTime * _transitionSpeed;
        Vector3 transition = Vector3.Lerp(_currentPos, _nextPos, _timer);

        CmManager.CmData.Instance.Position = transition;
        View();

        if (_nextPos == transition) _isTransition = true;
    }

    void View()
    {
        _rotateTimer += Time.deltaTime / _viewDelay;

        Vector3 dir = CmManager.CmData.Instance.NextTarget.position - _cm.position;
        Quaternion q = Quaternion.LookRotation(dir.normalized);
        _cm.rotation = Quaternion.Lerp(_cm.rotation, q, _rotateTimer);

        if (_cm.rotation == q) _rotateTimer = 0;
    }

    public override Enum Exit()
    {
        if (_isTransition)
        {
            CmManager.CmData.Instance.TransitionPos = _cm.position;
            return CmManager.CmData.Instance.NextState;
        }
        return CmManager.State.Transition;
    }
}

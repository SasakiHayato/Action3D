using UnityEngine;
using StateMachine;
using System;

public class LockonNearCm : State, ICmEntry
{
    [SerializeField] Vector3 _offSetPos;
    [SerializeField] float _viewDelay;
    [SerializeField] float _awayDist;
    [SerializeField] float _cmDist;

    Transform _user;
    Transform _cm;

    Transform _lockonTarget;

    float _dist;
    float _rotateTimer;

    Vector3 _setPos;

    public override void SetUp(GameObject user)
    {
        _user = CmManager.CmData.Instance.User;
        _cm = user.transform;

        CmManager.CmData.Instance.AddData(CmManager.State.LockonNear, _cm.position, GetComponent<ICmEntry>());
    }
    
    public Vector3 ResponsePos()
    {
        return _cm.position;
    }

    public override void Entry(Enum before)
    {
        _lockonTarget = GameManager.Instance.LockonTarget.transform;

        _cm.position = CmManager.CmData.Instance.TransitionPos;
        CmManager.CmData.Instance.CurrentState = CmManager.State.LockonNear;

        Vector3 diff = _lockonTarget.position - _user.position;
        float angle = (Mathf.Atan2(diff.z, diff.x) * Mathf.Rad2Deg) - 180;

        float rad = angle * Mathf.Deg2Rad;
        Vector3 pos = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * _cmDist;

        _setPos = pos + _offSetPos; 
    }

    public override void Run()
    {
        View();

        if (_lockonTarget == null) return;

        Vector2 playerPos = new Vector2(_user.position.x, _user.position.z);
        Vector2 targetPos = new Vector2(_lockonTarget.position.x, _lockonTarget.position.z);

        _dist = Vector2.Distance(playerPos, targetPos);

        CmManager.CmData.Instance.Position = _setPos + _user.position;
    }

    void View()
    {
        _rotateTimer += Time.deltaTime / _viewDelay;

        Vector3 dir = _user.position - _cm.position;
        Quaternion q = Quaternion.LookRotation(dir.normalized);
        _cm.rotation = Quaternion.Lerp(_cm.rotation, q, _rotateTimer);

        if (_cm.rotation == q) _rotateTimer = 0;
    }

    public override Enum Exit()
    {
        if (_lockonTarget == null)
        {
            CmManager.CmData.Data data = CmManager.CmData.Instance.GetData(CmManager.State.LockonNear);
            data.Pos = _cm.position - _user.position;

            CmManager.CmData.Instance.NextState = CmManager.State.Normal;
            CmManager.CmData.Instance.NextTarget = CmManager.CmData.Instance.User;
            
            return CmManager.State.Transition;
        }

        if (_dist > _awayDist)
        {
            CmManager.CmData.Data data = CmManager.CmData.Instance.GetData(CmManager.State.LockonNear);
            data.Pos = _cm.position - _user.position;

            CmManager.CmData.Instance.NextState = CmManager.State.Lockon;
            CmManager.CmData.Instance.NextTarget = CmManager.CmData.Instance.User;
            
            return CmManager.State.Transition;
        }
        else return CmManager.State.LockonNear;
    }
}

using UnityEngine;
using StateMachine;
using System;

public class LockonCm : State
{
    [SerializeField] Transform _user;
    [SerializeField] Vector3 _offSetPos;
    [SerializeField] float _viewDelay;

    Transform _cm;
    Transform _lookonTarget;

    float _rotateTimer;
    float _dist;

    public override void SetUp(GameObject user)
    {
        _cm = user.transform;
        _dist = Vector3.Distance(_user.position, _cm.position);
    }

    public override void Entry(Enum before)
    {
        _lookonTarget = GameManager.Instance.LockonTarget.transform;
        _rotateTimer = 0;
    }

    public override void Run()
    {
        Vector3 setPos = Vector3.zero;

        HorizontalPos(out setPos);
        setPos.y = VerticlePos();

        _cm.position = setPos;

        View();
    }

    void HorizontalPos(out Vector3 setPos)
    {
        Vector3 diff = _lookonTarget.position - _user.position;
        float angle = (Mathf.Atan2(diff.z, diff.x) * Mathf.Rad2Deg) - 180;

        float rad = angle * Mathf.Deg2Rad;
        Vector3 pos = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * _dist;
        setPos = pos + _user.position;
    }

    float VerticlePos()
    {
        return _offSetPos.y;
    }

    void View()
    {
        _rotateTimer += Time.deltaTime / _viewDelay;

        Vector3 dir = _lookonTarget.position - _cm.position;
        Quaternion q = Quaternion.LookRotation(dir.normalized);
        _cm.rotation = Quaternion.Lerp(_cm.rotation, q, _rotateTimer);

        if (_cm.rotation == q) _rotateTimer = 0;
    }

    public override Enum Exit()
    {
        if (GameManager.Instance.LockonTarget != null) return CmManager.State.Lockon;
        else return CmManager.State.Normal;
    }
}

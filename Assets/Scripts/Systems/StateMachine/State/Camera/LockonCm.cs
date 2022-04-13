using UnityEngine;
using StateMachine;
using System;

/// <summary>
/// Enemyをロックオンした際の制御クラス
/// </summary>

public class LockonCm : State
{
    [SerializeField] Vector3 _offSetPos;
    [SerializeField] float _deadDist;
    [SerializeField] float _viewDelay;

    Transform _user;
    Transform _cm;
    Transform _lookonTarget;

    float _rotateTimer;
    float _dist;

    Vector3 _saveHorizontalPos;
    Quaternion _saveQuaternion;

    public override void SetUp(GameObject user)
    {
        _user = CmManager.CmData.Instance.User;
        _cm = user.transform;
        _dist = Vector3.Distance(_user.position, _cm.position);

        CmManager.CmData.Instance.AddData(CmManager.State.Lockon, _cm.position);
    }

    public override void Entry(Enum before)
    {
        if (GameManager.Instance.LockonTarget != null)
        {
            _lookonTarget = GameManager.Instance.LockonTarget.transform;
        }
        else
        {
            _lookonTarget = null;
        }

        _rotateTimer = 0;

        _saveHorizontalPos = Vector3.zero;
        _saveQuaternion = Quaternion.identity;

        CmManager.CmData.Instance.CurrentState = CmManager.State.Lockon;
    }

    public override void Run()
    {
        Vector3 setPos = Vector3.zero;

        HorizontalPos(out setPos);
        setPos.y = VerticlePos();

        CmManager.CmData.Instance.Position = setPos;

        View();
    }

    void HorizontalPos(out Vector3 setPos)
    {
        if (_lookonTarget == null)
        {
            setPos = _saveHorizontalPos;
            return;
        }

        Vector3 diff = _lookonTarget.position - _user.position;
        float angle = (Mathf.Atan2(diff.z, diff.x) * Mathf.Rad2Deg) - 180;

        float rad = angle * Mathf.Deg2Rad;
        Vector3 pos = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * _dist;
        setPos = pos + _user.position;

        _saveHorizontalPos = setPos;
    }

    float VerticlePos()
    {
        return _offSetPos.y + _user.position.y;
    }

    void View()
    {
        if (_lookonTarget == null)
        {
            _cm.rotation = _saveQuaternion;
            return;
        }

        _rotateTimer += Time.deltaTime / _viewDelay;

        Vector3 dir = _lookonTarget.position - _cm.position;
        Quaternion q = Quaternion.LookRotation(dir.normalized);
        _cm.rotation = Quaternion.Lerp(_cm.rotation, q, _rotateTimer);

        _saveQuaternion = _cm.rotation;

        if (_cm.rotation == q) _rotateTimer = 0;
    }

    public override Enum Exit()
    {
        if (GameManager.Instance.LockonTarget != null) return CmManager.State.Lockon;
        else
        {
            CmManager.CmData.Data data = CmManager.CmData.Instance.GetData(CmManager.State.Lockon);
            data.Pos = _cm.position - _user.position;

            CmManager.CmData.Instance.NextState = CmManager.State.Normal;
            CmManager.CmData.Instance.NextTarget = CmManager.CmData.Instance.User;

            return CmManager.State.Transition;
        }
    }
}

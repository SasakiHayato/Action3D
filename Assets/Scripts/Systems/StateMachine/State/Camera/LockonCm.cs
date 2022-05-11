using UnityEngine;
using StateMachine;
using System;
using System.Linq;

/// <summary>
/// Enemyをロックオンした際の制御クラス
/// </summary>

public class LockonCm : State, ICmEntry
{
    [SerializeField] Vector3 _offSetPos;
    [SerializeField] float _deadInput = 0.75f;
    [SerializeField] float _lockOnDist;
    [SerializeField] float _deadDist;
    [SerializeField] float _viewDelay;

    Transform _user;
    Transform _cm;
    Transform _lookonTarget;

    float _rotateTimer;
    float _dist;
    bool _isNear;

    Vector3 _saveHorizontalPos;
    Quaternion _saveQuaternion;

    public override void SetUp(GameObject user)
    {
        _user = CmManager.CmData.Instance.User;
        _cm = user.transform;
        _dist = Vector3.Distance(_user.position, _cm.position);

        CmManager.CmData.Instance.AddData(CmManager.State.Lockon, _cm.position, GetComponent<ICmEntry>());
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

        _cm.position = CmManager.CmData.Instance.TransitionPos;
        CmManager.CmData.Instance.CurrentState = CmManager.State.Lockon;
    }

    public Vector3 ResponsePos()
    {
        Vector3 pos;

        HorizontalPos(out pos);
        pos.y = VerticlePos();

        return pos;
    }

    public override void Run()
    {
        if (_lookonTarget == null) return;

        Vector3 pos;

        HorizontalPos(out pos);
        pos.y = VerticlePos();

        Vector2 userPos = new Vector2(_user.position.x, _user.position.z);
        Vector2 tPos = new Vector2(_lookonTarget.position.x, _lookonTarget.position.z);
        float dist = Vector2.Distance(userPos, tPos);

        if (dist > _deadDist) _isNear = false;
        else _isNear = true;

        CmManager.CmData.Instance.Position = pos;

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
        float posY = _offSetPos.y + _user.position.y;
        
        return posY;
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
        if (_isNear)
        {
            CmManager.CmData.Data data = CmManager.CmData.Instance.GetData(CmManager.State.Lockon);
            data.Pos = _cm.position - _user.position;

            CmManager.CmData.Instance.NextState = CmManager.State.LockonNear;
            CmManager.CmData.Instance.NextTarget = CmManager.CmData.Instance.User;

            return CmManager.State.Transition;
        }

        if (GameManager.Instance.LockonTarget != null && _lookonTarget != null) return CmManager.State.Lockon;
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

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
    float _timer;
    float _dist;
    bool _isNear;

    Vector3 _saveHorizontalPos;
    Quaternion _saveQuaternion;

    int _saveInputX = 0;

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

        CmManager.CmData.Instance.CurrentState = CmManager.State.Lockon;
    }

    public Vector3 ResponsePos()
    {
        Vector3 pos = Vector3.zero;

        HorizontalPos(out pos);
        pos.y = VerticlePos();

        return pos;
    }

    public override void Run()
    {
        Vector3 pos = Vector3.zero;

        HorizontalPos(out pos);
        pos.y = VerticlePos();

        ChangeTarget();

        float dist = Vector3.Distance(_user.position, _lookonTarget.position);
        if (dist > _deadDist)
        {
            CmManager.CmData.Instance.Position = pos;
            _timer = 0;
            _isNear = false;
        }
        else
        {
            _isNear = true;
            MoveNear(pos);
        }

        View();
    }

    void MoveNear(Vector3 cmPos)
    {
        _timer += Time.deltaTime;

        Vector3 currentCmPos = CmManager.CmData.Instance.Position;
        Vector2 lerpSetPos = Vector3.Lerp(currentCmPos, cmPos, _timer);
        CmManager.CmData.Instance.Position = lerpSetPos;
    }

    void ChangeTarget()
    {
        Vector2 input = (Vector2)Inputter.Instance.GetValue(InputType.CmMove);
        if (Mathf.Abs(input.x) < _deadInput)
        {
            _saveInputX = 0;
            return;
        }

        if (input.x < 0 && _saveInputX != -1)
        {
            _saveInputX = -1;
            SetTarget();
        }
        else if (input.x > 0 && _saveInputX != 1)
        {
            _saveInputX = 1;
            SetTarget();
        }
    }

    void SetTarget()
    {
        var finds = GameObject.FindGameObjectsWithTag("Enemy")
                .Where(e =>
                {
                    float dist = Vector3.Distance(e.transform.position, transform.position);
                    if (dist < _lockOnDist) return e;
                    else return false;
                });

        if (finds.Count() <= 0) return;
        GameManager.Instance.IsLockOn = true;

        Vector3 cmForwrad = Camera.main.transform.forward;
        GameObject set = null;
        float saveAngle = float.MinValue;

        foreach (var e in finds)
        {
            Vector3 dir = Camera.main.transform.position - e.transform.position;

            float rad = Vector3.Dot(cmForwrad, dir.normalized);
            if (rad > 0) continue;

            float angle = Mathf.Acos(rad) * Mathf.Rad2Deg;
            if (_saveInputX == 1)
            {
                if (angle < 0) continue;
            }
            
            if (_saveInputX == -1)
            {
                if (angle > 0) continue;
            }

            if (saveAngle < angle)
            {
                CharaBase chara = e.GetComponent<CharaBase>();
                if (chara != null)
                {
                    set = e.GetComponent<CharaBase>().OffSetPosObj;
                    saveAngle = angle;
                }
            }
        }

        GameManager.Instance.LockonTarget = set;
        BaseUI.Instance.CallBack("Player", "Lockon", new object[] { set });
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

        Vector3 dir = SetViewPosition() - _cm.position;
        Quaternion q = Quaternion.LookRotation(dir.normalized);
        _cm.rotation = Quaternion.Lerp(_cm.rotation, q, _rotateTimer);

        _saveQuaternion = _cm.rotation;

        if (_cm.rotation == q) _rotateTimer = 0;
    }

    Vector3 SetViewPosition()
    {
        Vector3 setPos;
        if (_isNear) setPos = (_lookonTarget.position + _user.position) / 2;
        else setPos = _lookonTarget.position;

        return setPos;
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

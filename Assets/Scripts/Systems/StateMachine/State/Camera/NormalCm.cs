using UnityEngine;
using StateMachine;
using System;

public class NormalCm : State
{
    [SerializeField] Transform _user;
    [SerializeField] Vector3 _offSetPos;
    [SerializeField] float _sensitivityX = 0.5f;
    [SerializeField] float _sensitivityY = 0.5f;
    [SerializeField] float _viewDelay;
    [SerializeField] float _dead = 0.1f;

    Transform _cm;

    float _rotateTimer;
    float _horizontalAngle;
    float _verticleAngle;

    float _dist;
    float _savePosY;
    Vector3 _saveHorizontalPos;

    public override void SetUp(GameObject user)
    {
        _cm = user.transform;

        _cm.position = _user.position + _offSetPos;
        _dist = Vector3.Distance(_user.position, _cm.position);
    }

    public override void Entry(Enum before)
    {
        _saveHorizontalPos = new Vector2(_cm.position.x, _cm.position.z);
        _horizontalAngle = Mathf.Atan2(_saveHorizontalPos.z, _saveHorizontalPos.x) * Mathf.Rad2Deg - 90;

        _savePosY = _cm.position.y;
    }

    public override void Run()
    {
        Vector2 input = (Vector2)Inputter.GetValue(InputType.CmMove);

        Vector2 pos = HorizontalPos(input.normalized.x);
        float y = VerticlePos(input.normalized.y);

        _cm.position = new Vector3(pos.x, y, pos.y);

        View();
    }

    Vector2 HorizontalPos(float x)
    {
        float s = float.Parse(x.ToString("0.0"));
        if (Mathf.Abs(s * s) < _dead) return _saveHorizontalPos + _user.position;

        if (x > 0) _horizontalAngle++;
        else if (x < 0) _horizontalAngle--;

        float angle = _horizontalAngle * _sensitivityX;

        if (angle >= 360 || angle <= -360)
        {
            _horizontalAngle = 0;
            angle = 0;
        }

        float rad = angle * Mathf.Deg2Rad;
        Vector3 pos = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * _dist;

        _saveHorizontalPos = pos;
        
        return _saveHorizontalPos;
    }

    float VerticlePos(float y)
    {
        float s = float.Parse(y.ToString("0.0"));
        if (Mathf.Abs(s * s) < _dead) return _savePosY;

        if (y > 0) _verticleAngle++;
        else if (y < 0) _verticleAngle--;

        float angle = _verticleAngle * _sensitivityY * -1;

        if (angle > 90)
        {
            angle = 90;
            _verticleAngle++;
        }
        else if (angle < -90)
        {
            angle = -90;
            _verticleAngle--;
        }

        float rad = angle * Mathf.Deg2Rad;
        _savePosY = _offSetPos.y + Mathf.Sin(rad) * _dist;

        return _savePosY;
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
        if (GameManager.Instance.LockonTarget != null) return CmManager.State.Lockon;
        else return CmManager.State.Normal;
    }
}

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
    [SerializeField] float _deadInput = 0.1f;
    [SerializeField] float _cmDist;

    Transform _cm;

    float _rotateTimer;
    float _horizontalAngle;
    float _verticleAngle;

    float _savePosY;
    Vector3 _saveHorizontalPos;
    Vector3 _saveCmPos;

    const float Degree90 = 90f;

    public override void SetUp(GameObject user)
    {
        _cm = user.transform;
        _cm.position = _user.position + (_offSetPos.normalized * _cmDist);

        _saveCmPos = _user.position + (_offSetPos.normalized * _cmDist);
        _saveHorizontalPos = _offSetPos.normalized * _cmDist;
    }

    public override void Entry(Enum before)
    {
        _cm.position = _saveCmPos;
    }

    public override void Run()
    {
        Vector2 input = (Vector2)Inputter.GetValue(InputType.CmMove);

        Vector3 pos = HorizontalPos(input.normalized.x);
        float y = VerticlePos(input.normalized.y);
        
        _cm.position = new Vector3(pos.x, y, pos.z);

        View();
    }

    Vector3 HorizontalPos(float x)
    {
        float s = float.Parse(x.ToString("0.0"));
        if (Mathf.Abs(s * s) < _deadInput) return _saveHorizontalPos + _user.position;

        if (x > 0) _horizontalAngle++;
        else if (x < 0) _horizontalAngle--;

        float angle = _horizontalAngle * _sensitivityX;

        if (angle >= 360 || angle <= -360)
        {
            _horizontalAngle = 0;
            angle = 0;
        }

        float rad = (angle - Degree90) * Mathf.Deg2Rad;
        Vector3 pos = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * _cmDist;
        
        _saveHorizontalPos = pos;
        
        return _saveHorizontalPos + _user.position;
    }

    float VerticlePos(float y)
    {
        float s = float.Parse(y.ToString("0.0"));
        if (Mathf.Abs(s * s) < _deadInput) return _savePosY + _offSetPos.y;

        if (y > 0) _verticleAngle++;
        else if (y < 0) _verticleAngle--;

        float angle = _verticleAngle * _sensitivityY * -1;

        if (angle > Degree90)
        {
            angle = Degree90;
            _verticleAngle++;
        }
        else if (angle < Degree90 * -1)
        {
            angle = Degree90 * -1;
            _verticleAngle--;
        }

        float rad = angle * Mathf.Deg2Rad;
        _savePosY =  Mathf.Sin(rad) * _cmDist;

        return _savePosY + _offSetPos.y;
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
        if (GameManager.Instance.LockonTarget != null)
        {
            _saveCmPos = _cm.position;
            return CmManager.State.Lockon;
        }
        else return CmManager.State.Normal;
    }
}

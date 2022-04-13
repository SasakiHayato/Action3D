using UnityEngine;
using StateMachine;
using System;

/// <summary>
/// í èÌéûÇÃÉJÉÅÉâêßå‰ÉNÉâÉX
/// </summary>

public class NormalCm : State
{
    [SerializeField] Vector3 _offSetPos;
    [SerializeField] float _sensitivityX = 0.5f;
    [SerializeField] float _sensitivityY = 0.5f;
    [SerializeField] float _viewDelay;
    [SerializeField] float _deadInput = 0.1f;
    [SerializeField] float _deadAngle;
    [SerializeField] float _cmDist;

    Transform _user;
    Transform _cm;

    float _rotateTimer;
    float _horizontalAngle;
    float _verticleAngle;

    float _savePosY;
    Vector3 _saveHorizontalPos;
    
    const float Degree90 = 90f;
    const float MouseSensitivityRate = 2;

    public override void SetUp(GameObject user)
    {
        _user = CmManager.CmData.Instance.User;
        _cm = user.transform;
        _cm.position = _user.position + (_offSetPos.normalized * _cmDist);

        _saveHorizontalPos = _offSetPos.normalized * _cmDist;

        CmManager.CmData.Instance.AddData(CmManager.State.Normal, _cm.position);
    }

    public override void Entry(Enum before)
    {
        _cm.position = CmManager.CmData.Instance.TransitionPos;
        CmManager.CmData.Instance.CurrentState = CmManager.State.Normal;
    }

    public override void Run()
    {
        Vector2 input = (Vector2)Inputter.Instance.GetValue(InputType.CmMove);

        Vector3 pos = HorizontalPos(input.normalized.x);
        float y = VerticlePos(input.normalized.y);
        
        CmManager.CmData.Instance.Position = new Vector3(pos.x, y, pos.z);

        View();
    }

    Vector3 HorizontalPos(float x)
    {
        float s = float.Parse(x.ToString("0.0"));
        if (Mathf.Abs(s * s) < _deadInput) return _saveHorizontalPos + _user.position;

        if (x > 0) _horizontalAngle++;
        else if (x < 0) _horizontalAngle--;

        float sensitivity = 0;
        if (Inputter.Instance.IsConnectGamePad) sensitivity = _sensitivityX;
        else sensitivity = _sensitivityX * MouseSensitivityRate;

        float angle = _horizontalAngle * sensitivity;

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

        float sensitivity = 0;
        if (Inputter.Instance.IsConnectGamePad) sensitivity = _sensitivityY;
        else sensitivity = _sensitivityY * MouseSensitivityRate;

        float angle = _verticleAngle * sensitivity * -1;

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

        if (angle < _deadAngle)
        {
            angle = _deadAngle + 1;
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
            CmManager.CmData.Data data = CmManager.CmData.Instance.GetData(CmManager.State.Normal);
            data.Pos = _cm.position - _user.position;

            CmManager.CmData.Instance.NextState = CmManager.State.Lockon;
            CmManager.CmData.Instance.NextTarget = GameManager.Instance.LockonTarget.transform;

            return CmManager.State.Transition;
        }
        else return CmManager.State.Normal;
    }
}

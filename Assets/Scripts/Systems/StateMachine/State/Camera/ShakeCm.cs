using UnityEngine;
using StateMachine;
using System;
using System.Collections;

/// <summary>
/// ƒJƒƒ‰‚ğ—h‚ç‚·Û‚Ì§ŒäƒNƒ‰ƒX
/// </summary>

public class ShakeCm : State
{
    [SerializeField] float _shakeTime;
    [SerializeField] float _shakePower;
    
    Transform _user;
    Transform _cm;

    float _timer;
    float _rotateTimer;
    bool _shakeEnd = false;
    bool _shaking = false;

    Vector3 _saveCmPos;

    public override void SetUp(GameObject user)
    {
        _user = CmManager.CmData.Instance.User;
        _cm = user.transform;
    }

    public override void Entry(Enum before)
    {
        _timer = 0;
        _rotateTimer = 0;
        _shakeEnd = false;

        _saveCmPos = _cm.position;

        if (!_shaking) StartCoroutine(Shake());
    }

    public override void Run()
    {
        _timer += Time.deltaTime;
        View();
        if (_timer > _shakeTime) _shakeEnd = true;
    }

    IEnumerator Shake()
    {
        _shaking = true;

        while (_timer < _shakeTime)
        {
            float x = _saveCmPos.x + UnityEngine.Random.Range(-1f, 1f) * _shakePower;
            float y = _saveCmPos.y + UnityEngine.Random.Range(-1f, 1f) * _shakePower;
            float z = _saveCmPos.z + UnityEngine.Random.Range(-1f, 1f) * _shakePower;

            _cm.position = new Vector3(x, y, z);

            yield return null;
        }

        _cm.position = _saveCmPos;
        _shaking = false;
    }

    void View()
    {
        _rotateTimer += Time.deltaTime;

        Vector3 dir = _user.position - _cm.position;
        Quaternion q = Quaternion.LookRotation(dir.normalized);
        _cm.rotation = Quaternion.Lerp(_cm.rotation, q, _rotateTimer);

        if (_cm.rotation == q) _rotateTimer = 0;
    }

    public override Enum Exit()
    {
        if (_shakeEnd) return CmManager.CmData.Instance.CurrentState;
        else return CmManager.State.Shake;
    }
}

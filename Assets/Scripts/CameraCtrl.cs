using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] float _moveRate;

    CinemachineVirtualCamera _vCm;
    Cinemachine3rdPersonFollow _follow;

    float _v, _h;

    void Start()
    {
        _vCm = GetComponent<CinemachineVirtualCamera>();
        _follow = _vCm.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    private void Update()
    {
        Transform target = _vCm.Follow;
        Vector3 tEAngle = target.rotation.eulerAngles;
        if (Input.GetKey(KeyCode.J)) _v = 1;
        else if (Input.GetKey(KeyCode.L)) _v = -1;
        else _v = 0;

        if (Input.GetKey(KeyCode.I)) _h = 1;
        else if (Input.GetKey(KeyCode.K)) _h = -1;
        else _h = 0;

        tEAngle.y += _v * 180 * Time.deltaTime;
        tEAngle.x += _h * 180 * Time.deltaTime;


        target.rotation = Quaternion.Euler(tEAngle);
    }
}

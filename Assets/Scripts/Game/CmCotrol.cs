using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CmCotrol : MonoBehaviour
{
    [SerializeField] float _rotateSpeed;
    [SerializeField] Vector3 _offSet = Vector3.zero;

    CinemachineVirtualCamera _vCam;
    Cinemachine3rdPersonFollow _follow;

    GameObject _target;
    GameObject _core;

    Player _player;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _player = _target.GetComponent<Player>();

        _core = new GameObject("Core");
        _core.transform.position = _target.transform.position;

        _vCam = GetComponent<CinemachineVirtualCamera>();
        _follow = _vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        _follow.ShoulderOffset = _offSet;
        if (_vCam.Follow == null) _vCam.Follow = GameObject.Find("Core").transform;
    }

    void Update()
    {
        Move();
        _core.transform.position = _target.transform.position;
    }

    void Move()
    {
        Vector2 move = (Vector2)Inputter.GetValue(InputType.CmMove);
        Transform t = _vCam.Follow;
        Vector3 tRotate = t.eulerAngles;

        tRotate.y += move.x * _rotateSpeed;
        tRotate.x += move.y * _rotateSpeed;
        t.rotation = Quaternion.Euler(tRotate);
    }
}

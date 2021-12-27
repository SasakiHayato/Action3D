using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CmCotrol : MonoBehaviour
{
    [SerializeField] float _rotateSpeed;
    [SerializeField] Vector3 _offSet = Vector3.zero;
    bool _isLockOn = false;

    CinemachineVirtualCamera _vCam;
    Cinemachine3rdPersonFollow _follow;

    GameObject _player;
    GameObject _core;

    GameObject _target = null;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        
        _core = new GameObject("Core");
        _core.transform.position = _player.transform.position;

        _vCam = GetComponent<CinemachineVirtualCamera>();
        _follow = _vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        _follow.ShoulderOffset = _offSet;
        if (_vCam.Follow == null) _vCam.Follow = GameObject.Find("Core").transform;
    }

    void Update()
    {
        Move();
        _core.transform.position = _player.transform.position;
    }

    void Move()
    {
        
        Transform t = _vCam.Follow;
        Vector3 tRotate = t.eulerAngles;

        if(_isLockOn)
        {
            Vector3 set = _player.transform.position - _target.transform.position;
            set.y = 0;
            tRotate = Quaternion.Euler(set).eulerAngles;
        }
        else
        {
            Vector2 move = (Vector2)Inputter.GetValue(InputType.CmMove);
            tRotate.y += move.x * _rotateSpeed;
            tRotate.x += move.y * _rotateSpeed;
        }

        t.rotation = Quaternion.Euler(tRotate);
    }

    public void Lockon(GameObject target)
    {
        if (!_isLockOn)
        {
            _isLockOn = true;
            _target = target;
        }
        else
        {
            _isLockOn = false;
            _target = null;
        }

        Debug.Log("LockonCm");
    }
}

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
        Vector2 move = (Vector2)Inputter.GetValue(InputType.CmMove);
        Transform t = _vCam.Follow;
        Vector3 tRotate = t.eulerAngles;
        if (move.x != 0 || move.y != 0) GameManager.Instance.IsLockOn = false;
        if(GameManager.Instance.IsLockOn)
        {
            Vector3 set = _player.transform.position - _target.transform.position;
            set.z = 0;
            tRotate = set;
        }
        else
        {
            tRotate.y += move.x * _rotateSpeed;
            tRotate.x += move.y * _rotateSpeed;
        }

        t.rotation = Quaternion.Euler(tRotate);
    }

    public void Lockon(GameObject target)
    {
        if (GameManager.Instance.IsLockOn) _target = target;
        else _target = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CmCotrol : MonoBehaviour
{
    [SerializeField] float _rotateSpeed;
    [SerializeField] Vector3 _offSet = Vector3.zero;
    [SerializeField] float _lockonCollection;
    
    CinemachineVirtualCamera _vCam;
    Cinemachine3rdPersonFollow _follow;

    GameObject _player;
    GameObject _core;

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
        
        if(GameManager.Instance.IsLockOn)
        {
            LockOn(out tRotate);
        }
        else
        {
            tRotate.y += move.x * _rotateSpeed;
            tRotate.x += move.y * _rotateSpeed;
        }

        t.rotation = Quaternion.Euler(tRotate);
    }

    void LockOn(out Vector3 rotate)
    {
        Transform t = GameManager.Instance.LockonTarget.transform;
        Vector3 forward = (t.position - _player.transform.position).normalized;
        float angle = Mathf.Atan2(forward.z, forward.x) * Mathf.Rad2Deg;
        
        rotate = Quaternion.Euler(0, ((angle - 90) * -1) - _lockonCollection, 0).eulerAngles;
    }
}

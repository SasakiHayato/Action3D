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

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");

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

    public void RequestShake() => StartCoroutine(GoShake());

    IEnumerator GoShake()
    {
        Vector3 vec = _follow.ShoulderOffset;
        float time = 0.1f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            float posX = Random.Range(-1, 1);
            float posY = Random.Range(-1, 1);
            float posZ = Random.Range(-1, 1);
            Vector3 set = new Vector3(vec.x + posX, vec.y + posY, vec.z + posZ);
            _follow.ShoulderOffset = set;
            yield return null;
        }

        _follow.ShoulderOffset = vec;
    }
}

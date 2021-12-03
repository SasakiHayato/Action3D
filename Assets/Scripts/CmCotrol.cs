using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CmCotrol : MonoBehaviour
{
    CinemachineVirtualCamera _vCam;
    Cinemachine3rdPersonFollow _follow;

    public void SetUp()
    {
        _vCam = GetComponent<CinemachineVirtualCamera>();
        _follow = _vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        _follow.ShoulderOffset = new Vector3(0, 1f, 1.7f);
        if (_vCam.Follow == null) _vCam.Follow = GameObject.Find("Core").transform;
    }

    public void Move(float speed)
    {
        Vector2 move = (Vector2)Inputter.GetValue(InputType.CmMove);

        Transform t = _vCam.Follow;
        Vector3 tRotate = t.eulerAngles;

        tRotate.y += move.x * speed;
        tRotate.x += move.y * speed;
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

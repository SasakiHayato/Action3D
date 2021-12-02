using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CmCotrol : MonoBehaviour
{
    CinemachineVirtualCamera _vCam;

    public void SetUp()
    {
        _vCam = GetComponent<CinemachineVirtualCamera>();
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
}

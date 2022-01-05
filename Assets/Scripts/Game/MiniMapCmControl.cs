using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCmControl : MonoBehaviour
{
    [SerializeField] GameObject _parent;
    [SerializeField] Vector3 _offSet;

    Vector3 _setVec = Vector3.zero;

    void Start()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    void Update()
    {
        Vector3 tPos = _parent.transform.position;
        _setVec = new Vector3(tPos.x + _offSet.x, tPos.y + _offSet.y, tPos.z + _offSet.z);

        transform.position = _setVec;
    }
}

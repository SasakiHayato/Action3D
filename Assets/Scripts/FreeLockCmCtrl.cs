using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FreeLockCmCtrl : MonoBehaviour
{
    CinemachineFreeLook _look;
    void Start()
    {
        _look = GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        Vector2 input = (Vector2)Inputter.GetValue(InputType.CmMove);
        _look.m_YAxis.Value += input.y / 100;
        _look.m_XAxis.Value += input.x;
    }
}

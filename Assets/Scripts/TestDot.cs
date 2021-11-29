using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDot : MonoBehaviour
{
    [SerializeField] GameObject _target;
    Camera _cm;
    void Start()
    {
        _cm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 cmVec = _cm.transform.forward.normalized;
        Vector3 targetVec = 
            (_target.transform.position - _cm.transform.position).normalized;

        float dot = Vector3.Dot(cmVec, targetVec);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        if (angle <= 15 )
        {
            Debug.Log("Ž‹–ì“à");
        }
    }
}

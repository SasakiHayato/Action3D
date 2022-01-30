using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewBehaviourTree;

public class TestAction : IAction
{
    [SerializeField] float _timer;
    [SerializeField] string _debug;
    public GameObject Target { get; set; }

    float _time = 0;

    public void SetUp()
    {
        _time = 0;
    }

    public bool Execute()
    {
        _time += Time.deltaTime;
        if (_timer < _time)
        {
            Debug.Log(_debug);
            return true;
        }
        else
        {
            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewBehaviourTree;

public class TestCondition : IConditional
{
    [SerializeField] string _name;

    public GameObject Target { get; set; }
    public bool Check()
    {
        if (_name == "") return false;
        return Input.GetKey(_name);
    }
}

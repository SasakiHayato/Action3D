using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorAI;

public class ShotBullet : IAction
{
    bool _check = false;

    public void Execute()
    {
        Debug.Log("aaaa");
    }

    public bool End() => _check;
    public bool Reset { set { _check = value; } }
    public GameObject Target { private get; set; }
}

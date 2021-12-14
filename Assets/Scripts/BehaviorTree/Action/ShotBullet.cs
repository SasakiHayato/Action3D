using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorAI;

public class ShotBullet : IAction
{
    bool _check = false;

    public void Execute()
    {
        GameObject obj = BulletSettings.UseBullet(0);
        obj.GetComponent<Bullet>().Shot(Vector3.forward, 50);
    }

    public bool End() => _check;
    public bool Reset { set { _check = value; } }
    public GameObject Target { private get; set; }
}

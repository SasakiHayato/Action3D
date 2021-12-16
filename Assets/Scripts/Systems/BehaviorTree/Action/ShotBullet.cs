using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorAI;

public class ShotBullet : IAction
{
    [SerializeField] float _speed;
    [SerializeField] float _coolTime;
    
    bool _check = false;
    float _currentTime = 0;

    GameObject _player = null;

    public void Execute()
    {
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");

        _currentTime += Time.deltaTime;
        if (_currentTime > _coolTime)
        {
            _currentTime = 0;
            GameObject obj = BulletSettings.UseBullet(0);
            obj.transform.position = Target.transform.position;
            
            obj.GetComponent<Bullet>().Shot(Target.transform.forward, _speed * 10, Bullet.Parent.Enemy);
        }
    }

    public bool End() => _check;
    public bool Reset { set { _check = value; } }
    public GameObject Target { private get; set; }
}

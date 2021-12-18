using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorAI;

public class ShotBulletDeviation : IAction
{
    [SerializeField] float _speed;
    [SerializeField] float _coolTime;
    [SerializeField] int _bulletID;

    bool _check = false;
    float _currentTime = 0;

    GameObject _player = null;
    Vector3 _savePos;

    public void Execute()
    {
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");

        _currentTime += Time.deltaTime;
        if (_currentTime > _coolTime)
        {
            _currentTime = 0;

            GameObject obj = BulletSettings.UseRequest(_bulletID);
            obj.transform.position = Target.transform.position;
            obj.GetComponent<Bullet>().Shot(Set().normalized, _speed * 10, Bullet.Parent.Enemy);
        }

        _savePos = _player.transform.position;
    }

    Vector3 Set()
    {
        Vector3 set = Vector3.zero;
        Vector3 currentPos = _player.transform.position;
        Vector3 predictPos = currentPos + _savePos;

        Vector3 forward = currentPos - Target.transform.position;
        

        return set.normalized;
    }

    public bool End() => _check;
    public bool Reset { set { _check = value; } }
    public GameObject Target { private get; set; }
}

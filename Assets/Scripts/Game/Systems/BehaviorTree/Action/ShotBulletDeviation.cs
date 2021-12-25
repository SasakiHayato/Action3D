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
    Vector3 _beforePos;

    public void Execute()
    {
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");

        _currentTime += Time.deltaTime;
        if (_currentTime > _coolTime)
        {
            _currentTime = 0;

            GameObject obj = BulletSettings.UseRequest(_bulletID);
            obj.transform.position = Target.transform.position;
            obj.GetComponent<Bullet>().Shot(Set(), _speed * 10, Bullet.Parent.Enemy);
        }

        _beforePos = _player.transform.position;
    }

    Vector3 Set()
    {
        Vector3 myPos = Target.transform.position;
        Vector3 playerDir = (_player.transform.position - _beforePos).normalized;
        float playerDist = Vector3.Distance(_player.transform.position, _beforePos);

        // ‚È‚ñ‚©’m‚ç‚ñ‚¯‚Ç * _speed‚ð‚µ‚½‚ç‚¿‚å‚¤‚Ç—Ç‚­‚È‚Á‚½B
        float dist = Vector3.Distance(_player.transform.position, myPos) * 4;
        
        Vector3 predictPos = _player.transform.position + (playerDir * playerDist) * dist;
        Vector3 setDir = predictPos - myPos;

        return setDir.normalized;
    }

    public bool End() => _check;
    public bool Reset { set { _check = value; } }
    public GameObject Target { private get; set; }
}

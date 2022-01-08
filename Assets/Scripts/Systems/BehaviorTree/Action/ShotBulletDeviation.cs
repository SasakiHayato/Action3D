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
    Deviation _deviation = new Deviation();

    public void Execute()
    {
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");

        _currentTime += Time.deltaTime;
        if (_currentTime > _coolTime)
        {
            _currentTime = 0;

            GameObject obj = BulletSettings.UseRequest(_bulletID);
            obj.transform.position = Target.transform.position;

            float speed = _speed * 10;
            Vector3 tPos = _player.transform.position;
            Vector3 set = _deviation.DeviationDir(tPos, Target.transform.position, _beforePos, speed);
            obj.GetComponent<Bullet>()
                .Shot(set, speed, Bullet.Parent.Enemy, Target.GetComponent<CharaBase>().Power);
        }

        _beforePos = _player.transform.position;
    }

    public bool End() => _check;
    public bool Reset { set { _check = value; } }
    public GameObject Target { private get; set; }
}

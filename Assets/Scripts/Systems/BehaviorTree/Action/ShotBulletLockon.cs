using UnityEngine;
using BehaviorAI;

public class ShotBulletLockon : IAction
{
    [SerializeField] float _speed;
    [SerializeField] float _coolTime;
    [SerializeField] int _bulletID;
    
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
            
            GameObject obj = BulletSettings.UseRequest(_bulletID);
            obj.transform.position = Target.transform.position;
            Vector3 forward = (_player.transform.position - Target.transform.position).normalized;
            int power = Target.GetComponent<CharaBase>().Power;
            Debug.Log(power);
            obj.GetComponent<Bullet>()
                .Shot(forward, _speed * 10, Bullet.Parent.Enemy, power);
        }
    }

    public bool End() => _check;
    public bool Reset { set { _check = value; } }
    public GameObject Target { private get; set; }
}

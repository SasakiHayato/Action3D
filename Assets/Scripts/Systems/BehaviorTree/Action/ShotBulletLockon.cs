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
    EnemyBase _enemyBase = null;

    public void Execute()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _enemyBase = Target.GetComponent<EnemyBase>();
        }

        _currentTime += Time.deltaTime;
        if (_currentTime > _coolTime)
        {
            _currentTime = 0;
            
            GameObject obj = BulletSettings.UseRequest(_bulletID);
            obj.transform.position = Target.transform.position;
            Vector3 forward = (_player.transform.position - Target.transform.position).normalized;
            int power = Target.GetComponent<CharaBase>().Power;
            obj.GetComponent<Bullet>()
                .Shot(forward, _speed * 10, Bullet.Parent.Enemy, power);

            _check = true;
        }

        _enemyBase.MoveDir = Vector3.zero;
    }

    public bool End() => _check;
    public bool Reset { set { _check = value; } }
    public GameObject Target { private get; set; }
}

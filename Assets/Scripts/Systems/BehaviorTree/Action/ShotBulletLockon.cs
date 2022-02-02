using UnityEngine;
using BehaviourAI;

public class ShotBulletLockon : IAction
{
    [SerializeField] float _speed;
    [SerializeField] float _coolTime;
    [SerializeField] int _bulletID;
    
    float _currentTime = 0;

    GameObject _player = null;
    EnemyBase _enemyBase = null;

    public void SetUp()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _enemyBase = Target.GetComponent<EnemyBase>();
        }

        _currentTime = 0;
    }

    public bool Execute()
    {
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

            return true;
        }

        _enemyBase.MoveDir = Vector3.zero;
        return false;
    }

    public GameObject Target { get; set; }
}

using UnityEngine;
using BehaviourAI;

public class ShotBulletDeviation : IAction
{
    [SerializeField] float _speed;
    [SerializeField] float _coolTime;
    [SerializeField] int _bulletID;

    float _currentTime = 0;

    EnemyBase _enemyBase;
    GameObject _player = null;
    Vector3 _beforePos;
    Deviation _deviation = new Deviation();

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

            float speed = _speed * 10;
            Vector3 tPos = _player.transform.position;
            
            Vector3 set = _deviation.DeviationDir(tPos, Target.transform.position, _beforePos, speed);

            obj.GetComponent<Bullet>()
                .Shot(set, speed, Bullet.Parent.Enemy, Target.GetComponent<CharaBase>().Power);

            return true;
        }

        _enemyBase.MoveDir = Vector3.zero;
        _beforePos = _player.transform.position;
        Debug.Log("aaaa");
        return false;
    }

    public GameObject Target { get; set; }
}

using UnityEngine;
using BehaviourTree;

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

    GameObject _user;

    public void SetUp(GameObject user)
    {
        _user = user;
        _player = GameObject.FindGameObjectWithTag("Player");
        _enemyBase = user.GetComponent<EnemyBase>();

        _currentTime = 0;
    }

    public bool Execute()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > _coolTime)
        {
            GameObject obj = BulletSettings.UseRequest(_bulletID);
            obj.transform.position = _user.transform.position;

            float speed = _speed * 10;
            Vector3 tPos = _player.transform.position;
            
            Vector3 set = _deviation.DeviationDir(tPos, _user.transform.position, _beforePos, speed);

            obj.GetComponent<Bullet>()
                .Shot(set, speed, Bullet.Parent.Enemy, _user.GetComponent<CharaBase>().Power);

            return true;
        }

        _enemyBase.MoveDir = Vector3.zero;
        _beforePos = _player.transform.position;
        
        return false;
    }

    public void InitParam()
    {
        _currentTime = 0;
    }
}

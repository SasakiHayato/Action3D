using UnityEngine;
using BehaviourTree;

/// <summary>
/// PlayerÇ…å¸Ç©Ç¡ÇƒíeÇë≈Ç¬AIçsìÆ
/// </summary>

public class ShotBulletLockon : IAction
{
    [SerializeField] float _speed;
    [SerializeField] float _coolTime;
    [SerializeField] int _bulletID;
    
    float _currentTime = 0;

    GameObject _player = null;
    EnemyBase _enemyBase = null;

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
            Vector3 forward = (_player.transform.position - _user.transform.position).normalized;
            int power = _user.GetComponent<CharaBase>().Power;
            obj.GetComponent<Bullet>()
                .Shot(forward, _speed * 10, Bullet.Parent.Enemy, power);

            return true;
        }

        _enemyBase.MoveDir = Vector3.zero;
        return false;
    }

    public void InitParam()
    {
        _currentTime = 0;
    }
}

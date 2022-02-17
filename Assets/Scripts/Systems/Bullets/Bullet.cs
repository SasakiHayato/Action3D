using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour, IDamage
{
    public enum Parent
    {
        Player,
        Enemy,

        None,
    }

    Parent _parent = Parent.None;

    Rigidbody _rb;

    int _id;
    public float GetID { get => _id; }
    int _power;
    int _powerRate;
    Action<GameObject> _callBack;

    GameObject _target;
    Vector3 _tPostion = Vector3.zero;
    Vector3 _beforePos = Vector3.zero;

    float _time = 0;
    bool _isSet = false;

    float _speed;
    bool _isHoming = false;

    void Update()
    {
        if (!_isSet) return;
        _time += Time.deltaTime;
       
        if (_isHoming)
        {
            Homing();
        }

        if (_time > 5)
        {
            _callBack.Invoke(gameObject);
        }

        Rotate();
    }

    void Homing()
    {
        if (_target != null)
        {
            _tPostion = _target.transform.position;
            Vector3 dir = Vector3.Lerp(transform.position, _tPostion, _time / _speed);
            
            transform.position = dir;
        }
        else
        {
            _isHoming = false;
            Vector3 dir = _tPostion - transform.position;
            _rb.AddForce(dir.normalized * _speed, ForceMode.Impulse);
        }
    }

    void Rotate()
    {
        if (_beforePos == Vector3.zero)
        {
            _beforePos = transform.position;
            return;
        }
        Vector3 forward = (transform.position - _beforePos).normalized;
        _beforePos = transform.position;
        if (forward.magnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.LookRotation(forward);
            transform.rotation = rotation;
        }
    }

    public void GetDamage(int damage, AttackType type)
    {
        _callBack.Invoke(gameObject);
    }

    // 初期化
    public void Init()
    {
        _time = 0;
        _powerRate = 0;
        _isSet = false;
        _isHoming = false;
        _target = null;
    }

    /// <summary>
    /// BulletDataを参照して、Bullet自身にデータを入れ込む
    /// </summary>
    /// <param name="data">参照するデータ</param>
    /// <param name="callBack">PoolのDeleteの関数を登録</param>
    public void SetUp(BulletSettings.BulletData data, Action<GameObject> callBack)
    {
        _id = data.ID;
        _power = data.Power;
        
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        
        _callBack += callBack;
    }

    // Bulletをとばす処理
    public void Shot(Vector3 dir, float speed, Parent parent, int powerRate = 1)
    {
        _beforePos = Vector3.zero;
        _rb.velocity = Vector3.zero;
        _powerRate = powerRate;
        _parent = parent;
        
        _rb.AddForce(dir * speed, ForceMode.Impulse);
        _isSet = true;
    }

    public void ShotHoming(GameObject t, float speed, Parent parent, int powerRate = 1)
    {
        _target = t;
        _tPostion = t.transform.position;
        _speed = speed;
        _rb.velocity = Vector3.zero;
        _powerRate = powerRate;
        _parent = parent;

        _isHoming = true;
        _isSet = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (_parent)
        {
            case Parent.Player:
                if (other.CompareTag("Enemy"))
                {
                    other.GetComponent<IDamage>().GetDamage(_power * _powerRate, AttackType.Bullets);
                    _callBack.Invoke(gameObject);
                }
                break;
            case Parent.Enemy:
                if (other.CompareTag("Player"))
                {
                    other.GetComponent<IDamage>().GetDamage(_power * _powerRate, AttackType.Bullets);
                    other.GetComponent<Player>().KnockBack(transform.forward);
                    _callBack.Invoke(gameObject);
                }
                break;
        }
    }
}

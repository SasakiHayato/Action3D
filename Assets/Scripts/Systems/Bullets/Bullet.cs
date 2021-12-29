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
    Action<GameObject> _callBack;

    GameObject _target;
    Vector3 _beforePos = Vector3.zero;

    float _time = 0;
    bool _isSet = false;
    bool _isHoming = false;

    void Update()
    {
        if (!_isSet) return;

        if(_isHoming)
        {

        }

        _time += Time.deltaTime;
        if (_time > 5)
        {
            _callBack.Invoke(gameObject);
        }

        Rotate();
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

    public void GetDamage(int damage)
    {
        _callBack.Invoke(gameObject);
    }

    // 初期化
    public void Init()
    {
        _time = 0;
        _isSet = false;
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
        _power *= powerRate;
        _parent = parent;
        
        _rb.AddForce(dir * speed, ForceMode.Impulse);
        _isSet = true;
    }

    public void ShotHoming(float speed, GameObject target, Parent parent, int powerRate = 1)
    {
        _rb.velocity = Vector3.zero;
        _power *= powerRate;
        _parent = parent;
        _target = target;
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
                    other.GetComponent<IDamage>().GetDamage(_power);
                    other.GetComponent<EnemyBase>().KnockBack(transform.forward);
                    _callBack.Invoke(gameObject);
                }
                break;
            case Parent.Enemy:
                if (other.CompareTag("Player"))
                {
                    other.GetComponent<IDamage>().GetDamage(_power);
                    other.GetComponent<Player>().KnockBack(transform.forward);
                    _callBack.Invoke(gameObject);
                }
                break;
        }
    }
}

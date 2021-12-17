using System.Collections;
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
    float _power;
    Action<GameObject> _callBack;

    float _time = 0;
    bool _isSet = false;

    void Update()
    {
        if (!_isSet) return;
        _time += Time.deltaTime;
        if (_time > 5)
        {
            _callBack.Invoke(gameObject);
        }
    }

    public void GetDamage(float damage)
    {

    }

    public void Init()
    {
        _time = 0;
        _isSet = false;
    }

    public void SetUp(BulletSettings.BulletData data, Action<GameObject> callBack)
    {
        _id = data.ID;
        _power = data.Power;

        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _callBack += callBack;
    }

    public void Shot(Vector3 dir, float speed, Parent parent, float powerRate = 1)
    {
        _rb.velocity = Vector3.zero;
        _power *= powerRate;
        _parent = parent;
        _rb.AddForce(dir * speed, ForceMode.Impulse);
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
                    _callBack.Invoke(gameObject);
                }
                break;
            case Parent.Enemy:
                if (other.CompareTag("Player"))
                {
                    other.GetComponent<IDamage>().GetDamage(_power);
                    _callBack.Invoke(gameObject);
                }
                break;
        }
    }
}

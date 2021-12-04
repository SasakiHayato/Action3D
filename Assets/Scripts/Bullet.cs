using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _deleteTime;
    Rigidbody _rb;

    bool _isShot = false;
    float _time;

    void Update()
    {
        if (!_isShot) return;

        _time += Time.deltaTime;
        if (_time > _deleteTime) Destroy(gameObject);
    }

    public void SetUp()
    {
        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    public void Shot(Vector3 dir)
    {
        _rb.AddForce(dir.normalized * _speed, ForceMode.Impulse);
        _isShot = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    Rigidbody _rb;

    int _id;
    public float GetID { get => _id; }
    float _power;

    public void SetUp(BulletSettings.BulletData data)
    {
        _id = data.ID;
        _power = data.Power;
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    public void Shot(Vector3 dir, float speed)
    {
        _rb.AddForce(dir * speed, ForceMode.Impulse);
    }
}

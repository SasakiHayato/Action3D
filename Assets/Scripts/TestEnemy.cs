using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour, IDamage
{
    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public float AddDamage()
    {
        return 0;
    }

    public void GetDamage(float damage)
    {
        Debug.Log($"Get {damage} Damage. Obj{gameObject.name}");
    }
}

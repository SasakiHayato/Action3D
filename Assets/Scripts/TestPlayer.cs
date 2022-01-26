using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour, IDamage
{
    [SerializeField] float _speed;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * _speed, ForceMode.Impulse);
    }

    public void GetDamage(int damage, AttackType type)
    {
        Debug.Log("GetDamage");
    }
}

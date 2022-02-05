using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidCorrector : MonoBehaviour
{
    [SerializeField] float _avoidDist;
    GameObject _player;
    IDamage _iDamage;

    Collider _collider;

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _iDamage = _player.GetComponent<IDamage>();

        _collider = GetComponent<Collider>();
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, _player.transform.position);
        if (dist < _avoidDist && _collider.enabled)
        {
            _iDamage.GetDamage(0, AttackType.None);
        }
    }
}
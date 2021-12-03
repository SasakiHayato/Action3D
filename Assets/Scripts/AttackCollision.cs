using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttackSetting;

public class AttackCollision : MonoBehaviour, IAttack
{
    [SerializeField] int _groupID;
    public int GroupID { get => _groupID; }

    GameObject _parent;

    IDamage _iDamage;
    bool _check;

    public void SetUp(GameObject parent)
    {
        _parent = parent;
        Init();
    }

    public object[] CallBack()
    {
        object[] call = { _check, _iDamage };
        return call;
    }

    public void Init()
    {
        _check = false;
        _iDamage = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_parent.CompareTag("Player") == other.CompareTag("Enemy"))
        {
            Debug.Log("Player is Attack");
            _check = true;
            _iDamage = other.GetComponent<IDamage>();
        }
        else if (_parent.CompareTag("Enemy") == other.CompareTag("Player"))
        {
            Debug.Log("Enemy is Attack");
            _check = true;
            _iDamage = other.GetComponent<IDamage>();
        }
    }
}

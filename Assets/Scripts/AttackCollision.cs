using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttackSetting;

public class AttackCollision : MonoBehaviour, IAttack
{
    GameObject _parent;

    public void SetUp(GameObject parent)
    {
        _parent = parent;
    }

    public bool CallBack()
    {
        return true;
    }

    public void Init()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (_parent.CompareTag("Player") == other.CompareTag("Enemy"))
        {
            Debug.Log("Player is Attack");
        }
        else if (_parent.CompareTag("Enemy") == other.CompareTag("Player"))
        {
            Debug.Log("Enemy is Attack");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttackSetting;

public class AttackCollision : MonoBehaviour, IAttack
{
    public enum Parent
    {
        Player,
        Enemy,
    }

    [SerializeField] int _groupID;
    public int GroupID { get => _groupID; }
    public Parent ParentID { get; private set; }

    GameObject _parent;
    GameObject _hitObj;
    IDamage _iDamage;

    bool _check;

    public void SetUp(GameObject parent)
    {
        _parent = parent;
        if (_parent.CompareTag("Player")) ParentID = Parent.Player;
        else ParentID = Parent.Player;
        Init();
    }

    public object[] CallBack()
    {
        object[] call = { _check, _iDamage, _hitObj };
        return call;
    }

    public void Init()
    {
        _check = false;
        _iDamage = null;
        _hitObj = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_parent == null) return;

        if (_parent.CompareTag("Player") == other.CompareTag("Enemy"))
        {
            Debug.Log("Player is Attack");
            SetParam(other);
        }
        else if (_parent.CompareTag("Enemy") == other.CompareTag("Player"))
        {
            Debug.Log("Enemy is Attack");
            SetParam(other);
        }
    }

    void SetParam(Collider other)
    {
        _check = true;
        _iDamage = other.GetComponent<IDamage>();
        _hitObj = other.gameObject;
    }
}

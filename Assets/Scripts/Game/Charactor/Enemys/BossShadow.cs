using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;

public class BossShadow : EnemyBase, IDamage
{
    bool _isBackKnock = false;
    GameObject _player;

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        SetKnockBack(ref _isBackKnock);
        Tree.Repeater();
        Rotate();
        Vector3 move = MoveDir;
        
        Vector3 set = Vector3.Scale(move * Speed, PhsicsBase.GetVelocity);
        Character.Move(set * Time.deltaTime);
    }

    void Rotate()
    {
        Vector3 diff = _player.transform.position - transform.position;
        diff.y = 0;
        transform.localRotation = Quaternion.LookRotation(diff);
    }

    public void GetDamage(int damage, AttackType type)
    {
        HP -= damage;
        if (HP < 0) base.Dead(gameObject);
    }

    public override void KnockBack(Vector3 dir)
    {
        _isBackKnock = true;
        MoveDir = dir;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;

public class Skeleton : EnemyBase, IDamage
{
    [SerializeField] float _speed = 1;

    Animator _anim;
    AttackSettings _attack;
    bool _isDamage = false;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _attack = GetComponent<AttackSettings>();
    }

    void Update()
    {
        if (_isDamage) return;

        Tree.Repeater(this);
        Vector3 set = Vector3.Scale(MoveDir * _speed, PhsicsBase.GetVelocity);
        Character.Move(set * Time.deltaTime);
    }

    public void GetDamage(int damage)
    {
        _anim.Play("SkeletonDamage");
        _isDamage = true;
        _attack.Cansel();
        StartCoroutine(EndAnim());
    }

    IEnumerator EndAnim()
    {
        yield return null;
        yield return new WaitAnim(_anim);
        _isDamage = false;
    }

    public override void KnockBack(Vector3 dir)
    {
        
    }
}

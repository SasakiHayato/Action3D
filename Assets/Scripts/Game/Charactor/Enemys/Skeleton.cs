using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;

public class Skeleton : EnemyBase, IDamage, IFieldEnemy
{
    Animator _anim;
    AttackSettings _attack;
    bool _isDamage = false;
    bool _isBackKnock = false;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _attack = GetComponent<AttackSettings>();
        Tree = GetComponent<BehaviourAI.BehaviourTree>();
    }

    void Update()
    {
        SetKnockBack(ref _isBackKnock);
        if (_isDamage) return;

        Tree.Repeater();
        Vector3 set = Vector3.Scale(MoveDir * Speed, PhsicsBase.GetVelocity);
        Character.Move(set * Time.deltaTime);
    }

    public void GetDamage(int damage, AttackType type)
    {
        Sounds.SoundMaster.Request(transform, "Damage", 2);
        _isDamage = true;
        _attack.Cancel();

        object[] setUiData = { damage, gameObject, ColorType.Enemy };
        UIManager.CallBack(UIType.Game, 4, setUiData);

        StartCoroutine(EndAnim());
        HP -= damage;
        if (HP < 0) base.Dead(gameObject);
    }

    IEnumerator EndAnim()
    {
        yield return null;
        yield return new WaitAnim(_anim);
        _isDamage = false;
    }

    public override void KnockBack(Vector3 dir)
    {
        _anim.Play("SkeletonDamage");
        _isBackKnock = true;
        MoveDir = dir;
    }
}

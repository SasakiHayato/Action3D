using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : EnemyBase, IDamage
{
    Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
        MoveDir = new Vector3(0, 1, 0);
    }

    void Update()
    {
        Tree.Repeater(this);
        Vector3 set = Vector3.Scale(MoveDir, PhsicsBase.GetVelocity);
        Character.Move(set * Time.deltaTime);
    }

    public void GetDamage(int damage)
    {

    }

    public override void KnockBack(Vector3 dir)
    {
        
    }
}

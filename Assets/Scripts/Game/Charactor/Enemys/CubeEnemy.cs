using UnityEngine;

public class CubeEnemy : EnemyBase, IDamage, IFieldEnemy
{
    bool _isKnockBack = false;

    void Update()
    {
        SetKnockBack(ref _isKnockBack);
        Tree.Repeater();
        Vector3 set = Speed * MoveDir;
        Character.Move(set * Time.deltaTime);
    }

    public void GetDamage(int damage, AttackType type)
    {
        HP -= damage;
        if (HP <= 0) base.Dead(gameObject);
    }

    public override void KnockBack(Vector3 dir)
    {
        _isKnockBack = true;
        MoveDir = dir;
    }
}

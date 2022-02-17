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
        object[] setUiData = { damage, gameObject, ColorType.Enemy };
        UIManager.CallBack(UIType.Game, 4, setUiData);
        Sounds.SoundMaster.Request(transform, "Damage", 2);
        HP -= damage;
        if (HP <= 0) base.Dead(gameObject);
    }

    public override void KnockBack(Vector3 dir)
    {
        _isKnockBack = true;
        MoveDir = dir;
    }
}

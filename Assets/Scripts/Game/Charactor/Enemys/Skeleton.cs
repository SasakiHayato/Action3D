using UnityEngine;
using NewAttacks;

public class Skeleton : EnemyBase, IDamage, IFieldEnemy
{
    AttackSettings _attack;
    
    void Start()
    {
        _attack = GetComponent<AttackSettings>();

        BaseState.SetUp(gameObject)
            .AddState(State.BehaviorTree, "Tree")
            .AddState(State.KnockBack, "Knock")
            .RunRequest(State.BehaviorTree);
    }

    void Update()
    {
        BaseState.Update();

        Vector3 set = Vector3.Scale(MoveDir * Speed, PhsicsBase.GetVelocity);
        Character.Move(set * Time.deltaTime);
    }

    public void GetDamage(int damage, AttackType type)
    {
        BaseState.ChangeState(State.KnockBack);
        
        Sounds.SoundMaster.PlayRequest(transform, "Damage", Sounds.SEDataBase.DataType.Enemys);
        
        _attack.Cancel();

        object[] setUiData = { damage, gameObject, ColorType.Enemy };
        BaseUI.Instance.CallBack("Game", "Damage", setUiData);

        HP -= damage;
        if (HP < 0) base.Dead(gameObject);
    }
}

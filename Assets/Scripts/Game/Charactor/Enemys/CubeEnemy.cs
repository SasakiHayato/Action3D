using UnityEngine;

public class CubeEnemy : EnemyBase, IDamage, IFieldEnemy
{
    void Start()
    {
        BaseState.SetUp(gameObject)
            .AddState(State.BehaviorTree, "Tree")
            .AddState(State.KnockBack, "Knock")
            .AddState(State.Idle, "Idle")
            .RunRequest(State.BehaviorTree);
    }

    void Update()
    {
        BaseState.Update();

        Vector3 set = Speed * MoveDir;
        Character.Move(set * Time.deltaTime);
    }

    public void GetDamage(int damage, AttackType type)
    {
        object[] setUiData = { damage, gameObject, ColorType.Enemy };
        BaseUI.Instance.CallBack("Game", "Damage", setUiData);
        
        Sounds.SoundMaster.PlayRequest(transform, "Damage", Sounds.SEDataBase.DataType.Enemys);
        HP -= damage;
        if (HP <= 0) base.Dead(gameObject);
    }
}

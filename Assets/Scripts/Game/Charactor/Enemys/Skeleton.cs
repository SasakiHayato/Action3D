using UnityEngine;
using NewAttacks;

public class Skeleton : EnemyBase, IDamage, IFieldEnemy
{
    NewAttackSettings _attack;
    
    void Start()
    {
        _attack = GetComponent<NewAttackSettings>();
    }

    void Update()
    {
        State.Base();

        Vector3 set = Vector3.Scale(MoveDir * Speed, PhsicsBase.GetVelocity);
        if(State.Move != Vector3.zero) set = State.Move;

        Character.Move(set * Time.deltaTime);
    }

    public void GetDamage(int damage, AttackType type)
    {
        State.ChangeState(StateMachine.StateType.KnockBack);
        
        Sounds.SoundMaster.PlayRequest(transform, "Damage", Sounds.SEDataBase.DataType.Enemys);
        
        _attack.Cancel();

        object[] setUiData = { damage, gameObject, ColorType.Enemy };
        BaseUI.Instance.CallBack("Game", "Damage", setUiData);

        HP -= damage;
        if (HP < 0) base.Dead(gameObject);
    }
}

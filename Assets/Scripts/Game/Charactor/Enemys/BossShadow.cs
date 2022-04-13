using UnityEngine;

/// <summary>
/// BossÇÃå∂âeÇÃêßå‰ÉNÉâÉX
/// </summary>

public class BossShadow : EnemyBase, IDamage
{
    GameObject _player;

    void Start()
    {
        _player = GameObject.FindWithTag("Player");

        BaseState.SetUp(gameObject)
            .AddState(State.BehaviorTree, "Tree")
            .AddState(State.KnockBack, "Knock")
            .RunRequest(State.BehaviorTree);
    }

    void Update()
    {
        BaseState.Update();

        Rotate();
        Vector3 move = MoveDir;
        
        Vector3 set = Vector3.Scale(move * Speed, PhsicsBase.Gravity);
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
        object[] setUiData = { damage, gameObject, ColorType.Enemy };
        BaseUI.Instance.CallBack("Game", "Damage", setUiData);
        Sounds.SoundMaster.PlayRequest(transform, "Damage", Sounds.SEDataBase.DataType.Enemys);
        HP -= damage;
        if (HP < 0) base.Dead(gameObject);
    }
}

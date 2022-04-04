using UnityEngine;
using NewAttacks;

public class Boss : EnemyBase, IDamage, IFieldEnemy
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
        Rotate();
        BaseState.Update();
        
        Vector3 set = Vector3.Scale(MoveDir * Speed, PhsicsBase.Gravity);
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
        if (type == AttackType.Bullets)
        {
            Vector3 mPos = transform.localPosition;
            int x = Random.Range(-5, 5);
            int z = Random.Range(-5, 5);
            Character.ChangeLocalPos(new Vector3(mPos.x + x, mPos.y, mPos.z + z), gameObject);
            if (GameManager.Instance.IsLockOn) BaseUI.Instance.CallBack("Game", "Log", new object[] { 2 });
            GameManager.Instance.IsLockOn = false;
            return;
        }

        BaseState.ChangeState(State.KnockBack);

        Sounds.SoundMaster.PlayRequest(transform, "Damage", Sounds.SEDataBase.DataType.Enemys);
        object[] setUiData = { damage, gameObject, ColorType.Enemy };
        BaseUI.Instance.CallBack("Game", "Damage", setUiData);

        HP -= damage;
        if (HP < 0) base.Dead(gameObject);
    }
}

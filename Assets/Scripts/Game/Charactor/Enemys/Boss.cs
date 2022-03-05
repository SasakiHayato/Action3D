using UnityEngine;
using AttackSetting;

public class Boss : EnemyBase, IDamage, IFieldEnemy
{
    bool _isBackKnock = false;
    GameObject _player;

    Animator _anim;
    AttackSettings _attack;
    
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _anim = GetComponent<Animator>();
        _attack = GetComponent<AttackSettings>();
        Tree.SetUp();
    }

    void Update()
    {
        SetKnockBack(ref _isBackKnock);
        Tree.Run();
        Rotate();
        
        Vector3 set = Vector3.Scale(MoveDir * Speed, PhsicsBase.GetVelocity);
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

        if (_attack.EndCurrentAnim) _anim.Play("Damage_Back_Small_ver_B");
        else _isBackKnock = false;
        Sounds.SoundMaster.PlayRequest(transform, "Damage", Sounds.SEDataBase.DataType.Enemys);
        object[] setUiData = { damage, gameObject, ColorType.Enemy };
        BaseUI.Instance.CallBack("Game", "Damage", setUiData);

        HP -= damage;
        if (HP < 0) base.Dead(gameObject);
    }

    public override void KnockBack(Vector3 dir)
    {
        _isBackKnock = true;
        MoveDir = dir;
    }
}

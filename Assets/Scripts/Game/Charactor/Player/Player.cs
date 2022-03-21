using UnityEngine;
using System.Linq;
using NewAttacks;

/// <summary>
/// PlayerÇÃä«óùÉNÉâÉX
/// </summary>

public class Player : CharaBase, IDamage
{
    public enum State
    {
        Idle,
        Move,
        Float,
        Attack,
        Avoid,
        KnockBack
    }

    [SerializeField] float _lockOnDist;
    [SerializeField] Transform _muzzle;
    [SerializeField] float _shotCoolTime;
    [SerializeField] float _modeChangeTime;

    AttackSettings _settings;
    public PlayerAnimController AnimController { get; private set; }
    
    float _shotTimer = 0;
    public Vector3 Move { get; set; } = Vector3.zero;

    public bool IsAvoid { get; private set; } = false;
    public Vector3 GetKnockDir { get; private set; } = Vector3.zero;

    void Start()
    {
        GameManager.Instance.PlayerData.Player = this;
        GameManager.Instance.PlayerData.HP = HP;
        GameManager.Instance.PlayerData.Power = Power;

        AnimController = GetComponent<PlayerAnimController>();
        _settings = GetComponent<AttackSettings>();
        
        Inputter.Instance.Inputs.Player
            .Fire.started += context => Avoid();

        Inputter.Instance.Inputs.Player
            .Jump.started += context => Jump();

        Inputter.Instance.Inputs.Player
            .WeakAttack.started += context => WeakAttack();

        Inputter.Instance.Inputs.Player
            .StrengthAttack.started += context => StrengthAttack();

        Inputter.Instance.Inputs.Player
            .RockOn.started += context => SetLockon();

        BaseState.SetUp(gameObject)
            .AddState(State.Idle, "Idle")
            .AddState(State.Move, "Move")
            .AddState(State.Avoid, "Avoid")
            .AddState(State.Float, "Float")
            .AddState(State.KnockBack, "Knock")
            .AddState(State.Attack, "Attack")
            .RunRequest(State.Idle);
    }

    void Update()
    {
        if (!GameManager.Instance.PlayerData.CanMove) return;

        BaseState.Update();
        if (BaseState.CurrentStateType != State.Avoid.ToString()) IsAvoid = false;

        Shot();
       
        Vector3 set = Vector3.Scale(Move * Speed, PhsicsBase.Gravity);
        
        Character.Move(set * Time.deltaTime);
    }

    void Shot()
    {
        float value = (float)Inputter.GetValue(InputType.ShotVal);
        if ((int)value == 1)
        {
            _shotTimer += Time.deltaTime;
            if (_shotTimer > _shotCoolTime)
            {
                _shotTimer = 0;
                BulletShot();
            }
        }
        else
            _shotTimer = 0;
    }

    void BulletShot()
    {
        var getObj = BulletSettings.UseRequest(2);
        getObj.transform.position = _muzzle.position;
        Sounds.SoundMaster.PlayRequest(_muzzle, "ShotBullet", 0);

        if (GameManager.Instance.IsLockOn)
        {
            GameObject t = GameManager.Instance.LockonTarget;
            if (t == null) return;
            else getObj.GetComponent<Bullet>().ShotHoming(t, 3 * 10, Bullet.Parent.Player);
        }
        else
        {
            Vector3 dir = Camera.main.transform.forward;
            if (dir.y < 0) dir.y = 0;
            getObj.GetComponent<Bullet>().Shot(dir, 3 * 10, Bullet.Parent.Player);
        }
    }

    void Avoid()
    {
        if (BaseState.CurrentStateType == State.KnockBack.ToString()) return;
        if (GameManager.Option.Close != GameManager.Instance.OptionState) return;
        _settings.Cancel();
        BaseState.ChangeState(State.Avoid);
    }

    void Jump()
    {
        if (BaseState.CurrentStateType == State.KnockBack.ToString()) return;
        if (GameManager.Option.Close != GameManager.Instance.OptionState) return;

        if (BaseState.CurrentStateType == State.Float.ToString()) BaseState.ReturnEntry();
        else BaseState.ChangeState(State.Float);

        _settings.Cancel();
        PhsicsBase.Force(PhysicsBase.ForceType.Jump);
    }

    void WeakAttack()
    {
        if (_settings.ReadAttackType == NewAttacks.AttackType.Counter) return;
        if (BaseState.CurrentStateType == State.KnockBack.ToString()) return;
        if (GameManager.Option.Close != GameManager.Instance.OptionState) return;

        _settings.SetAttackType = NewAttacks.AttackType.Weak;
        _settings.SetNextRequest();
        BaseState.ChangeState(State.Attack);
    }

    void StrengthAttack()
    {
        if (_settings.ReadAttackType == NewAttacks.AttackType.Counter) return;
        if (BaseState.CurrentStateType == State.KnockBack.ToString()) return;
        if (GameManager.Option.Close != GameManager.Instance.OptionState) return;

        _settings.SetAttackType = NewAttacks.AttackType.Strength;
        _settings.SetNextRequest();
        BaseState.ChangeState(State.Attack);
    }

    void SetLockon()
    {
        if (GameManager.Option.Close != GameManager.Instance.OptionState) return;

        if (!GameManager.Instance.IsLockOn)
        {
            var finds = GameObject.FindGameObjectsWithTag("Enemy")
                .Where(e => 
                {
                      float dist = Vector3.Distance(e.transform.position, transform.position);
                      if (dist < _lockOnDist) return e;
                      else return false;
                });
            
            if (finds.Count() <= 0) return;
            GameManager.Instance.IsLockOn = true;

            Vector3 cmForwrad = Camera.main.transform.forward;
            GameObject set = null;
            float saveAngle = float.MinValue;
            foreach (var e in finds)
            {
                Vector3 dir = e.transform.position - cmForwrad;
                float rad = Mathf.Abs(Vector3.Dot(cmForwrad, dir.normalized));
                float angle = Mathf.Acos(rad) * Mathf.Rad2Deg;
                if (saveAngle < angle)
                {
                    CharaBase chara = e.GetComponent<CharaBase>();
                    if (chara != null)
                    {
                        set = e.GetComponent<CharaBase>().OffSetPosObj;
                        saveAngle = angle;
                    }
                }
            }

            GameManager.Instance.LockonTarget = set;
            BaseUI.Instance.CallBack("Player", "Lockon", new object[] { set });
        }
        else
            GameManager.Instance.IsLockOn = false;
    }

    public void GetDamage(int damage, AttackType type)
    {
        if (BaseState.CurrentStateType == State.Avoid.ToString())
        {
            if (IsAvoid) return;
            IsAvoid = true;

            BaseUI.Instance.CallBack("Player", "Avoid");
            FieldManager.FieldTimeRate(() => BaseUI.Instance.CallBack("Player", "Avoid"));
            
            return;
        }

        if (_settings.ReadAttackType == NewAttacks.AttackType.Counter) return;
        if (type == AttackType.None) return;

        Sounds.SoundMaster.PlayRequest(transform, "Damage", 0);

        object[] datas = { damage, gameObject, ColorType.Player };
        BaseUI.Instance.CallBack("Game", "Damage", datas);

        HP -= damage;
        if (HP <= 0)
        {
            GameManager.Instance.End();
            GameManager.Instance.GameStateSetUpEvents(GameManager.GameState.Dead);
        }
    }

    public void KnockBack(Vector3 dir)
    {
        if (BaseState.CurrentStateType == State.KnockBack.ToString()) return;
        if (BaseState.CurrentStateType == State.Avoid.ToString()) return;
        if (_settings.ReadAttackType == NewAttacks.AttackType.Counter) return;

        GetKnockDir = dir;
        BaseState.ChangeState(State.KnockBack);
    }

    public override void SetParam(int hp, int power, float speed, int level)
    {
        base.SetParam(hp, power, speed, level);
        GameManager.Instance.PlayerData.HP = HP;
        GameManager.Instance.PlayerData.Power = Power;
    }
}
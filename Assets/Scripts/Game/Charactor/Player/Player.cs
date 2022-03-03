using System.Collections;
using UnityEngine;
using System.Linq;
using System;
using AttackSetting;

/// <summary>
/// PlayerÇÃä«óùÉNÉâÉX
/// </summary>

public class Player : CharaBase, IDamage
{
    [SerializeField] float _lockOnDist;
    [SerializeField] Transform _muzzle;
    [SerializeField] float _shotCoolTime;
    [SerializeField] float _modeChangeTime;

    StateMachine _state;
    Animator _anim;
    AttackSettings _attack;

    float _shotTimer = 0;
    
    public bool EndAnim { get; private set; } = true;
    public bool IsAvoid { get; private set; } = false;
    public bool IsLockon { get; private set; } = false;
    public Vector3 GetKnockDir { get; private set; } = Vector3.zero;

    void Start()
    {
        GameManager.Instance.PlayerData.Player = this;
        GameManager.Instance.PlayerData.HP = HP;
        GameManager.Instance.PlayerData.Power = Power;

        _state = GetComponent<StateMachine>();
        _anim = GetComponent<Animator>();
        _attack = GetComponent<AttackSettings>();

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
    }

    void Update()
    {
        if (!GameManager.Instance.PlayerData.CanMove) return;

        _state.Base();
        if (_state.GetCurrentState != StateMachine.StateType.Avoid) IsAvoid = false;

        Shot();
       
        Vector3 set = Vector3.Scale(_state.Move * Speed, PhsicsBase.GetVelocity);
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
        if (_state.GetCurrentState == StateMachine.StateType.KnockBack) return;
        _state.ChangeState(StateMachine.StateType.Avoid);
    }

    void Jump()
    {
        if (_state.GetCurrentState == StateMachine.StateType.KnockBack) return;
        _state.ChangeState(StateMachine.StateType.Floating);
        PhsicsBase.SetJump();
    }

    void WeakAttack()
    {
        if (_attack.IsCounter) return;
        if (_state.GetCurrentState == StateMachine.StateType.KnockBack) return;
        _attack.SetAction = ActionType.WeakGround;
        _attack.NextRequest();
        _state.ChangeState(StateMachine.StateType.Attack);
    }

    void StrengthAttack()
    {
        if (_attack.IsCounter) return;
        if (_state.GetCurrentState == StateMachine.StateType.KnockBack) return;
        _attack.SetAction = ActionType.StrengthGround;
        _attack.NextRequest();
        _state.ChangeState(StateMachine.StateType.Attack);
    }

    void SetLockon()
    {
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
                Vector3 dir = transform.position - e.transform.position;
                float angle = Mathf.Abs(Vector3.Dot(cmForwrad, dir.normalized) * Mathf.Rad2Deg);
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
            UIManager.CallBack(UIType.Player, 2, new object[] { set });
        }
        else
            GameManager.Instance.IsLockOn = false;
    }

    public void GetDamage(int damage, AttackType type)
    {
        if (_state.GetCurrentState == StateMachine.StateType.Avoid)
        {
            if (IsAvoid) return;
            IsAvoid = true;

            UIManager.CallBack(UIType.Player, 1);
            FieldManager.FieldTimeRate(UIManager.CallBack, UIType.Player, 1);
            
            return;
        }
        
        if (_attack.IsCounter) return;
        if (type == AttackType.None) return;

        Sounds.SoundMaster.PlayRequest(transform, "Damage", 0);

        object[] datas = { damage, gameObject, ColorType.Player };
        UIManager.CallBack(UIType.Game, 4, datas);
        
        HP -= damage;
        if (HP <= 0)
        {
            GameManager.Instance.End();
            GameManager.Instance.GameStateSetUpEvents(GameManager.GameState.Dead);
        }
    }

    public void KnockBack(Vector3 dir)
    {
        if (_state.GetCurrentState == StateMachine.StateType.KnockBack) return;
        if (_state.GetCurrentState == StateMachine.StateType.Avoid) return;
        if (_attack.IsCounter) return;

        GetKnockDir = dir;
        _state.ChangeState(StateMachine.StateType.KnockBack);
    }

    public void SetAnim(string name, Action action = null)
    {
        if (!EndAnim) return;

        EndAnim = false;
        _anim.Play(name);
        StartCoroutine(WaitAnim(action));
    }

    IEnumerator WaitAnim(Action action)
    {
        yield return null;
        yield return new WaitAnim(_anim);
        EndAnim = true;
        if (action != null) action.Invoke();
    }

    public override void SetParam(int hp, int power, float speed, int level)
    {
        base.SetParam(hp, power, speed, level);
        GameManager.Instance.PlayerData.HP = HP;
        GameManager.Instance.PlayerData.Power = Power;
    }
}
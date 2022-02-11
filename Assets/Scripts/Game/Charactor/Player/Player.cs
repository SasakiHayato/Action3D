using System.Collections;
using UnityEngine;
using System.Linq;
using System;
using AttackSetting;

public class Player : CharaBase, IDamage
{
    [SerializeField] float _lockOnDist;
    [SerializeField] Transform _muzzle;
    [SerializeField] float _shotCoolTime;

    StateMachine _state;
    Animator _anim;
    AttackSettings _attack;

    float _timer = 0;

    bool _isAvoid = false;
    public bool IsAvoid => _isAvoid;

    bool _isLockon = false;
    public bool IsLockon => _isLockon;

    public Vector3 GetKnockDir { get; private set; }

    public bool EndAnim { get; private set; } = true;

    void Start()
    {
        GameManager.Instance.PlayerData.Player = this;
        GameManager.Instance.PlayerData.HP = HP;
        GameManager.Instance.PlayerData.Power = Power;

        _state = GetComponent<StateMachine>();
        _anim = GetComponent<Animator>();
        _attack = GetComponent<AttackSettings>();

        Inputter.Instance.Inputs.Player
            .Fire.started += context
            => _state.ChangeState(StateMachine.StateType.Avoid);

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
        _state.Base();
        if (_state.GetCurrentState != StateMachine.StateType.Avoid) _isAvoid = false;

        float value = (float)Inputter.GetValue(InputType.ShotVal);
        if ((int)value == 1)
        {
            _timer += Time.deltaTime;
            if (_timer > _shotCoolTime)
            {
                _timer = 0;
                BulletShot();
            }
        }
        else
        {
            _timer = 0;
        }

        Vector3 set = Vector3.Scale(_state.Move * Speed, PhsicsBase.GetVelocity);
        Character.Move(set * Time.deltaTime);
    }

    void BulletShot()
    {
        var getObj = BulletSettings.UseRequest(2);
        getObj.transform.position = _muzzle.position;
        Sounds.SoundMaster.Request(_muzzle, "ShotBullet", 0);

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

    void Jump()
    {
        _state.ChangeState(StateMachine.StateType.Floating);
        PhsicsBase.SetJump();
    }

    void WeakAttack()
    {
        if (_attack.IsCounter) return;
        _attack.SetAction = ActionType.WeakGround;
        _attack.NextRequest();
        _state.ChangeState(StateMachine.StateType.Attack);
    }

    void StrengthAttack()
    {
        if (_attack.IsCounter) return;
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
                    set = e.GetComponentInChildren<TargetCorrector>().gameObject;
                    saveAngle = angle;
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
            if (_isAvoid) return;
            _isAvoid = true;

            UIManager.CallBack(UIType.Player, 1);
            FieldManager.FieldTimeRate(UIManager.CallBack, UIType.Player, 1);
            
            return;
        }

        if (_attack.IsCounter) return;
        if (type == AttackType.None) return;

        Sounds.SoundMaster.Request(transform, "Damage", 0);

        HP -= damage;
        if (HP <= 0)
        {
            Destroy(gameObject);
            GameManager.Instance.End();
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
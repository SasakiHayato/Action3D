using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase, IDamage
{
    bool _isBackKnock = false;

    StateMachine _state;
    GameObject _player;
    
    void Start()
    {
        _state = GetComponent<StateMachine>();
        _player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        SetKnockBack(ref _isBackKnock);
        Tree.Repeat();
        Rotate();
        Vector3 move = Vector3.zero;

        if (_state.IsRunning) move = _state.Move;
        else move = MoveDir;
        
        Vector3 set = Vector3.Scale(move * Speed, PhsicsBase.GetVelocity);
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
            if (GameManager.Instance.IsLockOn) UIManager.CallBack(UIType.Game, 3, new object[] { 2 });
            GameManager.Instance.IsLockOn = false;
            return;
        }
        
        HP -= damage;
        if (HP < 0) base.Dead(gameObject);
    }

    public override void KnockBack(Vector3 dir)
    {
        _isBackKnock = true;
        MoveDir = dir;
    }
}

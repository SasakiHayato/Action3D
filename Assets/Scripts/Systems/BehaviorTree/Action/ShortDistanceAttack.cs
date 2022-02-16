using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAI;
using AttackSetting;

public class ShortDistanceAttack : IAction
{
    [SerializeField] ActionType _type = ActionType.WeakGround;

    EnemyBase _enemyBase = null;
    AttackSettings _attack = null;
    GameObject _player = null;

    public GameObject Target { get; set; } 

    public void SetUp()
    {
        if (_enemyBase == null)
        {
            _enemyBase = Target.GetComponent<EnemyBase>();
            _attack = Target.GetComponent<AttackSettings>();
            _player = GameObject.FindWithTag("Player");
        }
    }

    public bool Execute()
    {
        _enemyBase.MoveDir = Vector3.zero;
        _attack.NextRequest();
        Rotate();
        if (_attack.EndCurrentAnim)
        {
            if (_attack.IsNextRequest)
            {
                _attack.Request(_type);
                return true;
            }

            _attack.Cancel();
            _attack.Request(_type);
            return  true;
        }
        
        return false;
    }

    void Rotate()
    {
        Vector3 diff = _player.transform.position - Target.transform.position;
        diff.y = 0;
        Target.transform.localRotation = Quaternion.LookRotation(diff);
    }
}

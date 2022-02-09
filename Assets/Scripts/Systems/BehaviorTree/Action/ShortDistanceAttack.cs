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

    public GameObject Target { get; set; } 

    public void SetUp()
    {
        if (_enemyBase == null)
        {
            _enemyBase = Target.GetComponent<EnemyBase>();
            _attack = Target.GetComponent<AttackSettings>();
        }
    }

    public bool Execute()
    {
        _enemyBase.MoveDir = Vector3.zero;
        _attack.NextRequest();

        if (_attack.EndCurrentAnim)
        {
            if (_attack.IsNextRequest)
            {
                _attack.Request(_type);
                return true;
            }

            _attack.Cansel();
            _attack.Request(_type);
            return  true;
        }
        
        return false;
    }
}

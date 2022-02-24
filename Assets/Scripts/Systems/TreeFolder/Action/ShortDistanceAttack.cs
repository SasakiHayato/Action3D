using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using AttackSetting;

public class ShortDistanceAttack : IAction
{
    [SerializeField] ActionType _type = ActionType.WeakGround;

    EnemyBase _enemyBase = null;
    AttackSettings _attack = null;
    GameObject _player = null;

    GameObject _user;

    public void SetUp(GameObject user)
    {
        _user = user;
        _enemyBase = user.GetComponent<EnemyBase>();
        _attack = user.GetComponent<AttackSettings>();
        _player = GameObject.FindWithTag("Player");
    }

    public bool Execute()
    {
        _enemyBase.MoveDir = Vector3.zero;
        
        Rotate();
        if (_attack.EndCurrentAnim)
        {
            if (_attack.IsNextRequest)
            {
                _attack.Request(_type);
                //Debug.Log("IsNext");
                return true;
            }
            //Debug.Log("Request");
            _attack.Cancel();
            _attack.Request(_type);
            return  true;
        }
        else
        {
            _attack.NextRequest();
        }

        return false;
    }

    void Rotate()
    {
        Vector3 diff = _player.transform.position - _user.transform.position;
        diff.y = 0;
        _user.transform.localRotation = Quaternion.LookRotation(diff);
    }

    public void InitParam()
    {

    }
}

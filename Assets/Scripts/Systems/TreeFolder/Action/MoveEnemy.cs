using UnityEngine;
using BehaviourTree;
using NewAttacks;

public class MoveEnemy : IAction
{
    enum MoveType
    {
        ToWard,
        Back,
        Side,
    }

    [SerializeField] string _animName;
    [SerializeField] bool _applyYPos;
    [SerializeField] MoveType _moveType;
    [SerializeField] float _speedRate = 1;

    Animator _anim = null;
    EnemyBase _enemyBase = null;
    AttackSettings _attack;

    GameObject _player = null;
    GameObject _user;

    public void SetUp(GameObject user)
    {
        _user = user;
        _enemyBase = user.GetComponent<EnemyBase>();
        _attack = user.GetComponent<AttackSettings>();
        _anim = user.gameObject.GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player");
    }

    public bool Execute()
    {
        Vector3 dir = Vector3.zero;

        switch (_moveType)
        {
            case MoveType.ToWard:
                dir = (_player.transform.position - _user.transform.position).normalized / _speedRate;
                break;
            case MoveType.Back:
                dir = (_user.transform.position - _player.transform.position).normalized / _speedRate;
                dir.y /= 3;
                break;
            case MoveType.Side:
                dir = _user.transform.right / _speedRate;
                break;
        }

        if (!_applyYPos) dir.y = 1;
        if (_animName != "") _anim.Play(_animName);
        
        _enemyBase.MoveDir = dir;
        LookPlayer(dir);

        return true;
    }

    void LookPlayer(Vector3 dir)
    {
        Vector3 forward = new Vector3(dir.x, 0, dir.z);
        _user.transform.rotation = Quaternion.LookRotation(forward);
    }

    public void InitParam()
    {

    }
}

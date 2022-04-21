using UnityEngine;
using BehaviourTree;

/// <summary>
/// Carmeraì‡Ç÷à⁄ìÆÇ∑ÇÈAIçsìÆ
/// </summary>

public class MoveInCamera : IAction
{
    [SerializeField] TreeManager.RunType _runType;
    [SerializeField] float _offsetAngle;
    [SerializeField] Vector3 _offSetPos;
    [SerializeField] float _setDistance;

    Transform _user;
    Transform _cmPos;
    EnemyBase _enemyBase;
    GameObject _player;

    public void SetUp(GameObject user)
    {
        _user = user.transform;
        _enemyBase = user.GetComponent<EnemyBase>();
        _player = GameManager.Instance.PlayerData.Player.gameObject;
        _cmPos = Camera.main.transform;
    }

    public bool Execute()
    {
        Vector3 dir = _user.position - Camera.main.transform.position;
        float rad = Vector3.Dot(dir.normalized, Camera.main.transform.forward);
        float angle = Mathf.Acos(rad) * Mathf.Rad2Deg;

        Vector3 setPos = (_cmPos.position + _cmPos.forward * _setDistance) + _offSetPos;

        if (setPos.y < _player.transform.position.y) setPos.y = _player.transform.position.y + _offSetPos.y;

        _enemyBase.MoveDir = (setPos - _user.position).normalized;
        
        if (_runType == TreeManager.RunType.Task)
        {
            if (_offsetAngle > angle) return true;
            else return false;
        }
        else
        {
            return true;
        }
    }

    public void InitParam()
    {

    }
}

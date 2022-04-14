using UnityEngine;
using BehaviourTree;

/// <summary>
/// PlayerÇ…èuä‘à⁄ìÆÇ∑ÇÈAIçsìÆ
/// </summary>

public class MomentMove : IAction
{
    [SerializeField] float _distanceRate = 1;
    [SerializeField] bool _apllayPosY;
    
    GameObject _player = null;
    CharaBase _charaBase;

    GameObject _user;

    public void SetUp(GameObject user)
    {
        _user = user;
        _player = GameObject.FindWithTag("Player");
        _charaBase = user.GetComponent<CharaBase>();
    }

    public bool Execute()
    {
        Vector3 setPos = _player.GetComponent<CharaBase>().OffSetPosObj.transform.position;
        if (!_apllayPosY) setPos.y = _user.transform.position.y;
        Vector3 forward = _user.transform.forward;
        forward.y = 0;
        _charaBase.Character.ChangePos(setPos - forward * _distanceRate, _user);
        return true;
    }

    public void InitParam()
    {

    }
}

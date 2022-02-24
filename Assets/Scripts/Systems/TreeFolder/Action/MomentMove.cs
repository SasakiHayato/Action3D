using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class MomentMove : IAction
{
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
        Vector3 setPos = _player.GetComponentInChildren<TargetCorrector>().gameObject.transform.position;
        if (!_apllayPosY) setPos.y = _user.transform.position.y;
        Vector3 forward = _user.transform.forward;
        forward.y = 0;
        _charaBase.Character.ChangePos(setPos - forward * 3, _user);
        return true;
    }

    public void InitParam()
    {

    }
}

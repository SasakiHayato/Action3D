using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAI;

public class MomentMove : IAction
{
    [SerializeField] bool _apllayPosY;
    public GameObject Target { get; set; }

    GameObject _player = null;
    CharaBase _charaBase;

    public void SetUp()
    {
        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player");
            _charaBase = Target.GetComponent<CharaBase>();
        }
    }

    public bool Execute()
    {
        Vector3 setPos = _player.GetComponentInChildren<TargetCorrector>().gameObject.transform.position;
        if (!_apllayPosY) setPos.y = Target.transform.position.y;
        Vector3 forward = Target.transform.forward;
        forward.y = 0;
        _charaBase.Character.ChangePos(setPos - forward * 3f, Target);
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAI;

public class MomentMove : IAction
{
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
        Vector3 setPos = _player.transform.position;
        _charaBase.Character.ChangePos(setPos, Target);
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewBehaviourTree;

public class NewFind : IConditional
{
    [SerializeField] float _distance;
    GameObject _player = null;

    public bool Check()
    {
        if (_player == null) _player = GameObject.FindWithTag("Player");

        float dist = Vector3.Distance(Target.transform.position, _player.transform.position);
        if (dist < _distance) return true;
        else return false;
    }

    public GameObject Target { get; set; }
}

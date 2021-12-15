using UnityEngine;

using BehaviorAI;

public class FindDistance : IConditional
{
    [SerializeField] float _range;

    GameObject _player = null;
    bool _check = false;
    
    public bool Check()
    {
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");

        float distance = Vector3.Distance(Target.transform.position, _player.transform.position);
        if (distance < _range) _check = true;
        else _check = false;

        return _check;
    }

    public GameObject Target { private get; set; }
}

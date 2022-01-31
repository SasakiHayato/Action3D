using UnityEngine;

using BehaviourAI;

public class FindDistance : IConditional
{
    [SerializeField] float _range;

    GameObject _player = null;
    
    public bool Check()
    {
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");

        float distance = Vector3.Distance(Target.transform.position, _player.transform.position);
        if (distance < _range) return true;
        else return false;
    }

    public GameObject Target { get; set; }
}

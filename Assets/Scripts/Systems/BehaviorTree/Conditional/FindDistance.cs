using UnityEngine;

using BehaviourAI;

public class FindDistance : IConditional
{
    enum Range
    {
        In,
        Out
    }

    [SerializeField] float _range;
    [SerializeField] Range _type;

    GameObject _player = null;
    
    public bool Check()
    {
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");

        float distance = Vector3.Distance(Target.transform.position, _player.transform.position);
        switch (_type)
        {
            case Range.In:

                if (distance < _range) return true;
                else return false;
            case Range.Out:

                if (distance > _range) return true;
                else return false;

            default: return false;
        }
    }

    public GameObject Target { get; set; }
}

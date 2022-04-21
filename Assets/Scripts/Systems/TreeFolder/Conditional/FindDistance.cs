using UnityEngine;
using BehaviourTree;

/// <summary>
/// Player‚Æ‚Ì‹——£‚ğ’²‚×‚éğŒ•ªŠò
/// </summary>

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
    GameObject _user;
    
    public void SetUp(GameObject user)
    {
        _user = user;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public bool Try()
    {
        float distance = Vector3.Distance(_user.transform.position, _player.transform.position);
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

    public void InitParam()
    {

    }
}

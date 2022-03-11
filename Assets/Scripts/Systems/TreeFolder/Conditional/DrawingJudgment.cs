using UnityEngine;
using BehaviourTree;

public class DrawingJudgment : IConditional
{
    enum ReturnType
    {
        In,
        Out,
    }

    [SerializeField] ReturnType _returnType;

    Transform _user;
    float _fovAngle = 0;

    public void SetUp(GameObject user)
    {
        _fovAngle = Camera.main.fieldOfView / 2;
        _user = user.transform;
    }

    public bool Try()
    {
        Vector3 dir = _user.position - Camera.main.transform.position;
        float rad = Vector3.Dot(dir.normalized, Camera.main.transform.forward);
        float angle = Mathf.Acos(rad) * Mathf.Rad2Deg;
        
        if (_returnType == ReturnType.In)
        {
            if (_fovAngle > angle) return true;
            else return false;
        }
        else
        {
            if (_fovAngle > angle) return false;
            else return true;
        }
    }

    public void InitParam()
    {

    }
}

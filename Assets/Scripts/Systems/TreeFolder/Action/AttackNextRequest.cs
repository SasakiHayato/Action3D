using UnityEngine;
using NewAttacks;
using BehaviourTree;

public class AttackNextRequest : IAction
{
    AttackSettings _settings;

    public void SetUp(GameObject user)
    {
        _settings = user.GetComponent<AttackSettings>();
    }

    public bool Execute()
    {
        _settings.SetNextRequest();
        return _settings.IsNextRequest;
    }

    public void InitParam()
    {

    }
}

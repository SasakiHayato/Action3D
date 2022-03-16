using UnityEngine;
using BehaviourTree;
using NewAttacks;

public class AttackCancel : IAction
{
    AttackSettings _attack;

    public void SetUp(GameObject user)
    {
        _attack = user.GetComponent<AttackSettings>();
    }

    public bool Execute()
    {
        _attack.Cancel();
        return true;
    }

    public void InitParam()
    {

    }
}

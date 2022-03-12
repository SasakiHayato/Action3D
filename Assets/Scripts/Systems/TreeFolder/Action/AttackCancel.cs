using UnityEngine;
using BehaviourTree;
using NewAttacks;

public class AttackCancel : IAction
{
    NewAttackSettings _attack;

    public void SetUp(GameObject user)
    {
        _attack = user.GetComponent<NewAttackSettings>();
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

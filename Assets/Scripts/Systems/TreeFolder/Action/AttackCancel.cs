using UnityEngine;
using BehaviourTree;
using NewAttacks;

/// <summary>
/// AttackSettings�R���|�[�l���g������Enemy�̍U�����L�����Z������AI�s��
/// </summary>

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

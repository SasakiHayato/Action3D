using NewAttacks;
using UnityEngine;
using BehaviourTree;

/// <summary>
/// AttackSettings�R���|�[�l���g������Enemy��Animation��҂�AI�s��
/// </summary>

public class WaitAttackAnim : IAction
{
    AttackSettings _settings;

    public void SetUp(GameObject user)
    {
        _settings = user.GetComponent<AttackSettings>();
    }

    public bool Execute()
    {
        return _settings.EndCurrentAnim;
    }

    public void InitParam()
    {
        
    }
}

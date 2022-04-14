using NewAttacks;
using UnityEngine;
using BehaviourTree;

/// <summary>
/// AttackSettingsコンポーネントをもつEnemyのAnimationを待つAI行動
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

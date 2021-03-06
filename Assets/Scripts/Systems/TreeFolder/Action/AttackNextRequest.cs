using UnityEngine;
using NewAttacks;
using BehaviourTree;

/// <summary>
/// AttackSettingsをもつEnemyの連続攻撃を申請するAI行動
/// </summary>

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

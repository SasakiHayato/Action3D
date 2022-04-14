using UnityEngine;
using BehaviourTree;
using NewAttacks;

/// <summary>
/// AttackSettingsコンポーネントをもつEnemyの攻撃をキャンセルするAI行動
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

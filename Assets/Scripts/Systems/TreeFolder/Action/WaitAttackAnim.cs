using NewAttacks;
using UnityEngine;
using BehaviourTree;

public class WaitAttackAnim : IAction
{
    AttackSettings _settings;

    public void SetUp(GameObject user)
    {
        _settings = user.GetComponent<AttackSettings>();
    }

    public bool Execute()
    {
        Debug.Log($"WaitAnim {_settings.EndCurrentAnim}");
        return _settings.EndCurrentAnim;
    }

    public void InitParam()
    {
        
    }
}

using UnityEngine;
using BehaviourTree;

/// <summary>
/// Debug—p
/// </summary>

public class Console : IConditional, IAction
{
    [SerializeField] string _cosole;
    [SerializeField] bool _activeSetUp;
    [SerializeField] bool _activeTry;
    [SerializeField] bool _activeExecute;
    [SerializeField] bool _activeInit;

    public void SetUp(GameObject user)
    {
        if (_activeSetUp) Debug.Log($"SetUp {_cosole}");
    }

    public bool Try()
    {
        if(_activeTry) Debug.Log($"Try {_cosole}");
        return true;
    }

    public bool Execute()
    {
        if (_activeExecute) Debug.Log($"Execute {_cosole}");
        return true;
    }

    public void InitParam()
    {
        if(_activeInit) Debug.Log($"InitParam {_cosole}");
    }
}

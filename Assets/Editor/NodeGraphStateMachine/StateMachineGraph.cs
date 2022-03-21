#if UNITY_EDITOR

using UnityEngine;
using GraphProcessor;
using UnityEditor;

[CreateAssetMenu(menuName = "StateMachineGraph")]
public class StateMachineGraph : BaseGraph
{
    public static bool OnBaseGraphOpend(int instanceID)
    {
        StateMachineGraph asset = EditorUtility.InstanceIDToObject(instanceID) as StateMachineGraph;
        
        if (asset == null)
        {
            return false;
        }
        else
        {
            var window = EditorWindow.GetWindow<StateMachineWindow>();
            window.InitializeGraph(asset);
            return true;
        }
    }
}

#endif

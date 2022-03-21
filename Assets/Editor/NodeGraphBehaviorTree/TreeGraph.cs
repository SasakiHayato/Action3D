# if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using GraphProcessor;

namespace BehaviourTree
{
    [CreateAssetMenu (menuName = "TreeGraph")]
    public class TreeGraph : BaseGraph
    {
        public static bool OnBaseGraphOpend(int instanceID)
        {
            TreeGraph asset = EditorUtility.InstanceIDToObject(instanceID) as TreeGraph;

            if (asset == null)
            {
                return false;
            }
            else
            {
                var window = EditorWindow.GetWindow<TreeWindow>();
                window.InitializeGraph(asset);

                return true;
            }
        }
    }
}

#endif

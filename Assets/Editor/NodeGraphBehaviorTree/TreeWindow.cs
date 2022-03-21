using UnityEngine;
using UnityEditor;
using GraphProcessor;
using UnityEngine.Assertions;
using System.IO;

namespace BehaviourTree
{
    public class TreeWindow : BaseGraphWindow
    {
        protected override void InitializeWindow(BaseGraph graph)
        {
            Assert.IsNotNull(graph);
            string fileName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(graph));
            titleContent = new GUIContent(ObjectNames.NicifyVariableName(fileName));

            if (graphView == null)
            {
                graphView = new BaseGraphView(this);
            }

            rootView.Add(graphView);
        }
    }

}
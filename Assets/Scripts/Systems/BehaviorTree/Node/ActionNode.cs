using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourAI
{
    public partial class BehaviourTree : MonoBehaviour
    {
        ActionNode _action;

        class ActionNode
        {
            int _actionCount;
            int _actionID = 0;

            public ActionNode(QueueData queue, GameObject t)
            {
                Debug.Log("ActionNode. Constract");
                _actionID = 0;
                _actionCount = queue.Actions.Count;
                queue.Actions.ForEach(a => a.Target = t);
                queue.Actions.ForEach(a => a.SetUp());
            }

            public void Set(QueueData queue, BehaviourTree tree)
            {
                Debug.Log("ActionNode. SetMethod");
                Debug.Log(queue.Actions[_actionID].GetType());
                if (queue.Actions[_actionID].Execute())
                {
                    Debug.Log("ActionNode. IsTrue *****************************");
                    _actionID++;
                    if (_actionCount == _actionID) tree._treeState = State.Check;
                }
            }

            public void Cansel(QueueData queue, BehaviourTree tree)
            {
                Debug.Log("ActionNode. CanselMethod");
                queue.Actions.ForEach(a => a.SetUp());
                tree._treeState = State.Check;
            }
        }
    }
}
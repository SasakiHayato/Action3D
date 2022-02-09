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
                _actionID = 0;
                _actionCount = queue.Actions.Count;
                queue.Actions.ForEach(a => a.Target = t);
                queue.Actions.ForEach(a => a.SetUp());
            }

            public void Set(QueueData queue, BehaviourTree tree)
            {
                if (queue.Actions[_actionID].Execute())
                {
                    _actionID++;
                    //Debug.Log($"ActionID {_actionID} ActionCount {_actionCount}");
                    if (_actionCount == _actionID) tree._treeState = State.Set;
                }
            }

            public void Cansel(QueueData queue, BehaviourTree tree)
            {
                queue.Actions.ForEach(a => a.SetUp());
                tree._treeState = State.Check;
            }
        }
    }
}
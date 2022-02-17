using UnityEngine;

/// <summary>
/// BeheviaTreeのActionを管理するクラス
/// </summary>

namespace BehaviourAI
{
    public partial class BehaviourTree : MonoBehaviour
    {
        ActionNode _action;

        class ActionNode
        {
            int _actionCount;
            int _actionID = 0;

            /// <summary>
            /// 初期化
            /// </summary>
            /// <param name="queue">走らせるQueueData</param>
            /// <param name="t">対象のObject</param>
            public ActionNode(QueueData queue, GameObject t)
            {
                _actionID = 0;
                _actionCount = queue.Actions.Count;
                queue.Actions.ForEach(a => a.Target = t);
                queue.Actions.ForEach(a => a.SetUp());
            }

            /// <summary>
            /// Actionの実行
            /// </summary>
            /// <param name="queue">行うQueueData</param>
            /// <param name="tree">BeheviaTreeの元</param>
            public void Set(QueueData queue, BehaviourTree tree)
            {
                if (queue.Actions[_actionID].Execute())
                {
                    _actionID++;
                    if (_actionCount == _actionID) tree.TreeState = State.Check;
                }
            }

            /// <summary>
            /// 実行しているActionのキャンセル
            /// </summary>
            /// <param name="queue">行われているQueueData</param>
            /// <param name="tree">BeheviaTree元</param>
            public void Cancel(QueueData queue, BehaviourTree tree)
            {
                queue.Actions.ForEach(a => a.SetUp());
                tree.TreeState = State.Check;
            }
        }
    }
}
using UnityEngine;

/// <summary>
/// BeheviaTree��Action���Ǘ�����N���X
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
            /// ������
            /// </summary>
            /// <param name="queue">���点��QueueData</param>
            /// <param name="t">�Ώۂ�Object</param>
            public ActionNode(QueueData queue, GameObject t)
            {
                _actionID = 0;
                _actionCount = queue.Actions.Count;
                queue.Actions.ForEach(a => a.Target = t);
                queue.Actions.ForEach(a => a.SetUp());
            }

            /// <summary>
            /// Action�̎��s
            /// </summary>
            /// <param name="queue">�s��QueueData</param>
            /// <param name="tree">BeheviaTree�̌�</param>
            public void Set(QueueData queue, BehaviourTree tree)
            {
                if (queue.Actions[_actionID].Execute())
                {
                    _actionID++;
                    if (_actionCount == _actionID) tree.TreeState = State.Check;
                }
            }

            /// <summary>
            /// ���s���Ă���Action�̃L�����Z��
            /// </summary>
            /// <param name="queue">�s���Ă���QueueData</param>
            /// <param name="tree">BeheviaTree��</param>
            public void Cancel(QueueData queue, BehaviourTree tree)
            {
                queue.Actions.ForEach(a => a.SetUp());
                tree.TreeState = State.Check;
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public partial class TreeManager : MonoBehaviour
    {
        ActionNode _actionNode;

        class ActionNode
        {
            int _executeID = 0;
            List<IAction> _actions = null;

            public void SetUp(QueueData queue, TreeManager manager)
            {
                if (queue.RunType == RunType.Task)
                {
                    _actions = queue.Actions;
                    manager.State = TreeState.Task;
                }
                else
                {
                    manager.State = TreeState.Run;
                }
            }

            /// <summary>
            /// アクションタイプがRunだった場合の実行判定
            /// </summary>
            /// <param name="actions"></param>
            /// <returns>実行中か否か</returns>
            public bool RunUpdate(List<IAction> actions)
            {
                if (actions.Count <= _executeID)
                {
                    _executeID = 0;
                    return false;
                }
                
                if (actions[_executeID].Execute())
                {
                    _executeID++;
                }
              
                return true;
            }

            /// <summary>
            /// アクションタイプがTaskだった場合の実行判定
            /// </summary>
            /// <returns>実行中か否か</returns>
            public bool RunTask()
            {
                if (_actions == null)
                {
                    return false;
                }

                if (_actions.Count <= _executeID)
                {
                    return false;
                }
                
                if (_actions[_executeID].Execute())
                {
                    _executeID++;
                }

                return true;
            }

            public void Init()
            {
                _actions = null;
                _executeID = 0;
            }
        }
    }
}
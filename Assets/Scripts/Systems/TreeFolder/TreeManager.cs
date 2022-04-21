using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BehaviourTree
{
    public interface IConditional
    {
        public void SetUp(GameObject user);
        public bool Try();
        public void InitParam();
    }

    public interface IAction
    {
        public void SetUp(GameObject user);
        public bool Execute();
        public void InitParam();
    }

    /// <summary>
    /// Tree全体の管理クラス
    /// </summary>

    public partial class TreeManager : MonoBehaviour
    {
        public enum TreeState
        {
            Run,
            Task,
            Set,
            End,
        }

        bool _isSetUp = false;
        int _branchID = 0;

        int _saveConditionalID = 0;

        public List<BranchData> ConditionallyBranches { get; private set; }
        public List<BranchData> NormalBranches { get; private set; }

        public TreeState State { get; set; } = TreeState.Run;

        BranchData _runBranch;

        public void SetUp()
        {
            ConditionallyBranches = new List<BranchData>();
            NormalBranches = new List<BranchData>();

            int id = 0;

            _branchDatas.ForEach(branch => 
            {
                branch.ID = id;
                id++;

                if (branch.BrockType == BrockType.ConditionallySelector 
                || branch.BrockType == BrockType.ConditionallySequence)
                    ConditionallyBranches.Add(branch);
                else
                    NormalBranches.Add(branch);

                branch.BranchConditionals.ForEach(b => b.SetUp(gameObject));

                branch.BrockDatas.ForEach(brock => 
                {
                    brock.QueueDatas.ForEach(queue => 
                    {
                        queue.Conditionals.ForEach(c => c.SetUp(gameObject));
                        queue.Actions.ForEach(a => a.SetUp(gameObject));
                    });
                });
            });

            ConditionallyBranches.OrderByDescending(b => b.Priority);
            
            NormalBranches.OrderByDescending(b => b.Priority);

            _sequenceNode = new SequenceNode();
            _selectorNode = new SelectorNode();
            _conditionalNode = new ConditionalNode();
            _actionNode = new ActionNode();

            _isSetUp = true;
        }

        public void TreeUpdate()
        {
            if (!_isSetUp)
            {
                Debug.Log("Not set up");
                return;
            }

            switch (State)
            {
                case TreeState.Run:

                    Run();
                    break;
                case TreeState.Task:

                    if (!_actionNode.RunTask())
                    {
                        State = TreeState.Set;
                    }

                    return;
                case TreeState.Set:
                    
                    _actionNode.Init();
                    _selectorNode.Init();

                    _branchDatas[_runBranch.ID].BrockDatas.ForEach(b =>
                    {
                        b.QueueDatas.ForEach(q =>
                        {
                            q.Conditionals.ForEach(c => c.InitParam());
                            q.Actions.ForEach(a => a.InitParam());
                        });
                    });
                    
                    if (!_sequenceNode.SetNextQueueData(_runBranch))
                    {
                        if (!_sequenceNode.SetNextBrockData(_runBranch))
                        {
                            State = TreeState.End;
                            return;
                        }
                        else
                        {
                            _sequenceNode.InitQueueID();
                            State = TreeState.Run;
                            return;
                        }
                    }
                    else
                    {
                        State = TreeState.Run;   
                    }

                    return;
                case TreeState.End:

                    _branchDatas[_runBranch.ID].BrockDatas.ForEach(b =>
                    {
                        b.QueueDatas.ForEach(q => 
                        {
                            q.Conditionals.ForEach(c => c.InitParam());
                            q.Actions.ForEach(a => a.InitParam());
                        });
                    });

                    _sequenceNode.Init();
                    _actionNode.Init();
                    _selectorNode.Init();
                    _branchID++;
                    
                    State = TreeState.Run;

                    return;       
            }
        }

        void Run()
        {
            //　ブランチの取得
            BranchData branch = _conditionalNode.SetBranch(this, _branchID);

            if (branch == null)
            {
                InitParam();
                return;
            }
            else
            {
                _runBranch = branch;
            }

            if (branch.ID != _saveConditionalID)
            {
                _sequenceNode.Init();
                _selectorNode.Init();
                _actionNode.Init();
                _saveConditionalID = branch.ID;
            }

            BlockData blockData = null;
            QueueData queueData = null;

            //　ブロックの取得
            if (branch.BrockType == BrockType.Sequence
            || branch.BrockType == BrockType.ConditionallySequence)
            {
                blockData = _sequenceNode.SetBrockData(branch.BrockDatas);
            }
            else
            {
                blockData = _selectorNode.SetBrockData(branch.BrockDatas);
            }

            if (blockData == null)
            {
                State = TreeState.Set;
                return;
            }

            //　キューの取得
            if (blockData.QueueType == QueueType.Sequence)
            {
                queueData = _sequenceNode.SetQueueData(blockData.QueueDatas);
            }
            else
            {
                queueData = _selectorNode.SetQueueData(blockData.QueueDatas);
            }


            if (queueData == null)
            {
                State = TreeState.Set;
                return;
            }

            bool isAction = false;

            // キューデータのコンディションタイプを調べ成否を返す
            if (queueData.Condition == ConditionalType.Sequence)
                isAction = _conditionalNode.Sequence(queueData.Conditionals);
            else
                isAction = _conditionalNode.Selector(queueData.Conditionals);

            // アクションのタイプを調べ初期化
            _actionNode.SetUp(queueData, this);

            //　現在のキューデータの実行判定
            if (State == TreeState.Task)
            {
                if (!isAction)
                {
                    State = TreeState.Set;
                }
            }
            else
            {
                if (!isAction || !_actionNode.RunUpdate(queueData.Actions))
                {
                    State = TreeState.Set;
                }
            }
        }

        public void ResetBranch()
        {
            State = TreeState.End;
            _branchID = 0;
        }

        public void InitParam()
        {
            State = TreeState.Run;
            _branchID = 0;
            _actionNode.Init();
            _selectorNode.Init();
            _sequenceNode.Init();
            _saveConditionalID = 0;

            _branchDatas.ForEach(branch =>
            {
                branch.BrockDatas.ForEach(brock =>
                {
                    brock.QueueDatas.ForEach(queue =>
                    {
                        queue.Conditionals.ForEach(c => c.InitParam());
                        queue.Actions.ForEach(a => a.InitParam());
                    });
                });
            });
        }
    }
}

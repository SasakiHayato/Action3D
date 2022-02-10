using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BehaviourAI
{
    public interface IConditional
    {
        bool Check();
        GameObject Target { get; set; }
    }

    public interface IAction
    {
        void SetUp();
        bool Execute();
        GameObject Target { get; set; }
    }

    public partial class BehaviourTree : MonoBehaviour
    {
        BrockData _brockData;
        QueueData _queueData;
        TreeData _treeData;

        int _treeID = 0;
        
        public void Repeater()
        {
            if (_treeData != null
                && _treeData.Type != QueueType.ConditionTask 
                && !_treeData.BrockConditionals.All(c => c.Check()))
            {
                Debug.Log("ResetTreeData");
                _treeID++;
                _treeData = null;
                TreeState = State.Set;
                return;
            }

            switch (TreeState)
            {
                case State.Run: // Note. 与えられたQueueDataのConditionalとActionをリピートさせる。
                    bool check = false;

                    if (_queueData.Progress == QueueProgress.Task) check = true;
                    else if (_treeData != null)
                    {
                        if (_treeData.Type == QueueType.ConditionTask)
                            check = true;
                    }
                    else check = _conditional.CheckQueue(_queueData);

                    if (check) _action.Set(_queueData, this);
                    else
                    {
                        _sequence.SetNextBrockID(ref _treeID);
                        _action.Cancel(_queueData, this);
                    }

                    break;
                case State.Check: // Note. 与えられたBrockDataのQueueを順に調べて、TrueならQueueDataを差し込む
                    _queueData = _conditional.Check(_brockData.QueueDatas, gameObject);
                    Debug.Log($"StateChack TreeData{_treeData}");
                    if (_queueData != null)
                    {
                        _conditional.SetNextQueue();
                        _action = new ActionNode(_queueData, gameObject);
                        TreeState = State.Run;
                        Debug.Log("NextStateRun");
                    }
                    else
                    {
                        // 現在のQueueDataがマックスに達した際にSelectorなら次のTreeを調べる。
                        // Sequenceなら次のBrockDataの準備。
                        if (_brockData.QueueDatas.Count <= _conditional.QueueID)
                        {
                            _sequence.SetNextBrockID(ref _treeID);
                            _conditional.Init();
                            Debug.Log("StateCheckSetNext ***************************");
                            TreeState = State.Set;
                        }
                    }

                    break;
                case State.Set: // Note. Run、またはCheckが中断された際に、新たにBrockDataを差し込む
                    Debug.Log("StateSet");
                    _conditional.Init();
                    //_sequence.Init();
                    CheckBrock();

                    break;
                case State.Init: // Note. ゲーム開始時に一度呼ばれる。Setup
                    _treeData = null;
                    foreach (var tree in _treeDatas)
                        _conditional.SetUp(tree.BrockConditionals, gameObject);

                    TreeState = State.Set;

                    break;
            }
        }

        void CheckBrock()
        {
            // 対象のTreeDataが最後まで行った際にリセットする。
            if (_treeDatas.Count <= _treeID)
            {
                _treeID = 0;
                TreeState = State.Set;

                return;
            }
            
            // 条件付きのBrockDataを調べる。
            foreach (var tree in _treeDatas)
            {
                if (tree.Type == QueueType.ConditionSelect
                    && tree.BrockConditionals.All(c => c.Check()))
                {
                    _treeData = tree;
                    SetSelector(tree.BrockDatas);
                    return;
                }
                else if (tree.Type == QueueType.ConditionSequence
                    && tree.BrockConditionals.All(c => c.Check()))
                {
                    _treeData = tree;
                    SetSequence(tree.BrockDatas);
                    return;
                }
                else if (tree.Type == QueueType.ConditionTask
                    && tree.BrockConditionals.All(c => c.Check()))
                {
                    Debug.Log("ConditionTask");
                    _treeData = tree;
                    SetSequence(tree.BrockDatas);
                    return;
                }
            }

            // 無条件のBrockDataを上から順にたどる
            switch (_treeDatas[_treeID].Type)
            {
                case QueueType.Selector:
                    SetSelector(_treeDatas[_treeID].BrockDatas);
                    _treeID++;
                    return;

                case QueueType.Sequence:
                    SetSequence(_treeDatas[_treeID].BrockDatas);
                    return;
            }

            _treeID++;
        }

        void SetSelector(List<BrockData> brocks)
        {
            _brockData = _selector.GetRandomBrock(brocks);
            _treeID++;
            TreeState = State.Check;
        }

        void SetSequence(List<BrockData> brocks)　// Note. Sequenceの場合、QueueDataを順に調べる必要あり
        {
            if (brocks.Count <= _sequence.SequenceID)
            {
                _treeID++;
                _sequence.Init();
                TreeState = State.Set;
            }
            else
            {
                _brockData = _sequence.GetBrockData(brocks);
                TreeState = State.Check;
            }
        }
    }
}
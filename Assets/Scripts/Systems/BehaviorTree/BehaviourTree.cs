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
            Debug.Log($"CurrentTreeID {_treeID} *****************");

            if (_treeData != null && !_treeData.BrockConditionals.All(c => c.Check()))
            {
                Debug.Log("Repeater. NextTreeID");
                _treeID++;
                _treeData = null;
                TreeState = State.Set;
                return;
            }

            switch (TreeState)
            {
                case State.Run: // Note. 与えられたQueueDataのConditionalとActionをリピートさせる。
                    Debug.Log("StateRun");
                    bool check = false;

                    if (_queueData.Progress == QueueProgress.Task) check = true;
                    else check = _conditional.CheckQueue(_queueData);

                    if (check) _action.Set(_queueData, this);
                    else
                    {
                        _sequence.SetNextBrockID(ref _treeID);
                        _action.Cancel(_queueData, this);
                    }

                    break;
                case State.Check: // Note. 与えられたBrockDataのQueueを順に調べて、TrueならQueueDataを差し込む
                    Debug.Log("StateCheck");
                    _queueData = _conditional.Check(_brockData.QueueDatas, gameObject);
                    
                    if (_queueData != null)
                    {
                        Debug.Log("StateCheck. IsDataSet.");
                        _conditional.SetNextQueue();
                        _action = new ActionNode(_queueData, gameObject);
                        TreeState = State.Run;
                    }
                    else
                    {
                        Debug.Log("StateCheck. NotDataSet.");
                        // 現在のQueueDataがマックスに達した際にSelectorなら次のTreeを調べる。
                        // Sequenceなら次のBrockDataの準備。
                        if (_brockData.QueueDatas.Count <= _conditional.QueueID)
                        {
                            Debug.Log("StateCheck. NextTreeData.");
                            _sequence.SetNextBrockID(ref _treeID);
                            _conditional.Init();
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
                    Debug.Log("StateInit");
                    _treeData = null;
                    foreach (var tree in _treeDatas)
                        _conditional.SetUp(tree.BrockConditionals, gameObject);

                    TreeState = State.Set;

                    break;
            }
        }

        void CheckBrock()
        {
            Debug.Log("CheckBrockMethod");
            // 対象のTreeDataが最後まで行った際にリセットする。
            if (_treeDatas.Count <= _treeID)
            {
                Debug.Log("CheckBrockMethod. ResetTree");
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
                    Debug.Log("CheckBrockMethod. ConditionalSelect");
                    _treeData = tree;
                    SetSelector(tree.BrockDatas);
                    return;
                }
                else if (tree.Type == QueueType.ConditionSequence
                    && tree.BrockConditionals.All(c => c.Check()))
                {
                    Debug.Log("CheckBrockMethod. ConditionalSequence");
                    _treeData = tree;
                    SetSequence(tree.BrockDatas);
                    return;
                }
            }

            // 無条件のBrockDataを上から順にたどる
            switch (_treeDatas[_treeID].Type)
            {
                case QueueType.Selector:
                    Debug.Log("CheckBrockMethod. Selector");
                    SetSelector(_treeDatas[_treeID].BrockDatas);
                    _treeID++;
                    return;

                case QueueType.Sequence:
                    Debug.Log("CheckBrockMethod. Sequence");
                    SetSequence(_treeDatas[_treeID].BrockDatas);
                    return;
            }

            _treeID++;
        }

        void SetSelector(List<BrockData> brocks)
        {
            Debug.Log("SetSelectorMethod");
            _brockData = _selector.GetRandomBrock(brocks);
            _treeID++;
            TreeState = State.Check;
        }

        void SetSequence(List<BrockData> brocks)　// Note. Sequenceの場合、QueueDataを順に調べる必要あり
        {
            Debug.Log("SetSequenceMethod");
            if (brocks.Count == _sequence.SequenceID)
            {
                Debug.Log("SetSequenceMethod. NextTreeID");
                _treeID++;
                _sequence.Init();
                TreeState = State.Set;
            }
            else
            {
                Debug.Log("SetSequenceMethod. SetSquence");
                _brockData = _sequence.GetBrockData(brocks);
                TreeState = State.Check;
            }
        }
    }
}
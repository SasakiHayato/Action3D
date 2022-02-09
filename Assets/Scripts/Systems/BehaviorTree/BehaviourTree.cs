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
        bool _isSequence = false;

        public void Repeater()
        {
            if (_treeData != null && !_treeData.BrockConditionals.All(c => c.Check()))
            {
                Debug.Log("Repeater. NextTreeID");
                _treeID++;
                _treeData = null;
                _treeState = State.Set;
                return;
            }

            switch (_treeState)
            {
                case State.Run: // Note. 与えられたQueueDataのConditionalとActionをリピートさせる。
                    Debug.Log("StateRun");
                    bool check = _conditional.CheckQueue(_queueData);

                    if (check) _action.Set(_queueData, this);
                    else
                    {
                        _sequence.SetNextBrockID();
                        _action.Cansel(_queueData, this);
                    }

                    break;
                case State.Check: // Note. 与えられたBrockDataのQueueを順に調べて、TrueならQueueDataを差し込む
                    Debug.Log("StateCheck **************");
                    _queueData = _conditional.Check(_brockData.QueueDatas, gameObject);
                    
                    if (_queueData != null)
                    {
                        Debug.Log("StateCheck. IsDataSet.");
                        _conditional.SetNextQueue();
                        _action = new ActionNode(_queueData, gameObject);
                        _treeState = State.Run;
                    }
                    else
                    {
                        Debug.Log("StateCheck. NotDataSet.");
                        // 現在のQueueDataがマックスに達した際にSelectorなら次のTreeを調べる。
                        // Sequenceなら次のBrockDataの準備。
                        if (_brockData.QueueDatas.Count <= _conditional.QueueID)
                        {
                            Debug.Log("StateCheck. NextTreeData.");
                            if (!_isSequence) _treeID++;
                            else _sequence.SetNextBrockID();
                            
                            _conditional.Init();
                            _treeState = State.Set;
                        }
                    }

                    break;
                case State.Set: // Note. Run、またはCheckが中断された際に、新たにBrockDataを差し込む
                    Debug.Log("StateSet");
                    _conditional.Init();
                    _sequence.Init();
                    CheckBrock();

                    break;
                case State.Init: // Note. ゲーム開始時に一度呼ばれる。Setup
                    Debug.Log("StateInit");
                    _treeData = null;

                    foreach (var tree in _treeDatas)
                        _conditional.SetUp(tree.BrockConditionals, gameObject);

                    _treeState = State.Set;

                    break;
            }
        }

        void CheckBrock()
        {
            Debug.Log("CheckBrockMethod");
            Debug.Log($"TreeID {_treeID}");
            // 対象のTreeDataが最後まで行った際にリセットする。
            if (_treeDatas.Count <= _treeID)
            {
                Debug.Log("CheckBrockMethod. ResetTree");
                _treeID = 0;
                _treeState = State.Set;

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
        }

        void SetSelector(List<BrockData> brocks)
        {
            Debug.Log("SetSelectorMethod");
            _brockData = _selector.GetRandomBrock(brocks);
            _treeID++;
            _treeState = State.Check;
        }

        void SetSequence(List<BrockData> brocks)　// Note. Sequenceの場合、QueueDataを順に調べる必要あり
        {
            Debug.Log("SetSequenceMethod");
            if (brocks.Count == _sequence.SequenceID)
            {
                Debug.Log("SetSequenceMethod. NextTreeID");
                _isSequence = false;
                _treeID++;
                _sequence.Init();
                _treeState = State.Set;
            }
            else
            {
                Debug.Log("SetSequenceMethod. SetSquence");
                _isSequence = true;
                _brockData = _sequence.GetBrockData(brocks);
                _treeState = State.Check;
            }
        }
    }
}
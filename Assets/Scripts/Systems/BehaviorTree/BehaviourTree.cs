using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// BeheviaTreeの管理クラス
/// </summary>

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

        /// <summary>
        /// 現在のステート管理
        /// </summary>
        public void Repeater()
        {
            Debug.Log($"{gameObject.name} TreeID{_treeID} TreeState {TreeState}");

            if (_treeData != null && !_treeData.BrockConditionals.All(c => c.Check()))
            {
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
                    else check = _conditional.CheckQueue(_queueData);

                    if (check) _action.Set(_queueData, this);
                    else
                    {
                        _sequence.SetNextBrockID(ref _treeID);
                        _action.Cancel(_queueData, this);
                    }

                    break;
                case State.Check: // Note. 与えられたBrockDataのQueueを順に調べて、TrueならQueueDataを差し込むs
                    if (_brockData.BrockType == BrockType.Sequence)
                    {
                        _queueData = _conditional.CheckSequence(_brockData.QueueDatas, gameObject);
                    }
                    else
                    {
                        _queueData = _conditional.CheckSelector(_brockData, gameObject);
                    }
              
                    if (_queueData != null)
                    {
                        _conditional.SetNextQueue();
                        _action = new ActionNode(_queueData, gameObject);
                        TreeState = State.Run;
                    }
                    else
                    {
                        // 現在のQueueDataがマックスに達した際にSelectorなら次のTreeを調べる。
                        // Sequenceなら次のBrockDataの準備。
                        if (_brockData.QueueDatas.Count <= _conditional.QueueID
                            || _brockData.BrockType == BrockType.Selector)
                        {
                            _sequence.SetNextBrockID(ref _treeID);
                            _conditional.Init();
                            _treeData = null;
                            TreeState = State.Set;
                        }
                    }

                    break;
                case State.Set: // Note. Run、またはCheckが中断された際に、新たにBrockDataを差し込む
                    _conditional.Init();
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
                    Debug.Log("ConditionSelect***************************");
                    _treeData = tree;
                    SetSelector(tree.BrockDatas);
                    return;
                }
                else if (tree.Type == QueueType.ConditionSequence
                    && tree.BrockConditionals.All(c => c.Check()))
                {
                    Debug.Log("ConditionSequence");
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
                    Debug.Log("Sequence");
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
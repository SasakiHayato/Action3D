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
        int _treeID = 0;

        BrockData _brockData;
        QueueData _queueData;

        TreeData _treeData;
        bool _isSequence = false;

        public void Repeat()
        {
            Debug.Log($"TreeType {_treeDatas[_treeID].Type}");
            Debug.Log($"QueueID {_conditional.QueueID}");
            if (_treeData != null && !_treeData.BrockConditionals.All(c => c.Check()))
            {
                _treeID++;
                _treeData = null;
                _treeState = State.Set;
                return;
            }

            switch (_treeState)
            {
                case State.Run:
                    bool check = _conditional.CheckQueue(_queueData);

                    if (check) _action.Set(_queueData, this);
                    else _action.Cansel(_queueData, this);

                    break;
                case State.Check:
                    _conditional.Init();
                    _queueData = _conditional.Check(_brockData.QueueDatas, gameObject);

                    if (_queueData != null)
                    {
                        _action = new ActionNode(_queueData, gameObject);
                        _treeState = State.Run;
                    }
                    else
                    {
                        if (_brockData.QueueDatas.Count == _conditional.QueueID)
                        {
                            if (!_isSequence) _treeID++;
                            else _sequence.SetNextID();

                            _conditional.Init();
                            _treeState = State.Set;
                        }
                    }

                    break;
                case State.Set:
                    _sequence.Init();
                    _treeData = null;
                    CheckBrock();
                    break;
                case State.Init:

                    foreach (var tree in _treeDatas)
                    {
                        _conditional.SetUp(tree.BrockConditionals, gameObject);
                    }

                    _treeState = State.Set;
                    break;
            }
        }

        void CheckBrock()
        {
            if (_treeDatas.Count == _treeID)
            {
                _treeID = 0;
                _treeState = State.Set;
                return;
            }
            
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
            }

            switch (_treeDatas[_treeID].Type)
            {
                case QueueType.Selector:
                    SetSelector(_treeDatas[_treeID].BrockDatas);
                    return;

                case QueueType.Seqence:
                    SetSequence(_treeDatas[_treeID].BrockDatas);
                    return;
            }
        }

        void SetSelector(List<BrockData> brocks)
        {
            _brockData = _selector.GetRandomBrock(brocks);
            _treeID = _brockData.BrockID;
            _treeState = State.Check;
        }

        void SetSequence(List<BrockData> brocks)
        {
            if (brocks.Count == _sequence.SequenceID)
            {
                _isSequence = false;
                _treeID++;
                _sequence.Init();
                _treeState = State.Set;
            }
            else
            {
                _isSequence = true;
                _brockData = _sequence.GetBrockData(brocks);
                _treeID = _brockData.BrockID;
                _treeState = State.Check;
            }
        }
    }
}
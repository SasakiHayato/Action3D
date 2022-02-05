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

    public class BehaviourTree : MonoBehaviour
    {
        enum QueueType
        {
            Selector,
            Seqence,

            ConditionSelect,
            ConditionSequence,
        }

        [System.Serializable]
        class TreeData
        {
            public QueueType Type;

            [SerializeReference, SubclassSelector]
            public List<IConditional> BrockConditionals;

            public List<BrockData> BrockDatas;
        }

        [System.Serializable]
        class BrockData 
        {
            public int BrockID;
            public List<QueueData> QueueDatas;
        }

        [System.Serializable]
        class QueueData
        {
            [SerializeReference, SubclassSelector]
            public List<IConditional> Conditionals;

            [SerializeReference, SubclassSelector]
            public List<IAction> Actions;
        }

        [SerializeField] List<TreeData> _treeDatas;

        public State _treeState { get; set; } = State.Init;
        public enum State
        {
            Run,
            Check,
            Set,
            Init,
        }

        int _treeID = 0;

        SelectorNode _selector = new SelectorNode();
        SequenceNode _sequence = new SequenceNode();
        ConditionalNode _conditional = new ConditionalNode();
        ActionNode _action;

        BrockData _brockData;
        QueueData _queueData;

        TreeData _treeData;
        bool _isSequence = false;

        public void Repeat()
        {
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
                    _queueData = _conditional.Check(_brockData.QueueDatas, gameObject);
                    if (_queueData != null)
                    {
                        _action = new ActionNode(_queueData, gameObject);
                        _conditional.Init();
                        
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
                    break;

                case QueueType.Seqence:
                    SetSequence(_treeDatas[_treeID].BrockDatas);
                    break;
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

        class SelectorNode
        {
            public BrockData GetRandomBrock(List<BrockData> brockDatas)
            {
                int random = Random.Range(0, brockDatas.Count);
                return brockDatas[random];
            }

            int GetRandomSeed(int seed)
            {
                return 0;
            }
        }

        class SequenceNode
        {
            public int SequenceID { get; private set; } = 0;

            public void Init()
            {
                SequenceID = 0;
            }

            public void SetNextID() => SequenceID++;

            public BrockData GetBrockData(List<BrockData> brockDatas)
            {
                return brockDatas[SequenceID];
            }
        }

        class ConditionalNode
        {
            public int QueueID { get; private set; } = 0;
            
            public void Init()
            {
                QueueID = 0;
            }

            public void SetUp(List<IConditional> conditions, GameObject t)
            {
                conditions.ForEach(c => c.Target = t);
            }

            public QueueData Check(List<QueueData> queueDatas, GameObject t)
            {
                queueDatas[QueueID].Conditionals.ForEach(q => q.Target = t);

                if (queueDatas[QueueID].Conditionals.All(c => c.Check()))
                {
                    return queueDatas[QueueID];
                }
                else
                {
                    QueueID++;
                    return null;
                }
            }

            public bool CheckQueue(QueueData queueData)
            { 
                if (queueData.Conditionals.All(c => c.Check())) return true;
                else return false;
            }
        }

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
                if(queue.Actions[_actionID].Execute())
                {
                    _actionID++;
                    if (_actionCount == _actionID) tree._treeState = State.Check;
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
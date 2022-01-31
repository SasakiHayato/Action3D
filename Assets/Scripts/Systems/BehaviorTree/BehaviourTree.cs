using System.Collections;
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
            Selector = 0,
            Seqence = 1,

            CompsiteSelect = 2,
            CompsiteSequence = 3,
        }

        [System.Serializable]
        class TreeData
        {
            public QueueType Type;

            [SerializeReference, SubclassSelector]
            public IConditional CompsiteConditional;

            public List<BrockData> BrockDatas;
        }

        [System.Serializable]
        class BrockData { public List<QueueData> QueueDatas; }

        [System.Serializable]
        class QueueData
        {
            [SerializeReference, SubclassSelector]
            public List<IConditional> Conditionals;

            [SerializeReference, SubclassSelector]
            public List<IAction> Actions;
        }

        [SerializeField] List<TreeData> _treeDatas;

        public static State TreeState = State.Set;
        public enum State
        {
            Run,
            Check,
            Set,
        }

        int _treeID = 0;

        SelectorNode _selector = new SelectorNode();
        SequenceNede _sequence = new SequenceNede();
        ConditionalNode _conditional = new ConditionalNode();
        ActionNade _action;

        BrockData _brockData;
        QueueData _queueData;

        bool _isSequence = false;

        public void Repeat()
        {
            switch (TreeState)
            {
                case State.Run:
                    bool check = _conditional.CheckQueue(_queueData);
                    if (check) _action.Set(_queueData);
                    else _action.Cansel(_queueData);
                    break;
                case State.Check:
                    _queueData = _conditional.Check(_brockData.QueueDatas, gameObject);
                    if (_queueData != null)
                    {
                        _action = new ActionNade(_queueData, gameObject);
                        _conditional.Init();
                        
                        TreeState = State.Run;
                    }
                    else
                    {
                        if (_brockData.QueueDatas.Count == _conditional.QueueID)
                        {
                            
                            if (!_isSequence) _treeID++;
                            else _sequence.SetNextID();
                            _conditional.Init();
                            TreeState = State.Set;
                        }
                    }
                    break;
                case State.Set:
                    CheckBrock();
                    break;
            }
        }

        void CheckBrock()
        {
            if (_treeDatas.Count == _treeID)
            {
                _treeID = 0;
                TreeState = State.Set;
                return;
            }

            if (_treeDatas[_treeID].Type == QueueType.Selector)
            {
                _brockData = _selector.GetRandomBrock(_treeDatas[_treeID].BrockDatas);
                TreeState = State.Check;
                return;
            }

            if (_treeDatas[_treeID].Type == QueueType.Seqence)
            {
                if (_treeDatas[_treeID].BrockDatas.Count == _sequence.SequenceID)
                {
                    _isSequence = false;
                    _treeID++;
                    _sequence.Init();
                    TreeState = State.Set;
                    return;
                }
                else
                {
                    _isSequence = true;
                    _brockData = _sequence.GetBrockData(_treeDatas[_treeID].BrockDatas);
                    TreeState = State.Check;
                    return;
                }
            }

            foreach (var tree in _treeDatas)
            {
                if (tree.Type == QueueType.CompsiteSelect
                    && tree.CompsiteConditional.Check())
                {
                    _brockData = _selector.GetRandomBrock(tree.BrockDatas);
                    TreeState = State.Check;
                    return;
                }
                else if (tree.Type == QueueType.CompsiteSequence
                         && tree.CompsiteConditional.Check())
                {
                    _brockData = _sequence.GetBrockData(tree.BrockDatas);
                    TreeState = State.Check;
                    return;
                }
            }
        }

        class SelectorNode
        {
            public BrockData GetRandomBrock(List<BrockData> brockDatas)
            {
                int random = Random.Range(0, brockDatas.Count);
                return brockDatas[random];
            }
        }

        class SequenceNede
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
                if (queueData.Conditionals.All(c => c.Check()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        class ActionNade
        {
            int _actionCount;
            int _actionID = 0;

            public ActionNade(QueueData queue, GameObject t)
            {
                _actionID = 0;
                _actionCount = queue.Actions.Count;
                queue.Actions.ForEach(a => a.Target = t);
                queue.Actions.ForEach(a => a.SetUp());
            }

            public void Set(QueueData queue)
            {
                if(queue.Actions[_actionID].Execute())
                {
                    _actionID++;
                    if (_actionCount == _actionID) TreeState = State.Check;
                }
            }

            public void Cansel(QueueData queue)
            {
                queue.Actions.ForEach(a => a.SetUp());
                TreeState = State.Check;
            }
        }
    }
}
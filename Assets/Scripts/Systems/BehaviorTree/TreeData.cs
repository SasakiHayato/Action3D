using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourAI
{
    public partial class BehaviourTree : MonoBehaviour
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
    }
}
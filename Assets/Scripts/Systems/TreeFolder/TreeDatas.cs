using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    /// <summary>
    /// BehaviourTreeのデータクラス
    /// </summary>

    public partial class TreeManager : MonoBehaviour
    {
        public enum BrockType
        {
            Sequence,
            Selector,
            ConditionallySequence,
            ConditionallySelector,
        }

        public enum QueueType
        {
            Sequence,
            Selector,
        }

        public enum RunType
        {
            Update,
            Task,
        }

        public enum ConditionalType
        {
            Sequence,
            Selector,
        }

        [System.Serializable]
        public class BranchData
        {
            public BrockType BrockType;
            public ConditionalType Condition;
            public int Priority;
            public int ID { get; set; }

            [SerializeReference, SubclassSelector]
            public List<IConditional> BranchConditionals;

            public List<BlockData> BrockDatas;
        }

        [System.Serializable]
        public class BlockData
        {
            public QueueType QueueType;
            public List<QueueData> QueueDatas;
        }

        [System.Serializable]
        public class QueueData
        {
            public RunType RunType;
            public ConditionalType Condition;

            [SerializeReference, SubclassSelector]
            public List<IConditional> Conditionals;

            [SerializeReference, SubclassSelector]
            public List<IAction> Actions;
        }

        [SerializeField] List<BranchData> _branchDatas;
    }
}

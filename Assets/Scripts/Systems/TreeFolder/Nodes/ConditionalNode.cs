using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BehaviourTree
{
    /// <summary>
    /// �����Ɋւ���Ǘ��N���X
    /// </summary>

    public partial class TreeManager : MonoBehaviour
    {
        ConditionalNode _conditionalNode;

        class ConditionalNode
        {
            public bool Sequence(List<IConditional> conditionals)
            {
                return conditionals.All(c => c.Try());
            }

            public bool Selector(List<IConditional> conditionals)
            {
                return conditionals.Any(c => c.Try());
            }

            // �����t����D�悷��
            public BranchData SetBranch(TreeManager manager, int branchID)
            {
                foreach (var b in manager.ConditionallyBranches)
                {
                    if (b.BrockType == BrockType.ConditionallySelector 
                        || b.BrockType == BrockType.ConditionallySequence)
                    {
                        if (b.Condition == ConditionalType.Sequence)
                            if (Sequence(b.BranchConditionals))
                            {
                                return b;
                            }

                        if (b.Condition == ConditionalType.Selector)
                            if (Selector(b.BranchConditionals))
                            {
                                return b;
                            }
                    }
                }

                return manager.NormalBranches[branchID];
            }
        }
    }
}

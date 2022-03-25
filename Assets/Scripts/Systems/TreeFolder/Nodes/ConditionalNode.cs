using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BehaviourTree
{
    /// <summary>
    /// èåèÇ…ä÷Ç∑ÇÈä«óùÉNÉâÉX
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

            // èåèïtÇ´ÇóDêÊÇ∑ÇÈ
            public BranchData SetBranch(TreeManager manager, int branchID)
            {
                var branch = manager.ConditionallyBranches.FirstOrDefault(b => 
                {
                    if (b.Condition == ConditionalType.Sequence)
                        if (Sequence(b.BranchConditionals))
                        {
                            return true;
                        }

                    if (b.Condition == ConditionalType.Selector)
                        if (Selector(b.BranchConditionals))
                        {
                            return true;
                        }

                    return false;
                });

                if (branch != null) return branch;
                

                if (manager.NormalBranches.Count <= branchID) return null;
                else return manager.NormalBranches[branchID];
            }
        }
    }
}

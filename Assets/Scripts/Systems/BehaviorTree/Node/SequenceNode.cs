using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourAI
{
    public partial class BehaviourTree : MonoBehaviour
    {
        SequenceNode _sequence = new SequenceNode();

        class SequenceNode
        {
            public bool IsSequence { get; private set; }
            public int SequenceID { get; private set; }

            public void Init()
            {
                IsSequence = false;
                SequenceID = 0;
            }

            public void SetNextBrockID(ref int treeID)
            {
                if (IsSequence) SequenceID++;
                else treeID++;
            }

            public BrockData GetBrockData(List<BrockData> brockDatas)
            {
                IsSequence = true;
                return brockDatas[SequenceID];
            }
        }
    }
}

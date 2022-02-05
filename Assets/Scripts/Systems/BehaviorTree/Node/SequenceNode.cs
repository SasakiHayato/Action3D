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
    }
}

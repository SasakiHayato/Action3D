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
                Debug.Log("SequenceNode. InitMethod");
                SequenceID = 0;
            }

            public void SetNextBrockID()
            {
                Debug.Log("SequenceNode. SeNextBrockIDMethod");
                SequenceID++;
            }

            public BrockData GetBrockData(List<BrockData> brockDatas)
            {
                Debug.Log("SequenceNode. GetBrockData");
                return brockDatas[SequenceID];
            }
        }
    }
}

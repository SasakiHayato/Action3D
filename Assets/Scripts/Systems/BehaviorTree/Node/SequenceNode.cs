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
                Debug.Log("SequenceNode. InitMethod");
                IsSequence = false;
                SequenceID = 0;
            }

            public void SetNextBrockID(ref int treeID)
            {
                Debug.Log("SequenceNode. SeNextBrockIDMethod");
                if (IsSequence) SequenceID++;
                else treeID++;
            }

            public BrockData GetBrockData(List<BrockData> brockDatas)
            {
                Debug.Log("SequenceNode. GetBrockData");
                Debug.Log($"**************** CurrentSequenceID {SequenceID} *********************");
                IsSequence = true;
                return brockDatas[SequenceID];
            }
        }
    }
}

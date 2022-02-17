using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BeheviaTree‚ÌSelector‚ÌŠÇ—ƒNƒ‰ƒX
/// </summary>

namespace BehaviourAI
{
    public partial class BehaviourTree : MonoBehaviour
    {
        SelectorNode _selector = new SelectorNode();

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
    }
}

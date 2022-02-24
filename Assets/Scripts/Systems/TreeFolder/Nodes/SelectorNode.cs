using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    /// <summary>
    /// or‚É‘Š“–‚·‚é
    /// </summary>

    public partial class TreeManager : MonoBehaviour
    {
        SelectorNode _selectorNode;

        class SelectorNode
        {
            BrockData _saveBrockData = null;
            QueueData _saveQueueData = null;

            public BrockData SetBrockData(List<BrockData> brockDatas)
            {
                
                if (_saveBrockData != null)
                {
                 
                    return _saveBrockData;
                }
                else
                {
                    int random = Random.Range(0, brockDatas.Count);
                    _saveBrockData = brockDatas[random];
                    
                    return brockDatas[random];
                }
            }

            public QueueData SetQueueData(List<QueueData> queueDatas)
            {
                if (_saveBrockData != null)
                {
                    return _saveQueueData;
                }
                else
                {
                    int random = Random.Range(0, queueDatas.Count);
                    _saveQueueData = queueDatas[random];
                    
                    return queueDatas[random];
                }
            }

            public void Init()
            {
                _saveBrockData = null;
                _saveQueueData = null;
            }
        }
    }
}
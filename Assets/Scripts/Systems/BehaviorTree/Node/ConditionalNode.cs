using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// BeheviaTree‚ÌConditional‚ğŠÇ—‚·‚éƒNƒ‰ƒX
/// </summary>

namespace BehaviourAI
{
    public partial class BehaviourTree : MonoBehaviour
    {
        ConditionalNode _conditional = new ConditionalNode();

        class ConditionalNode
        {
            public int QueueID { get; private set; } = 0;

            public void SetNextQueue() => QueueID++;
           
            public void Init() => QueueID = 0;
         
            /// <summary>
            /// ’²‚×‚éConditional‚Ìİ’è
            /// </summary>
            /// <param name="conditions">‘ÎÛ‚ÌConditional</param>
            /// <param name="t">‘ÎÛ‚ÌObject</param>
            public void SetUp(List<IConditional> conditions, GameObject t)
            {
                conditions.ForEach(c => c.Target = t);
            }

            /// <summary>
            /// —^‚¦‚ç‚ê‚½QueueData‚ÌConditional‚ğ‡‚É’²‚×‚é
            /// </summary>
            /// <param name="queueDatas">—^‚¦‚ç‚ê‚éQueueData</param>
            /// <param name="t">‘ÎÛ‚ÌObject</param>
            /// <returns>QueueData</returns>
            public QueueData CheckSequence(List<QueueData> queueDatas, GameObject t)
            {
                if (queueDatas.Count == QueueID) return null;

                queueDatas[QueueID].Conditionals.ForEach(q => q.Target = t);
                if (queueDatas[QueueID].Conditionals.All(c => c.Check()))
                {
                    return queueDatas[QueueID];
                }
                else
                {
                    QueueID++;
                    return null;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="brockData">Queue‚ÌBrockData</param>
            /// <param name="t">‘ÎÛ‚ÌObject</param>
            /// <returns></returns>
            public QueueData CheckSelector(BrockData brockData, GameObject t)
            {
                int random = Random.Range(0, brockData.QueueDatas.Count);
                QueueData queueData = brockData.QueueDatas[random];
                queueData.Conditionals.ForEach(q => q.Target = t);

                if (queueData.Conditionals.All(c => c.Check()))
                {
                    return queueData;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// —^‚¦‚ç‚ê‚½QueueData‚ÌConditional‚Ì¬”Û
            /// </summary>
            /// <param name="queueData">’²‚×‚éQueueData</param>
            /// <returns>bool</returns>
            public bool CheckQueue(QueueData queueData)
            {
                if (queueData.Conditionals.All(c => c.Check())) return true;
                else return false;
            }
        }
    }
}
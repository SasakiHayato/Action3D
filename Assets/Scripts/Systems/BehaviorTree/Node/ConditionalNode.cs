using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// BeheviaTreeのConditionalを管理するクラス
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
            /// 調べるConditionalの設定
            /// </summary>
            /// <param name="conditions">対象のConditional</param>
            /// <param name="t">対象のObject</param>
            public void SetUp(List<IConditional> conditions, GameObject t)
            {
                conditions.ForEach(c => c.Target = t);
            }

            /// <summary>
            /// 与えられたQueueDataのConditionalを順に調べる
            /// </summary>
            /// <param name="queueDatas">与えられるQueueData</param>
            /// <param name="t">対象のObject</param>
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
            /// <param name="brockData">QueueのBrockData</param>
            /// <param name="t">対象のObject</param>
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
            /// 与えられたQueueDataのConditionalの成否
            /// </summary>
            /// <param name="queueData">調べるQueueData</param>
            /// <returns>bool</returns>
            public bool CheckQueue(QueueData queueData)
            {
                if (queueData.Conditionals.All(c => c.Check())) return true;
                else return false;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BehaviourAI
{
    public partial class BehaviourTree : MonoBehaviour
    {
        ConditionalNode _conditional = new ConditionalNode();

        class ConditionalNode
        {
            public int QueueID { get; private set; } = 0;

            public void SetNextQueue()
            {
                Debug.Log("ConditionalNode. SetNextQueueMethod");
                QueueID++;
            }

            public void Init()
            {
                Debug.Log("ConditionalNode. InitMethod");
                QueueID = 0;
            }

            public void SetUp(List<IConditional> conditions, GameObject t)
            {
                Debug.Log("ConditionalNode. SetUpMethod");
                conditions.ForEach(c => c.Target = t);
            }

            public QueueData Check(List<QueueData> queueDatas, GameObject t)
            {
                Debug.Log("ConditionalNode. CheckMethod");
                Debug.Log($"queueDatasCount {queueDatas.Count} CurrentQueueID {QueueID}");
                if (queueDatas.Count == QueueID)
                {
                    Debug.Log("End");
                    return null;
                }

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

            public bool CheckQueue(QueueData queueData)
            {
                Debug.Log("ConditionalNode. CheckQueueMethod");
                if (queueData.Conditionals.All(c => c.Check())) return true;
                else return false;
            }
        }
    }
}
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

            public void SetNextQueue() => QueueID++;

            public void Init()
            {
                QueueID = 0;
            }

            public void SetUp(List<IConditional> conditions, GameObject t)
            {
                conditions.ForEach(c => c.Target = t);
            }

            public QueueData Check(List<QueueData> queueDatas, GameObject t)
            {
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
                if (queueData.Conditionals.All(c => c.Check())) return true;
                else return false;
            }
        }
    }
}
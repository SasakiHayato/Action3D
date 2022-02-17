using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// BeheviaTree��Conditional���Ǘ�����N���X
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
            /// ���ׂ�Conditional�̐ݒ�
            /// </summary>
            /// <param name="conditions">�Ώۂ�Conditional</param>
            /// <param name="t">�Ώۂ�Object</param>
            public void SetUp(List<IConditional> conditions, GameObject t)
            {
                conditions.ForEach(c => c.Target = t);
            }

            /// <summary>
            /// �^����ꂽQueueData��Conditional�����ɒ��ׂ�
            /// </summary>
            /// <param name="queueDatas">�^������QueueData</param>
            /// <param name="t">�Ώۂ�Object</param>
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
            /// <param name="brockData">Queue��BrockData</param>
            /// <param name="t">�Ώۂ�Object</param>
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
            /// �^����ꂽQueueData��Conditional�̐���
            /// </summary>
            /// <param name="queueData">���ׂ�QueueData</param>
            /// <returns>bool</returns>
            public bool CheckQueue(QueueData queueData)
            {
                if (queueData.Conditionals.All(c => c.Check())) return true;
                else return false;
            }
        }
    }
}
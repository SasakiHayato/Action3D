using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    /// <summary>
    /// And�ɑ�������
    /// </summary>

    public partial class TreeManager : MonoBehaviour
    {
        SequenceNode _sequenceNode;

        class SequenceNode
        {
            public int _brockID = 0;
            public int _queueID = 0;

            public void InitQueueID() => _queueID = 0;
            public void InitBrockID() => _brockID = 0;

            public int CurrentBrockID => _brockID;

            public BrockData SetBrockData(List<BrockData> brockDatas)
            {
                if (brockDatas.Count <= _brockID)
                {
                    return null;
                }

                return brockDatas[_brockID];
            }

            public QueueData SetQueueData(List<QueueData> queueDatas)
            {
                if (queueDatas.Count <= _queueID)
                {
                    return null;
                }
                return queueDatas[_queueID];
            }

            /// <summary>
            /// �u�����`�f�[�^���������߂邩�ǂ����̔���
            /// </summary>
            /// <param name="branch">���ׂ�u�����`�f�[�^</param>
            /// <returns>�������߂����ۂ�</returns>
            public bool SetNextBrockData(BranchData branch)
            {
                _brockID++;

                if (branch.BrockDatas.Count <= _brockID)
                {
                    _brockID = 0;
                    return false;
                }

                return true;
            }

            public bool SetNextQueueData(BranchData branch)
            {
                _queueID++;
                
                if (branch.BrockDatas[_brockID].QueueDatas.Count <= _queueID)
                {
                    _queueID = 0;
                    return false;
                }
                
                return true;
            }

            public void Init()
            {
                _brockID = 0;
                _queueID = 0;
            }
        }
    }
} 

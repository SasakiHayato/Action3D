using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// BeheviaTree�̊Ǘ��N���X
/// </summary>

namespace BehaviourAI
{
    public interface IConditional
    {
        bool Check();
        GameObject Target { get; set; }
    }

    public interface IAction
    {
        void SetUp();
        bool Execute();
        GameObject Target { get; set; }
    }

    public partial class BehaviourTree : MonoBehaviour
    {
        BrockData _brockData;
        QueueData _queueData;
        TreeData _treeData;

        int _treeID = 0;

        /// <summary>
        /// ���݂̃X�e�[�g�Ǘ�
        /// </summary>
        public void Repeater()
        {
            Debug.Log($"{gameObject.name} TreeID{_treeID} TreeState {TreeState}");

            if (_treeData != null && !_treeData.BrockConditionals.All(c => c.Check()))
            {
                _treeID++;
                _treeData = null;
                TreeState = State.Set;
                return;
            }

            switch (TreeState)
            {
                case State.Run: // Note. �^����ꂽQueueData��Conditional��Action�����s�[�g������B
                    bool check = false;
                    if (_queueData.Progress == QueueProgress.Task) check = true;
                    else check = _conditional.CheckQueue(_queueData);

                    if (check) _action.Set(_queueData, this);
                    else
                    {
                        _sequence.SetNextBrockID(ref _treeID);
                        _action.Cancel(_queueData, this);
                    }

                    break;
                case State.Check: // Note. �^����ꂽBrockData��Queue�����ɒ��ׂāATrue�Ȃ�QueueData����������s
                    if (_brockData.BrockType == BrockType.Sequence)
                    {
                        _queueData = _conditional.CheckSequence(_brockData.QueueDatas, gameObject);
                    }
                    else
                    {
                        _queueData = _conditional.CheckSelector(_brockData, gameObject);
                    }
              
                    if (_queueData != null)
                    {
                        _conditional.SetNextQueue();
                        _action = new ActionNode(_queueData, gameObject);
                        TreeState = State.Run;
                    }
                    else
                    {
                        // ���݂�QueueData���}�b�N�X�ɒB�����ۂ�Selector�Ȃ玟��Tree�𒲂ׂ�B
                        // Sequence�Ȃ玟��BrockData�̏����B
                        if (_brockData.QueueDatas.Count <= _conditional.QueueID
                            || _brockData.BrockType == BrockType.Selector)
                        {
                            _sequence.SetNextBrockID(ref _treeID);
                            _conditional.Init();
                            _treeData = null;
                            TreeState = State.Set;
                        }
                    }

                    break;
                case State.Set: // Note. Run�A�܂���Check�����f���ꂽ�ۂɁA�V����BrockData����������
                    _conditional.Init();
                    CheckBrock();

                    break;
                case State.Init: // Note. �Q�[���J�n���Ɉ�x�Ă΂��BSetup
                    _treeData = null;
                    foreach (var tree in _treeDatas)
                        _conditional.SetUp(tree.BrockConditionals, gameObject);

                    TreeState = State.Set;

                    break;
            }
        }

        void CheckBrock()
        {
            // �Ώۂ�TreeData���Ō�܂ōs�����ۂɃ��Z�b�g����B
            if (_treeDatas.Count <= _treeID)
            {
                _treeID = 0;
                TreeState = State.Set;
                return;
            }
            
            // �����t����BrockData�𒲂ׂ�B
            foreach (var tree in _treeDatas)
            {
                if (tree.Type == QueueType.ConditionSelect
                    && tree.BrockConditionals.All(c => c.Check()))
                {
                    Debug.Log("ConditionSelect***************************");
                    _treeData = tree;
                    SetSelector(tree.BrockDatas);
                    return;
                }
                else if (tree.Type == QueueType.ConditionSequence
                    && tree.BrockConditionals.All(c => c.Check()))
                {
                    Debug.Log("ConditionSequence");
                    _treeData = tree;
                    SetSequence(tree.BrockDatas);
                    return;
                }
            }

            // ��������BrockData���ォ�珇�ɂ��ǂ�
            switch (_treeDatas[_treeID].Type)
            {
                case QueueType.Selector:
                    SetSelector(_treeDatas[_treeID].BrockDatas);
                    _treeID++;
                    return;

                case QueueType.Sequence:
                    Debug.Log("Sequence");
                    SetSequence(_treeDatas[_treeID].BrockDatas);
                    return;
            }

            _treeID++;
        }

        void SetSelector(List<BrockData> brocks)
        {
            _brockData = _selector.GetRandomBrock(brocks);
            _treeID++;
            TreeState = State.Check;
        }

        void SetSequence(List<BrockData> brocks)�@// Note. Sequence�̏ꍇ�AQueueData�����ɒ��ׂ�K�v����
        {
            if (brocks.Count <= _sequence.SequenceID)
            {
                _treeID++;
                _sequence.Init();
                TreeState = State.Set;
            }
            else
            {
                _brockData = _sequence.GetBrockData(brocks);
                TreeState = State.Check;
            }
        }
    }
}
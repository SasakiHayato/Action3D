using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NewBehaviorAI
{
    // BehaviorAI���g��User�Ɍp��������
    public interface NewIBehavior
    {
        GameObject SetTarget();
    }

    // �ʃN���X�Ōp�������ď��������߂� 
    public interface IConditional
    {
        GameObject Target { set; }
        bool Check();
    }

    // �ʃN���X�Ōp�������čs�������߂�
    // Note : �^�X�N���I�������^�C�~���O��Bool��True�ɂ���End()��Ԃ�
    // Note : �ʓr��bool�^�̕ϐ����K�v
    public interface IAction
    {
        GameObject Target { set; }
        void Execute();
        bool End();
        bool Reset { set; }
    }

    public class NewBehaviourTree : MonoBehaviour
    {
        public enum State
        {
            Run,
            Set,

            None,
        }

        public static State TreeState { get; set; } = State.None;
        
        [SerializeField] List<SelectorData> _selector = new List<SelectorData>();

        [System.Serializable]
        public class SelectorData
        {
            public List<SeqenceData> SequenceDatas = new List<SeqenceData>();

            [System.Serializable]
            public class SeqenceData
            {
                public List<SelectorData> SelectData = new List<SelectorData>();
                [SerializeReference, SubclassSelector]
                public List<IConditional> Conditionals;
                [SerializeReference, SubclassSelector]
                public List<IAction> Actions;
            }
        }

        SelectorNode _stN;
        SequenceNode _sqN;

        /// <summary>
        /// �Ώۂ�Object�ɑ΂���s�������߂�
        /// </summary>
        /// <typeparam name="T">���g��IBehavior</typeparam>
        /// <param name="get">���s������Ώ�. this</param>
        public void Repeater<T>(T get) where T : NewIBehavior
        {
            switch (TreeState)
            {
                case State.Run:
                    _sqN.Set(_selector[_stN.GetID].SequenceDatas[_sqN.SequenceID]);
                    break;
                case State.Set:
                    TreeState = State.None;
                    break;
                case State.None:
                    _stN = new SelectorNode(_selector, get.SetTarget());
                    _sqN = new SequenceNode(get.SetTarget());
                    TreeState = State.Set;
                    _stN.Set(_sqN);
                    break;
            }
        }

        class SelectorNode
        {
            // ������
            public SelectorNode(List<SelectorData> st, GameObject t)
            {
                _selectorDatas = st;
                _target = t;
            }
            List<SelectorData> _selectorDatas;
            GameObject _target;

            // Note : ���P�̗]�n����. ���݂͑S�� 0 ���w��
            public int GetID { get => _selectorID; }
            int _selectorID = 0;

            /// <summary>
            /// �ǂ���Conditional�𒲂ׂ邩�����߂�
            /// </summary>
            /// <param name="sq">���g�̎���Sequence</param>
            public void Set(SequenceNode sq)
            {
                ConditionalNode cN = new ConditionalNode();
                cN.SetTarget = _target;
                if (_selectorDatas.Count <= 0)
                {
                    Debug.LogError("�f�[�^������܂���");
                    return;
                }
                if (_selectorDatas.Count <= 1)
                {
                    _selectorID = 0;
                    cN.Set(_selectorDatas[_selectorID], sq);
                }
                else
                {
                    // �����ݒ� : Note ���P�̗]�n����
                    int randomID = Random.Range(0, _selectorDatas.Count);
                    _selectorID = randomID;
                    
                }
            }


        }

        class ConditionalNode
        {
            public GameObject SetTarget { private get; set; }

            /// <summary>
            /// Conditinal�𒲂ׂ�True�Ȃ炻��Conditional��ۑ�
            /// </summary>
            /// <param name="st">�Ώۂ�Selector</param>
            /// <param name="sq">Selector�ɑ΂���Sequence</param>
            public void Set(SelectorData st, SequenceNode sq)
            {
                for (int id = 0; id < st.SequenceDatas.Count; id++)
                {
                    List<IConditional> c = st.SequenceDatas[id].Conditionals;
                    c.ForEach(t => t.Target = SetTarget);

                    if (c.All(c => c.Check()))
                    {
                        TreeState = State.Run;
                        sq.SequenceID = id;
                        return;
                    }
                }

                TreeState = State.None;
            }
        }

        class SequenceNode
        {
            // ������
            public SequenceNode(GameObject t)
            {
                _target = t;
            }
            GameObject _target;

            public int SequenceID { get; set; } = 0;
            ActionNode _aN = new ActionNode();

            /// <summary>
            /// �ۑ�����SequenceData�𑖂点��
            /// </summary>
            /// <param name="sq">�Ώۂ�SequenceData</param>
            public void Set(SelectorData.SeqenceData sq)
            {
                _aN.SetTarget = _target;
                if (sq.Conditionals.All(c => c.Check())) _aN.Set(sq.Actions);
                else
                {
                    sq.Actions.All(a => a.Reset = false);
                    TreeState = State.None;
                }
            }
        }

        class ActionNode
        {
            public GameObject SetTarget { get; set; }
            int _currentActionID = 0;

            /// <summary>
            /// True������Conditional�ɑ΂���Action��ΏۂɕԂ�
            /// </summary>
            /// <param name="a">Conditional�ɑ΂���Action</param>
            public void Set(List<IAction> a)
            {
                if (_currentActionID < a.Count())
                {
                    a[_currentActionID].Target = SetTarget;
                    if (a[_currentActionID].End())
                    {
                        //�@����Action��ݒ�
                        a[_currentActionID].Reset = false;
                        _currentActionID++;
                    }
                    else
                        a[_currentActionID].Execute();
                }
                else
                {
                    TreeState = State.None;
                    _currentActionID = 0;
                }
            }
        }
    }
}

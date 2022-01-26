using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NewBehaviorAI
{
    // BehaviorAIを使うUserに継承させる
    public interface NewIBehavior
    {
        GameObject SetTarget();
    }

    // 別クラスで継承させて条件を決める 
    public interface IConditional
    {
        GameObject Target { set; }
        bool Check();
    }

    // 別クラスで継承させて行動を決める
    // Note : タスクが終了したタイミングでBoolをTrueにしてEnd()を返す
    // Note : 別途でbool型の変数が必要
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
        /// 対象のObjectに対する行動を決める
        /// </summary>
        /// <typeparam name="T">自身のIBehavior</typeparam>
        /// <param name="get">実行させる対象. this</param>
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
            // 初期化
            public SelectorNode(List<SelectorData> st, GameObject t)
            {
                _selectorDatas = st;
                _target = t;
            }
            List<SelectorData> _selectorDatas;
            GameObject _target;

            // Note : 改善の余地あり. 現在は全て 0 を指定
            public int GetID { get => _selectorID; }
            int _selectorID = 0;

            /// <summary>
            /// どこのConditionalを調べるかを決める
            /// </summary>
            /// <param name="sq">自身の持つSequence</param>
            public void Set(SequenceNode sq)
            {
                ConditionalNode cN = new ConditionalNode();
                cN.SetTarget = _target;
                if (_selectorDatas.Count <= 0)
                {
                    Debug.LogError("データがありません");
                    return;
                }
                if (_selectorDatas.Count <= 1)
                {
                    _selectorID = 0;
                    cN.Set(_selectorDatas[_selectorID], sq);
                }
                else
                {
                    // 乱数設定 : Note 改善の余地あり
                    int randomID = Random.Range(0, _selectorDatas.Count);
                    _selectorID = randomID;
                    
                }
            }


        }

        class ConditionalNode
        {
            public GameObject SetTarget { private get; set; }

            /// <summary>
            /// Conditinalを調べてTrueならそのConditionalを保存
            /// </summary>
            /// <param name="st">対象のSelector</param>
            /// <param name="sq">Selectorに対するSequence</param>
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
            // 初期化
            public SequenceNode(GameObject t)
            {
                _target = t;
            }
            GameObject _target;

            public int SequenceID { get; set; } = 0;
            ActionNode _aN = new ActionNode();

            /// <summary>
            /// 保存したSequenceDataを走らせる
            /// </summary>
            /// <param name="sq">対象のSequenceData</param>
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
            /// TrueだったConditionalに対するActionを対象に返す
            /// </summary>
            /// <param name="a">Conditionalに対するAction</param>
            public void Set(List<IAction> a)
            {
                if (_currentActionID < a.Count())
                {
                    a[_currentActionID].Target = SetTarget;
                    if (a[_currentActionID].End())
                    {
                        //　次のActionを設定
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

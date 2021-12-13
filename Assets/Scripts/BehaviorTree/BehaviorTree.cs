using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorAI
{
    public interface IBehavior
    {
        GameObject SetTarget();
        void Call(IAction set);
    }

    public interface IConditional
    {
        GameObject Target { set; }
        bool Check();
    }

    public interface IAction
    {
        GameObject Target { set; }
        void Execute();
        bool End();
        bool Reset { set; }
    }


    public class BehaviorTree : MonoBehaviour
    {
        enum State
        {
            Run,
            Set,

            None,
        }

        State _state = State.None;
        [SerializeField] List<Selector> _selector = new List<Selector>();

        [System.Serializable]
        public class Selector
        {
            public List<Seqence> Datas = new List<Seqence>();

            [System.Serializable]
            public class Seqence
            {
                [SerializeReference, SubclassSelector]
                public IConditional Conditional;
                [SerializeReference, SubclassSelector]
                public IAction Action;
            }
        }

        SelectorNode _stN;
        SequenceNode _sqN;

        public void Repeater<T>(T get) where T : IBehavior
        {
            GameObject t = get.SetTarget();
            switch (_state)
            {
                case State.Run:
                    _sqN.Set(_selector[_stN.GetID].Datas[_sqN.ID], get, ref _state, t);
                    break;
                case State.Set:
                    _state = State.None;
                    break;
                case State.None:
                    _stN = new SelectorNode();
                    _sqN = new SequenceNode();
                    _state = State.Set;
                    _stN.Set(_selector, _sqN, ref _state, t);
                    break;
            }
        }

        class SelectorNode
        {
            public int GetID { get => _id; }
            int _id = 0;

            public void Set(List<Selector> st, SequenceNode sq, ref State state, GameObject t)
            {
                ConditionalNode cN = new ConditionalNode();
                cN.SetTarget = t;
                if (st.Count <= 0)
                {
                    Debug.LogError("データがありません");
                    return;
                }
                if (st.Count <= 1)
                {
                    _id = 0;
                    cN.Set(st[0], sq, ref state);
                }
                else
                {
                    // 乱数設定 : Note 改善の余地あり
                    int randomID = Random.Range(0, st.Count);
                    _id = randomID;
                    cN.Set(st[randomID], sq, ref state);
                }
            }
        }

        class ConditionalNode
        {
            public GameObject SetTarget { private get; set; }

            public void Set(Selector st, SequenceNode sq, ref State state)
            {
                for (int id = 0; id < st.Datas.Count; id++)
                {
                    IConditional c = st.Datas[id].Conditional;
                    c.Target = SetTarget;
                    if (c.Check())
                    {
                        state = State.Run;
                        sq.ID = id;
                        return;
                    }
                }

                state = State.None;
            }
        }

        class SequenceNode
        {
            public int ID { get; set; } = 0;

            public void Set(Selector.Seqence sq, IBehavior b, ref State s, GameObject t)
            {
                ActionNode aN = new ActionNode();
                aN.SetTarget = t;
                if (sq.Conditional.Check()) aN.Set(sq.Action, b, ref s);
                else
                {
                    sq.Action.Reset = false;
                    s = State.None;
                }
            }
        }

        class ActionNode
        {
            public GameObject SetTarget { get; set; }
            public void Set(IAction a, IBehavior iB, ref State state)
            {
                a.Target = SetTarget;
                if (a.End())
                {
                    state = State.None;
                    a.Reset = false;
                }
                else
                    iB.Call(a);
            }
        }
    }
}

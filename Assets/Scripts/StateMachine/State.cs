using UnityEngine;
using System;

namespace StateMachine
{
    /// <summary>
    /// 各Stateに継承させる、各Stateの管理クラス
    /// </summary>

    public abstract class State : MonoBehaviour
    {
        public abstract void SetUp(GameObject user);
        public abstract void Entry(Enum before);
        public abstract void Run();
        public abstract Enum Exit();
    }

    [Serializable]
    public class StateData
    {
        public string Path;
        public State State;
    }
}

using UnityEngine;
using System;

namespace StateMachine
{
    /// <summary>
    /// �eState�Ɍp��������A�eState�̊Ǘ��N���X
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

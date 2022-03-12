using System.Collections.Generic;
using UnityEngine;
using System;

namespace StateMachine
{
    /// <summary>
    /// StateMachine�̊Ǘ��N���X
    /// </summary>

    [Serializable]
    public class StateManager
    {
        [SerializeField] List<StateData> _stateDatas = new List<StateData>();

        GameObject _user;
        Dictionary<Enum, State> _stateDic;

        State _runState;
        Enum _saveType;
        public string CurrentStateType => _saveType.ToString();

        bool _isRun = false;

        /// <summary>
        /// User�̐ݒ�
        /// </summary>
        /// <param name="user">�Ώۂ�Object</param>
        /// <returns></returns>
        public StateManager SetUp(GameObject user)
        {
            _user = user;
            _stateDic = new Dictionary<Enum, State>();

            return this;
        }

        /// <summary>
        /// State�̒ǉ�
        /// </summary>
        /// <param name="type">State��Enum</param>
        /// <param name="pathName">State��PathName</param>
        /// <returns></returns>
        public StateManager AddState(Enum type, string pathName)
        {
            foreach (StateData data in _stateDatas)
            {
                if (data.Path == pathName)
                {
                    data.State.SetUp(_user);
                    _stateDic.Add(type, data.State);
                    return this;
                }
            }

            Debug.Log($"Can't AddState. Enum:{type}, Parh:{pathName}");
            return null;
        }

        /// <summary>
        /// Update�̐\��
        /// </summary>
        /// <param name="type">Update������State</param>
        /// <returns></returns>
        public StateManager RunRequest(Enum type)
        {
            foreach (var data in _stateDic)
            {
                if (data.Key.ToString() == type.ToString())
                {
                    _runState = data.Value;
                    _runState.Entry(type);

                    _saveType = data.Key;

                    _isRun = true;
                    Debug.Log($"IsSet. Request => State:{type}");
                    return this;
                }
            }

            Debug.Log($"Can't RunRequest. State:{type}");
            return null;
        }

        public void Update()
        {
            if (!_isRun)
            {
                Debug.Log("Not RunRequest yat.");
                return;
            }

            _runState.Run();

            Enum type = _runState.Exit();
            if (_saveType.ToString() != type.ToString())
            {
                ChangeState(type);
            }
        }

        /// <summary>
        /// Update������State��ς���
        /// </summary>
        /// <param name="type">Update������State</param>
        public Enum ChangeState(Enum type)
        {
            if (_saveType.ToString() == type.ToString()) return type;

            foreach (var data in _stateDic)
            {
                if (data.Key.ToString() == type.ToString())
                {
                    _runState = data.Value;
                    _runState.Entry(_saveType);
                    
                    _saveType = type;

                    return type;
                }
            }

            return null;
        }

        public Enum SetEntry(Enum type)
        {
            _runState.Entry(type);
            _saveType = type;

            return type;
        }
    }
}

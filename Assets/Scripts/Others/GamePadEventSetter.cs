using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// GamePad�ł�Button������Ǘ�
/// </summary>

public class GamePadEventSetter : SingletonAttribute<GamePadEventSetter>
{
    Dictionary<int, EventsData> _eventDic = new Dictionary<int, EventsData>();
    EventsData _pickUpEvent = null;
    
    public class EventsData
    {
        public bool IsRequest { get; set; }

        Dictionary<Button, Action> _eventDic = new Dictionary<Button, Action>();

        int _currentID = 0;

        public void Set(Button button, Action action)
        {
            _eventDic.Add(button, action);
        }

        public void Select()
        {

        }

        public void Call()
        {

        }
    }

    /// <summary>
    /// Event������List�����
    /// </summary>
    /// <param name="id">�f�[�^�ԍ��A�P�ȏ��ݒ�</param>
    /// <returns></returns>
    public GamePadEventSetter CreateList(int id)
    {
        EventsData events = new EventsData();
        _eventDic.Add(id, events);
        return this;
    }

    /// <summary>
    /// ���ꂽList�Ƀf�[�^����ꍞ��
    /// </summary>
    /// <param name="button">�Ώۂ�Button</param>
    /// <param name="action">Button�ɑ΂���Action</param>
    /// <param name="id">�ǉ�����List�ԍ�</param>
    /// <returns></returns>
    public GamePadEventSetter AddEvents(Button button, Action action, int id)
    {
        foreach (var dic in _eventDic)
        {
            if (dic.Key == id)
            {
                dic.Value.Set(button, action);
                return this;
            }
        }

        return this;
    }

    public void PickUpRequest(int id)
    {
        if (id <= 0)
        {
            _pickUpEvent = null;
        }

        foreach (var dic in _eventDic)
        {
            if (dic.Key == id)
            {
                dic.Value.IsRequest = true;
                _pickUpEvent = dic.Value;
            }
            else
            {
                dic.Value.IsRequest = false;
            }
        }
    }

    public void PickUp()
    {
        _pickUpEvent.Select();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// GamePadでのButton操作を管理
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
    /// Eventを入れるListを作る
    /// </summary>
    /// <param name="id">データ番号、１以上を設定</param>
    /// <returns></returns>
    public GamePadEventSetter CreateList(int id)
    {
        EventsData events = new EventsData();
        _eventDic.Add(id, events);
        return this;
    }

    /// <summary>
    /// 作られたListにデータを入れ込む
    /// </summary>
    /// <param name="button">対象のButton</param>
    /// <param name="action">Buttonに対するAction</param>
    /// <param name="id">追加するList番号</param>
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

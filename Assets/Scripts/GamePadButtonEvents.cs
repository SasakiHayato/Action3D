using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GamePadButtonEvents : SingletonAttribute<GamePadButtonEvents>
{
    public class ButtonEventsData
    {
        public int ID { get; set; }

        bool _isSetUp = false;

        public List<Button> Buttons { get; private set; } = new List<Button>();
        public List<Action> Actions { get; private set; } = new List<Action>();

        public ButtonEventsData AddEvents(Button button, Action action)
        {
            if (_isSetUp) return this;
            
            if (button.gameObject.activeSelf)
            {
                Buttons.Add(button);
                Actions.Add(action);
            }
            
            return this;
        }

        public ButtonEventsData FirstSetUp()
        {
            _isSetUp = true;
            return this;
        }
    }

    List<ButtonEventsData> _eventsDatas;
    ButtonEventsData _pickUpEvents; 
    
    int _currentSelectID = 0;
    int _saveID = 0;
    int _setID = 0;

    int _savePickUpID = 0;
    public int SavePickUpID { set { _savePickUpID = value; } }
    Action _saveCallBackAction = null;
    public Action SaveCallBackAction { set { _saveCallBackAction = value; } } 

    public override void SetUp()
    {
        base.SetUp();
        _eventsDatas = new List<ButtonEventsData>();
    }

    void SetSelectID(ref int setID)
    {
        if (_pickUpEvents == null)
        {
            _currentSelectID = 0;
            return;
        }

        int id = setID;
        if (id < 0) id = 0;
        if (id >= _pickUpEvents.Buttons.Count) id = _pickUpEvents.Buttons.Count - 1;
        setID = id;
        _currentSelectID = id;
    }

    public ButtonEventsData CreateList(int createID)
    {
        if (_eventsDatas.Count != 0)
        {
            foreach (var item in _eventsDatas)
            {
                if (item.ID == createID)
                {
                    Debug.Log("‚·‚Å‚É‚ ‚é");
                    return null;
                }
            }
        }
        
        ButtonEventsData data = new ButtonEventsData();
        data.ID = createID;
        _eventsDatas.Add(data);
        return data;
    }

    public void BackPickUp()
    {
        foreach (var item in _eventsDatas)
        {
            if (item.ID == _savePickUpID)
            {
                _pickUpEvents = item;
                _saveCallBackAction?.Invoke();
                return;
            }
        }
    }

    public void PickUpRequest(int id)
    {
        foreach (var item in _eventsDatas)
        {
            if (item.ID == id)
            {
                _pickUpEvents = item;
                return;
            }
        }
    }

    public void SelectChangeScale(Vector2 offset, Vector2 selected)
    {
        if (_pickUpEvents == null) return;

        for (int id = 0; id < _pickUpEvents.Buttons.Count; id++)
        {
            if (id == _currentSelectID)
            {
                _pickUpEvents.Buttons[id].transform.localScale = selected;
            }
            else
            {
                _pickUpEvents.Buttons[id].transform.localScale = offset;
            }
        }
    }

    public void Select()
    {
        Vector2 getVal = (Vector2)Inputter.GetValue(InputType.Select);

        if (getVal == Vector2.zero)
        {
            _saveID = 0;
            return;
        }

        if (_saveID != (int)getVal.y)
        {
            if ((int)getVal.y < 0)
            {
                _saveID = (int)getVal.y;
                _setID++;
                SetSelectID(ref _setID);
            }
            else
            {
                _saveID = (int)getVal.y;
                _setID--;
                SetSelectID(ref _setID);
            }
        }
    }

    public void IsSelected()
    {
        if (_pickUpEvents == null) return;

        for (int id = 0; id < _pickUpEvents.Buttons.Count; id++)
        {
            if (id == _currentSelectID)
            {
                Sounds.SoundMaster.PlayRequest(null, "Click", Sounds.SEDataBase.DataType.UI);
                _pickUpEvents.Actions[id].Invoke();
                return;
            }
        }
    }

    public void Dispose()
    {
        _eventsDatas = new List<ButtonEventsData>();
        _pickUpEvents = null;
        _currentSelectID = 0;
        _saveID = 0;
        _setID = 0;
    }
}

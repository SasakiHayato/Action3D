using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class WindowManager : SingletonAttribute<WindowManager>
{
    class WindowGroup
    {
        public IWindow IWindow;
        public int ID;
        public string Path;

        public List<WindowData> WindowDatas;

        public class WindowData
        {
            public int ID;
            public Image Target;
            public Action Action;
        }
    }

    List<WindowGroup> _windowDatas;

    int _groupID;
    int _windowID;

    int _selectID;
    int _inputY;

    WindowGroup _groupSetter;
    WindowGroup _saveWindow = null;
    string _savePath;

    public override void SetUp()
    {
        base.SetUp();

        _windowDatas = new List<WindowGroup>();
        _groupID = 0;
        _windowID = 0;
    }

    public WindowManager CreateWindowList(IWindow iWindow, string path)
    {
        WindowGroup window = new WindowGroup();
        window.IWindow = iWindow;
        window.ID = _groupID;
        window.Path = path;

        _groupSetter = window;

        window.WindowDatas = new List<WindowGroup.WindowData>();

        _windowDatas.Add(window);
        _groupID++;
        _windowID = 0;

        return this;
    }

    public WindowManager AddEvents(Image target, Action action)
    {
        WindowGroup.WindowData data = new WindowGroup.WindowData();
        data.Target = target;
        data.Action = action;
        data.ID = _windowID;

        _groupSetter.WindowDatas.Add(data);
        _windowID++;

        return this;
    }

    public WindowManager SetWindow(string path)
    {
        WindowGroup group = _windowDatas.FirstOrDefault(w => w.Path == path);
        
        _saveWindow = group;
        _savePath = group.Path;
        return this;
    }

    public void Request(string path)
    {
        WindowGroup group = _windowDatas.FirstOrDefault(w => w.Path == path);
        _selectID = 0;
        
        if (group == null)
        {
            Debug.Log("Nothing MatchData");
            return;
        }

        if (_saveWindow == null)
        {
            _saveWindow = group;
            _savePath = group.Path;
            group.IWindow.Open();
        }
        else
        {
            _saveWindow.IWindow.Close();

            if (_savePath != group.Path)
            {
                _saveWindow = group;
                group.IWindow.Open();
                Debug.Log("Request");
            }
            else
            {
                _saveWindow = null;
            }
        }
    }

    public void CloseRequest()
    {
        if (_saveWindow == null) return;

        _saveWindow.IWindow.Close();
        _saveWindow = null;
        _selectID = 0;
    }

    public void Selecting()
    {
        if (_saveWindow == null || _saveWindow.WindowDatas.Count <= 0) return;
        
        Vector2 input = (Vector2)Inputter.GetValue(InputType.Select);
        
        if ((int)input.y < 0 && _inputY != (int)input.y)
        {
            _inputY = (int)input.y;
            _selectID++;
            if (_selectID >= _saveWindow.WindowDatas.Count) _selectID--;
        }
        else if ((int)input.y > 0 && _inputY != (int)input.y)
        {
            _inputY = (int)input.y;
            _selectID--;
            if (_selectID < 0) _selectID = 0;
        }
        else if (input == Vector2.zero)
        {
            _inputY = 0;
        }
        
        foreach (WindowGroup.WindowData data in _saveWindow.WindowDatas)
        {
            if (data.Target == null) continue;

            if (data.ID == _selectID)
            {
                data.Target.rectTransform.localScale = Vector2.one * 1.5f;
            }
            else
            {
                data.Target.rectTransform.localScale = Vector2.one;
            }
        }
    }

    public void IsSelect()
    {
        if (_saveWindow == null) return;
        
        _saveWindow.WindowDatas[_selectID].Action.Invoke();
    }
}

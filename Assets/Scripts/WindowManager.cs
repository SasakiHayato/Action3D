using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

/// <summary>
/// Optionの階層構造の管理
/// </summary>

public class WindowManager : SingletonAttribute<WindowManager>
{
    // Note. 一つ一つのWindowのデータクラス
    class WindowGroup
    {
        public IWindow IWindow;
        public int ID;
        public string Path;
        
        public List<WindowData> WindowDatas;
        public List<WindowGroup> WindowList;

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
    List<WindowGroup> _windowList;
    string _savePath;

    public override void SetUp()
    {
        base.SetUp();

        _windowDatas = new List<WindowGroup>();
        _windowList = new List<WindowGroup>();
        _groupID = 0;
        _windowID = 0;
        _selectID = 0;
        _inputY = 0;
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
            }
        }

        _windowList.Add(group);
    }

    public void CloseRequest()
    {
        if (_saveWindow == null) return;
        
        _saveWindow.IWindow.Close();

        if (_windowList.Count > 1)
        {
            _windowList.Remove(_windowList.Last());
            _saveWindow = _windowList.Last();
        }

        _selectID = 0;
    }

    public void OpenRequest(string path)
    {
        WindowGroup window = _windowDatas.FirstOrDefault(w => w.Path == path);

        if (window == null) return;

        _windowList.Add(window);
        _saveWindow = window;

        window.IWindow.Open();

        _selectID = 0;
    }

    public void Selecting()
    {
        if (_saveWindow == null || _saveWindow.WindowDatas.Count <= 0) return;
        
        Vector2 input = (Vector2)Inputter.Instance.GetValue(InputType.Select);
        
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
        
        _saveWindow.WindowDatas[_selectID].Action?.Invoke();
    }
}

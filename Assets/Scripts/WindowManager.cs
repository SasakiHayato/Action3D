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
        public GameObject Target;
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

    WindowGroup _group;
    WindowGroup _saveWindow = null;

    public override void SetUp()
    {
        base.SetUp();

        _windowDatas = new List<WindowGroup>();
        _groupID = 0;
        _windowID = 0;
    }

    public WindowManager CreateWindowList(IWindow iWindow, GameObject target, string path)
    {
        WindowGroup window = new WindowGroup();
        window.IWindow = iWindow;
        window.Target = target;
        window.ID = _groupID;
        window.Path = path;

        _group = window;

        window.WindowDatas = new List<WindowGroup.WindowData>();

        _windowDatas.Add(window);
        _groupID++;

        return this;
    }

    public WindowManager AddEvents(Image target, Action action)
    {
        WindowGroup.WindowData data = new WindowGroup.WindowData();
        data.Target = target;
        data.Action = action;
        data.ID = _windowID;

        _group.WindowDatas.Add(data);
        _windowID++;

        return this;
    }

    public void Request(string path)
    {
        WindowGroup group = _windowDatas.FirstOrDefault(w => w.Path == path);

        if (group == null)
        {
            Debug.Log("Nothing MatchData");
            return;
        }

        if (_saveWindow == null)
        {
            _saveWindow = group;
            group.IWindow.Open();
        }
        else
        {
            _saveWindow.IWindow.Close();
            _saveWindow = group;
            group.IWindow.Open();
        }
    }

    public void Selecting()
    {

    }

    public void IsSelect()
    {

    }
}

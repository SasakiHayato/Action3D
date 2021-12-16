using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    Player,
    Game,

    None,
}

[System.Serializable]
public abstract class UIWindowParent
{
    [System.Serializable]
    public abstract class UIWindowChild
    {
        public int ID { get; set; }

        public abstract void SetUp();
        public abstract void UpDate();
        public abstract void CallBack(object[] data);
    }

    [SerializeField] UIType _type= UIType.None;
    [SerializeReference, SubclassSelector]
    List<UIWindowChild> _windows = new List<UIWindowChild>();

    public UIType GetUIType { get => _type; }

    public virtual void SetUp()
    {
        int setID = 1;
        foreach (UIWindowChild ui in _windows)
        {
            ui.ID = setID;
            setID++;
            ui.SetUp();
        }
    }

    public virtual void UpDate()
    {
        foreach (UIWindowChild ui in _windows)
        {
            ui.UpDate();
        }
    }

    public virtual void CallBack(int id, object[] data)
    {
        foreach (UIWindowChild ui in _windows)
        {
            if (ui.ID == id)
            {
                ui.CallBack(data);
            }
        }
    }
}
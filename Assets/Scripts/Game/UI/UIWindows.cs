using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 各UITypeのUI情報の基底クラス
/// </summary>

[System.Serializable]
public abstract class UIWindowParent
{
    [System.Serializable]
    public abstract class UIWindowChild
    {
        public int ID { get; set; }
        public Image ParentPanel { get; set; }

        public abstract void SetUp();
        public abstract void UpDate();
        public abstract void CallBack(object[] data);
    }

    [SerializeField] Image _targetPanel;
    [SerializeField] UIType _type;
    [SerializeReference, SubclassSelector]
    List<UIWindowChild> _windows = new List<UIWindowChild>();

    public Image GetPanel => _targetPanel;
    public Image SetPanel { set { _targetPanel = value; } }
    public UIType GetUIType { get => _type; }

    /// <summary>
    /// UI情報の初期化
    /// </summary>
    public virtual void SetUp()
    {
        int setID = 1;
        foreach (UIWindowChild ui in _windows)
        {
            ui.ID = setID;
            ui.ParentPanel = _targetPanel;
            ui.SetUp();
            setID++;
        }
    }

    /// <summary>
    /// 常にUI情報の更新をさせる
    /// </summary>
    public virtual void UpDate()
    {
        foreach (UIWindowChild ui in _windows)
        {
            ui.UpDate();
        }
    }

    /// <summary>
    /// 任意のタイミングでのUI情報の更新
    /// </summary>
    /// <param name="id">UITypeに対する更新させるUI情報のID</param>
    /// <param name="data">更新する際に伝える諸々のData群</param>
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
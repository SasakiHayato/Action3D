using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// UI管理のベースクラス
/// </summary>

public class BaseUI : SingletonAttribute<BaseUI>
{
    const string CanvasName = "Canvas";
    List<ParentUI> _uiList;

    public override void SetUp()
    {
        base.SetUp();
    }

    public void Load()
    {
        ParentUI[] parents = GameObject.Find(CanvasName).GetComponentsInChildren<ParentUI>();

        int id = 0;
        _uiList = new List<ParentUI>();

        foreach (ParentUI ui in parents)
        {
            Debug.Log(ui.gameObject.name);

            ui.ID = id;
            ui.gameObject.GetComponent<Image>().raycastTarget = false;
            ui.CanvasGroup = ui.gameObject.AddComponent<CanvasGroup>();
            ui.SetUp();

            _uiList.Add(ui);

            id++;
        }
    }

    public void CallBack(int parentID, int childrenID, object[] data = null)
    {
        var parent = _uiList.First(u => u.ID == parentID);
        parent.UIList.First(u => u.ID == childrenID).CallBack(data);
    }

    public void CallBack(string parentPath, string childrenPath, object[] data = null)
    {
        var parent = _uiList.First(u => u.Path == parentPath);
        parent.UIList.First(u => u.Path == childrenPath).CallBack(data);
    }

    public void CallBack(int parentID, string childrenPath, object[] data = null)
    {
        var parent = _uiList.First(u => u.ID == parentID);
        parent.UIList.First(u => u.Path == childrenPath).CallBack(data);
    }

    public void CallBack(string parentPath, int childrenID, object[] data = null)
    {
        var parent = _uiList.First(u => u.Path == parentPath);
        parent.UIList.First(u => u.ID == childrenID).CallBack(data);
    }

    public void ParentActive(int id, bool active)
    {
        _uiList.First(u => u.ID == id).Active(active);
    }

    public void ParentActive(string path, bool active)
    {
        _uiList.First(u => u.Path == path).Active(active);
    }
}

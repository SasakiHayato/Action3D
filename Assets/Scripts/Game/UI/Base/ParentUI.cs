using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// êeÇ∆Ç»ÇÈUIÇÃä«óùÉNÉâÉX
/// </summary>

public abstract class ParentUI : MonoBehaviour
{
    [SerializeField] string _path;
    public string Path => _path;

    public int ID { get; set; }
    public CanvasGroup CanvasGroup { get; set; }
    
    List<ChildrenUI> _uiList = new List<ChildrenUI>();
    public List<ChildrenUI> UIList => _uiList;

    public virtual void SetUp()
    {
        ChildrenUI[] uiList = GetComponentsInChildren<ChildrenUI>();
        int index = 0;

        foreach (ChildrenUI ui in uiList)
        {
            ui.ID = index;
            ui.ParentPanel = GetComponent<Image>();
            ui.SetUp();

            _uiList.Add(ui);

            index++;
        }
    }

    public void Active(bool active)
    {
        if (active) CanvasGroup.alpha = 1;
        else CanvasGroup.alpha = 0;
    }

    public abstract void CallBack(object[] datas);
}

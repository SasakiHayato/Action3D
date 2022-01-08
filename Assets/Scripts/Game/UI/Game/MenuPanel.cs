using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : UIWindowParent.UIWindowChild
{
    [SerializeField] string _name;
    Image _panel;
    RectTransform _rect;
    Vector2 _saveVec = Vector2.zero;

    public override void SetUp()
    {
        _panel = ParentPanel.gameObject.transform.Find(_name).GetComponent<Image>();
        _rect = _panel.GetComponent<RectTransform>();
        _saveVec = _rect.position;
    }

    public override void UpDate()
    {
        
    }

    public override void CallBack(object[] data)
    {
        
    }
}

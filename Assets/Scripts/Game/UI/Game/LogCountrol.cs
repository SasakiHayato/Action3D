using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogCountrol : UIWindowParent.UIWindowChild
{
    [SerializeField] string _panelName;
    [SerializeField] TextDataBase _textDataBase;
    GameObject _panel;
    Text _txt;

    public override void SetUp()
    {
        _panel = GameObject.Find(_panelName);
        _txt = _panel.GetComponentInChildren<Text>();
        _txt.text = "";
        _panel.SetActive(false);
    }

    public override void UpDate()
    {
        if (!_panel.activeSelf) return;
    }

    public override void CallBack(object[] data)
    {
        
    }
}

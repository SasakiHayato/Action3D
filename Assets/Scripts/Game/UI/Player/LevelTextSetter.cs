using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTextSetter : UIWindowParent.UIWindowChild
{
    [SerializeField] string _imageName;
    Text _txt;

    public override void SetUp()
    {
        _txt = GameObject.Find(_imageName).GetComponentInChildren<Text>();
        _txt.text = "Level : 001";
    }

    public override void UpDate()
    {
        
    }

    public override void CallBack(object[] data)
    {
        int level = (int)data[0];
        _txt.text = $"Level : {level.ToString("000")}";
    }
}

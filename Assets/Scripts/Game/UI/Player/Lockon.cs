using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lockon : UIWindowParent.UIWindowChild
{
    bool _isRockOn = false;
    GameObject _target;

    public override void SetUp()
    {
        
    }

    public override void UpDate()
    {
        
    }

    public override void CallBack(object[] data)
    {
        _target = (GameObject)data[0];
        Debug.Log(_target.name);
        Debug.Log("Rock");
    }
}
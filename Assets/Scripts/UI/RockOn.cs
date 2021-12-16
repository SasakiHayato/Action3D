using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockOn : UIWindowParent.UIWindowChild
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
        Debug.Log("Rock");
    }
}

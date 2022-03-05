using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : UIWindowParent
{
    public override void SetUp()
    {
        base.SetUp();
    }

    public override void UpDate()
    {
        if (GameManager.Option.Open != GameManager.Instance.OptionState) return;
        base.UpDate();
    }

    public override void CallBack(int id, object[] data)
    {
        base.CallBack(id, data);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessData : UIWindowParent.UIWindowChild
{
    [SerializeField] PostProcessProfile _profile;
    Vignette _vignette;

    public override void SetUp()
    {
        _vignette = _profile.GetSetting<Vignette>();
        _vignette.active = false;
    }

    public override void UpDate()
    {
        
    }

    public override void CallBack(object[] data)
    {
        if (!_vignette.active) _vignette.active = true;
        else _vignette.active = false;
    }
}

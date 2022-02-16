using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpSlider : UIWindowParent.UIWindowChild
{
    [SerializeField] string _sliderName;
    Slider _slider;

    public override void SetUp()
    {
        _slider = GameObject.Find(_sliderName).GetComponent<Slider>();
    }

    public override void UpDate()
    {
        
    }

    public override void CallBack(object[] data)
    {
        
    }
}

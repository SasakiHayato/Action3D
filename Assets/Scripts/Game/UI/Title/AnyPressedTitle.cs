using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

/// <summary>
/// TitleÉVÅ[ÉìÇÃàÍî‘ç≈èâÇÃButton
/// </summary>

public class AnyPressedTitle : UIWindowParent.UIWindowChild
{
    [SerializeField] string _buttonName;
    [SerializeField] string _requestActivePanelName;
    Button _button;

    public override void SetUp()
    {
        _button = ParentPanel.transform.Find(_buttonName).GetComponent<Button>();
        
        _button.OnClickAsObservable()
            .TakeUntilDestroy(_button)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => CallBack(null))
            .AddTo(_button);
    }

    public override void UpDate()
    {
        
    }

    public override void CallBack(object[] data)
    {
        Fader.Instance.Cancel();
        ParentPanel.transform.Find(_requestActivePanelName).gameObject.SetActive(true);
    }
}

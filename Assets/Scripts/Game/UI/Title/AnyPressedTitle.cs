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
    [SerializeField] string _titleButtonTextName;
    [SerializeField] string _requestActivePanelName;
    Button _button;

    GameObject _titleTextObj;

    public override void SetUp()
    {
        _titleTextObj = ParentPanel.transform.Find(_titleButtonTextName).gameObject;
        _button = ParentPanel.transform.Find(_buttonName).GetComponent<Button>();
        
        _button.OnClickAsObservable()
            .TakeUntilDestroy(_button)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => CallBack(null))
            .AddTo(_button);

        GamePadButtonEvents.Instance.CreateList(0)
            .AddEvents(_button, () => CallBack(null));

        GamePadButtonEvents.Instance.PickUpRequest(0);
    }

    public override void UpDate()
    {
        
    }

    public override void CallBack(object[] data)
    {
        Fader.Instance.Cancel();
        _titleTextObj.SetActive(false);
        _button.gameObject.SetActive(false);
        ParentPanel.transform.Find(_requestActivePanelName).gameObject.SetActive(true);
        GamePadButtonEvents.Instance.PickUpRequest(1);
    }
}

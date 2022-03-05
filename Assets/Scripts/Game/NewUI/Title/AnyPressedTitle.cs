using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

/// <summary>
/// TitleÉVÅ[ÉìÇÃàÍî‘ç≈èâÇÃButton
/// </summary>

public class AnyPressedTitle : ChildrenUI
{
    [SerializeField] GameObject _requestActivePanel;
    [SerializeField] GameObject _titleTextObj;

    Button _button;

    public override void SetUp()
    {
        _button = GetComponent<Button>();
        
        _button.OnClickAsObservable()
            .TakeUntilDestroy(_button)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => CallBack(null))
            .AddTo(_button);

        GamePadButtonEvents.Instance.CreateList(0)
            .AddEvents(_button, () => CallBack(null));

        GamePadButtonEvents.Instance.PickUpRequest(0);
    }

    public override void CallBack(object[] data)
    {
        Fader.Instance.Cancel();
        _titleTextObj.SetActive(false);
        _button.gameObject.SetActive(false);
        _requestActivePanel.SetActive(true);
        GamePadButtonEvents.Instance.PickUpRequest(1);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

/// <summary>
/// TitleÉVÅ[ÉìÇÃàÍî‘ç≈èâÇÃButton
/// </summary>

public class AnyPressedTitle : ChildrenUI, IWindow
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

        WindowManager.Instance.CreateWindowList(GetComponent<IWindow>(), "Title")
            .AddEvents(null, () => CallBack(null))
            .SetWindow("Title");
    }

    public void Open()
    {

    }

    public void Close()
    {
        _titleTextObj.SetActive(false);
        _requestActivePanel.SetActive(true);
        _button.gameObject.SetActive(false);
        Fader.Instance.Cancel();
    }

    public override void CallBack(object[] data)
    {
        WindowManager.Instance.Request("SetStart");
    }
}

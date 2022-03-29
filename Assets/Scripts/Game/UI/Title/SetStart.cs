using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

/// <summary>
/// Titleのスタートに関するクラス
/// </summary>

public class SetStart : ChildrenUI, IWindow
{
    [SerializeField] GameObject _panel;

    [SerializeField] Button _startButton;
    [SerializeField] Button _optionButton;
   
    public override void SetUp()
    {
        _startButton.OnClickAsObservable()
            .TakeUntilDestroy(_startButton)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => CallBack(null))
            .AddTo(_startButton);

        _optionButton.OnClickAsObservable()
            .TakeUntilDestroy(_optionButton)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => SetOptionPanel())
            .AddTo(_optionButton);

        WindowManager.Instance.CreateWindowList(GetComponent<IWindow>(), "SetStart")
            .AddEvents(_startButton.GetComponent<Image>(), () => CallBack(null))
            .AddEvents(_optionButton.GetComponent<Image>(), () => SetOptionPanel());

        gameObject.SetActive(false);
    }

    public override void CallBack(object[] data) 
    {
        WindowManager.Instance.Request("IntoGame");
    }

    public void Open()
    {

    }

    public void Close()
    {

    }
    
    void SetOptionPanel()
    {
        BaseUI.Instance.CallBackParent("Option");
    }
}

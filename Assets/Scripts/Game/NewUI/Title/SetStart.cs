using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

/// <summary>
/// Titleのスタートに関するクラス
/// </summary>

public class SetStart : ChildrenUI
{
    [SerializeField] GameObject _panel;

    [SerializeField] Button _startButton;
    [SerializeField] Button _optionButton;
    [SerializeField] Button _startWarldButton;
    [SerializeField] Button _startArenaButton;

    const float WaitTime = 1f;
    
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

        _startWarldButton.OnClickAsObservable()
            .TakeUntilDestroy(_startWarldButton)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => SetWorldCallBack())
            .AddTo(_startWarldButton);

        _startArenaButton.OnClickAsObservable()
            .TakeUntilDestroy(_startArenaButton)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => SetArenaCallBack())
            .AddTo(_startArenaButton);

        GamePadButtonEvents.Instance.CreateList(1)
            .AddEvents(_startButton, () => CallBack(null))
            .AddEvents(_optionButton, () => SetOptionPanel());


        GamePadButtonEvents.Instance.CreateList(2)
            .AddEvents(_startWarldButton, SetWorldCallBack)
            .AddEvents(_startArenaButton, SetArenaCallBack);

        gameObject.SetActive(false);
    }

    public override void CallBack(object[] data) 
    {
        _panel.SetActive(true);
        GamePadButtonEvents.Instance.PickUpRequest(2);
    }
    
    void SetOptionPanel()
    {
        BaseUI.Instance.CallBack("Option", "Menu", new object[] { 3 });
    }

    void SetArenaCallBack()
    {
        Sounds.SoundMaster.StopRequest("TitleBGM", Sounds.SEDataBase.DataType.BGM);
        Fader.Instance.Request(Fader.FadeType.Out, WaitTime);
        SceneSettings.Instance.LoadAsync(2, WaitTime);
    }

    void SetWorldCallBack()
    {
        Sounds.SoundMaster.StopRequest("TitleBGM", Sounds.SEDataBase.DataType.BGM);
        Fader.Instance.Request(Fader.FadeType.Out, WaitTime);
        SceneSettings.Instance.LoadAsync(1, WaitTime);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

/// <summary>
/// Titleのスタートに関するクラス
/// </summary>

public class SetStart : UIWindowParent.UIWindowChild
{
    [SerializeField] string _buttonPanelName;
    [SerializeField] string _startButtonName;
    [SerializeField] string _activePanelName;
    [SerializeField] string _warldButtonName;
    [SerializeField] string _arenaButtonName;

    Button _startButton;
    Button _startWarldButton;
    Button _startArenaButton;

    const float WaitTime = 1f;
    
    public override void SetUp()
    {
        Transform transform = ParentPanel.transform.Find(_buttonPanelName);
        _startButton = transform.Find(_startButtonName).GetComponent<Button>();
        _startButton.OnClickAsObservable()
            .TakeUntilDestroy(_startButton)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => CallBack(null))
            .AddTo(_startButton);

        GameObject panel = ParentPanel.transform.Find(_activePanelName).gameObject;
        panel.SetActive(true);

        _startWarldButton = panel.transform.Find(_warldButtonName).GetComponent<Button>();
        _startWarldButton.OnClickAsObservable()
            .TakeUntilDestroy(_startWarldButton)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => SetWorldCallBack())
            .AddTo(_startWarldButton);

        _startArenaButton = panel.transform.Find(_arenaButtonName).GetComponent<Button>();
        _startArenaButton.OnClickAsObservable()
            .TakeUntilDestroy(_startArenaButton)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => SetArenaCallBack())
            .AddTo(_startArenaButton);

        GamePadButtonEvents.Instance.CreateList(1)
            .AddEvents(_startButton, () => CallBack(null));

        GamePadButtonEvents.Instance.CreateList(2)
            .AddEvents(_startArenaButton, SetArenaCallBack)
            .AddEvents(_startWarldButton, SetWorldCallBack);

        panel.SetActive(false);
    }

    public override void UpDate() 
    {
        
    }

    public override void CallBack(object[] data) 
    {
        GameObject panel = ParentPanel.transform.Find(_activePanelName).gameObject;
        panel.SetActive(true);
        GamePadButtonEvents.Instance.PickUpRequest(2);
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

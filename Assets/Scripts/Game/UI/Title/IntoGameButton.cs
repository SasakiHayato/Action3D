using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

/// <summary>
/// InGameÇ÷å¸Ç©Ç§ÇΩÇﬂÇÃê›íËÉNÉâÉX
/// </summary>

public class IntoGameButton : ChildrenUI, IWindow
{
    [SerializeField] Button _goWorldButton;
    [SerializeField] Button _goArenaButton;

    const float WaitTime = 1f;

    public override void SetUp()
    {
        _goWorldButton.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .TakeUntilDestroy(_goWorldButton)
            .Subscribe(_ => SetWorldCallBack())
            .AddTo(_goWorldButton);

        _goArenaButton.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .TakeUntilDestroy(_goArenaButton)
            .Subscribe(_ => SetArenaCallBack())
            .AddTo(_goArenaButton);

        WindowManager.Instance.CreateWindowList(GetComponent<IWindow>(), "IntoGame")
            .AddEvents(_goWorldButton.GetComponent<Image>(), () => SetWorldCallBack())
            .AddEvents(_goArenaButton.GetComponent<Image>(), () => SetArenaCallBack());

        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
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

    public override void CallBack(object[] datas = null)
    {
        
    }
}

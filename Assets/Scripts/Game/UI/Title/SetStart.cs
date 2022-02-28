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
    [SerializeField] string _buttonName;

    Button _button;

    const float WaitTime = 1f;
    
    public override void SetUp()
    {
        Transform transform = ParentPanel.transform.Find(_buttonPanelName);
        _button = transform.Find(_buttonName).GetComponent<Button>();
        _button.OnClickAsObservable()
            .TakeUntilDestroy(_button)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => CallBack(null))
            .AddTo(_button);
    }

    public override void UpDate() { }

    public override void CallBack(object[] data) 
    {
        Sounds.SoundMaster.StopRequest("TitleBGM", Sounds.SEDataBase.DataType.BGM);
        Fader.Instance.Request(Fader.FadeType.Out, WaitTime);
        SceneSettings.Instance.LoadAsync(1, WaitTime);
    }
}

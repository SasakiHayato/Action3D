using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

/// <summary>
/// Titleのスタートに関するクラス
/// </summary>

public class SetStart : UIWindowParent.UIWindowChild
{
    [SerializeField] string _buttonName;

    Button _button;

    const float WaitTime = 1f;
    
    public override void SetUp()
    {
        _button = ParentPanel.transform.Find(_buttonName).GetComponent<Button>();
        _button.OnClickAsObservable()
            .TakeUntilDestroy(_button)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => OnClick())
            .AddTo(_button);
    }

    public override void UpDate() { }
    public override void CallBack(object[] data) { }

    void OnClick()
    {
        Fader.Instance.Request(Fader.FadeType.Out, WaitTime);
        SceneSettings.Instance.LoadAsync(1, WaitTime);
    }
}

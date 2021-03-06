using UnityEngine.UI;
using UniRx;
using System;

/// <summary>
/// Optionを閉じるクラスButtonからの制御
/// </summary>

public class BackOption : ChildrenUI
{
    Button _button;

    public override void SetUp()
    {
        _button = GetComponent<Button>();

        _button.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .TakeUntilDestroy(_button)
            .Subscribe(_ => WindowManager.Instance.CloseRequest())
            .AddTo(_button);
    }

    public override void CallBack(object[] datas = null)
    {
        WindowManager.Instance.CloseRequest();
    }
}

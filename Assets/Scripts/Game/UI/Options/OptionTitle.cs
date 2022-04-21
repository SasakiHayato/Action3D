using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

/// <summary>
/// TitleÇ÷å¸Ç©Ç§ÇΩÇﬂÇÃButtonê›íËÉNÉâÉX
/// </summary>

public class OptionTitle : ChildrenUI
{
    Button _button;

    public override void SetUp()
    {
        _button = GetComponent<Button>();

        _button.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .TakeUntilDestroy(_button)
            .Subscribe(_ => GoTitle())
            .AddTo(_button);
    }

    void GoTitle()
    {
        GameManager.Instance.InGameFieldType = GameManager.FieldType.None;
        GameManager.Instance.GameStateSetUpSystems(GameManager.GameState.End);
        GameManager.Instance.GameStateSetUpEvents(GameManager.GameState.End);
        Time.timeScale = 1;
    }

    public override void CallBack(object[] datas = null)
    {
        GoTitle();
    }
}

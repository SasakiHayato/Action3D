using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using System;

public class ArenaPanel : ChildrenUI, IWindow
{
    [SerializeField] Button _titleButton;
    [SerializeField] Button _retryButton;

    public override void SetUp()
    {
        _retryButton.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .TakeUntilDestroy(_retryButton)
            .Subscribe(_ => Retry())
            .AddTo(_retryButton);

        _titleButton.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .TakeUntilDestroy(_titleButton)
            .Subscribe(_ => GoTitle())
            .AddTo(_titleButton);

        WindowManager.Instance.CreateWindowList(GetComponent<IWindow>(), "ArenaPanel")
            .AddEvents(_retryButton.GetComponent<Image>(), () => Retry())
            .AddEvents(_titleButton.GetComponent<Image>(), () => GoTitle());

        Close();
    }

    public void Open()
    {
        RectTransform rect = gameObject.GetRect();
        
        rect.transform.DOScale(Vector2.one, 0.2f)
            .SetEase(Ease.Linear)
            .SetUpdate(true);

        GameManager.Instance.SetOptionState(GameManager.Option.Open);
    }

    public void Close()
    {
        RectTransform rect = gameObject.GetRect();
        
        rect.transform.DOScale(Vector2.zero, 0.2f)
            .SetEase(Ease.Linear)
            .SetUpdate(true);

        GameManager.Instance.SetOptionState(GameManager.Option.Close);
    }

    void GoTitle()
    {
        GameManager.Instance.InGameFieldType = GameManager.FieldType.None;
        GameManager.Instance.GameStateSetUpEvents(GameManager.GameState.End);
        Close();
    }

    void Retry()
    {
        int hp = GameManager.Instance.PlayerData.Player.MaxHP;
        int power = GameManager.Instance.PlayerData.Power;
        float speed = GameManager.Instance.PlayerData.Speed;

        GameManager.Instance.PlayerData.Player.SetParam(hp, power, speed, 1);
        WindowManager.Instance.CloseRequest();
        Close();
    }

    public override void CallBack(object[] datas = null)
    {
        
    }
}

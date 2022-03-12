using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

/// <summary>
/// OptionButtonÇ…ä÷Ç∑ÇÈUIèÓïÒ
/// </summary>

public class OptionButtons : ChildrenUI
{
    [SerializeField] Button _mapButton;
    [SerializeField] Button _itemButton;
    [SerializeField] Button _optionButton;
    [SerializeField] Button _titleButton;
    [SerializeField] Button _backButton;

    public override void SetUp()
    {
        _mapButton.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .TakeUntilDestroy(_mapButton)
            .Subscribe(_ => MapPanelActive())
            .AddTo(_mapButton);

        _itemButton.OnClickAsObservable()
            .TakeUntilDestroy(_itemButton)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => ItemPanelActive())
            .AddTo(_itemButton);

        _optionButton.OnClickAsObservable()
            .TakeUntilDestroy(_optionButton)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => OptionPanelActive())
            .AddTo(_optionButton);

        _backButton.OnClickAsObservable()
            .TakeUntilDestroy(_backButton)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => Back())
            .AddTo(_backButton);

        _titleButton.OnClickAsObservable()
            .TakeUntilDestroy(_titleButton)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => GoTitle())
            .AddTo(_titleButton);

        if (GameManager.GameState.Title == GameManager.Instance.CurrentGameState)
        {
            _titleButton.gameObject.SetActive(false);
        }

        GamePadButtonEvents.Instance.CreateList(3)
            .AddEvents(_mapButton, MapPanelActive)
            .AddEvents(_itemButton, ItemPanelActive)
            .AddEvents(_optionButton, OptionPanelActive)
            .AddEvents(_titleButton, GoTitle)
            .AddEvents(_backButton, Back)
            .FirstSetUp();

        if (GameManager.GameState.InGame == GameManager.Instance.CurrentGameState)
        {
            GamePadButtonEvents.Instance.PickUpRequest(3);
        }
    }

    void MapPanelActive()
    {
        Debug.Log("Map");
    }

    void ItemPanelActive()
    {
        Debug.Log("Item");
    }

    void OptionPanelActive()
    {
        Debug.Log("option");
    }

    void GoTitle()
    {
        GameManager.Instance.GameStateSetUpSystems(GameManager.GameState.End);
        GameManager.Instance.GameStateSetUpEvents(GameManager.GameState.End);
        Time.timeScale = 1;
    }

    void Back()
    {
        BaseUI.Instance.CallBack("Option", "Menu", new object[] { 1 });
    }

    public override void CallBack(object[] data) { }
}

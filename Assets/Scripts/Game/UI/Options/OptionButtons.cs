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

        GamePadButtonEvents.Instance.CreateList(3)
            .AddEvents(_backButton, Back)
            .AddEvents(_optionButton, OptionPanelActive)
            .AddEvents(_itemButton, ItemPanelActive)
            .AddEvents(_mapButton, MapPanelActive)
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

    void Back()
    {
        BaseUI.Instance.CallBack("Option", "Menu", new object[] { 1 });
    }

    public override void CallBack(object[] data) { }
}

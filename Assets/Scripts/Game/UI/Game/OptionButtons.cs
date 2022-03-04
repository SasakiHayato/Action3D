using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

/// <summary>
/// OptionButtonÇ…ä÷Ç∑ÇÈUIèÓïÒ
/// </summary>

public class OptionButtons : UIWindowParent.UIWindowChild
{
    [SerializeField] string _menuPanelName;
    [SerializeField] string _mapButtonName;
    [SerializeField] string _itemButtonName;
    [SerializeField] string _optinButtonName;

    Button _mapButton;
    Button _itemButton;
    Button _optionButton;

    public override void SetUp()
    {
        GameObject obj = ParentPanel.transform.Find(_menuPanelName).gameObject;

        _mapButton = obj.transform.Find(_mapButtonName).GetComponent<Button>();
        _mapButton.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .TakeUntilDestroy(_mapButton)
            .Subscribe(_ => MapPanelActive())
            .AddTo(_mapButton);

        _itemButton = obj.transform.Find(_itemButtonName).GetComponent<Button>();
        _itemButton.OnClickAsObservable()
            .TakeUntilDestroy(_itemButton)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => ItemPanelActive())
            .AddTo(_itemButton);

        _optionButton = obj.transform.Find(_optinButtonName).GetComponent<Button>();
        _optionButton.OnClickAsObservable()
            .TakeUntilDestroy(_optionButton)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => OptionPanelActive())
            .AddTo(_optionButton);

        GamePadButtonEvents.Instance.CreateList(3)
            .AddEvents(_optionButton, OptionPanelActive)
            .AddEvents(_itemButton, ItemPanelActive)
            .AddEvents(_mapButton, MapPanelActive)
            .FirstSetUp();

        GamePadButtonEvents.Instance.PickUpRequest(3);
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

    public override void UpDate() { }
    public override void CallBack(object[] data) { }
}

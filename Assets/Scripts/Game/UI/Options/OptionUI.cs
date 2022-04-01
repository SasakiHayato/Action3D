using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OptionUI : ParentUI, IWindow
{
    [SerializeField] RectTransform _rect;
    [SerializeField] Vector2 _setPos;
    [SerializeField] Image _backButtonImage;
    [SerializeField] Image _mapButtonImage;
    [SerializeField] Image _itemButtonImage;
    [SerializeField] Image _titleButtonImage;
    [SerializeField] Image _systemButtonImage;

    Vector2 _centerPos = Vector2.zero;
    
    public override void SetUp()
    {
        base.SetUp();
    
        _centerPos = _rect.anchoredPosition;
        _rect.anchoredPosition = _setPos;

        WindowManager.Instance.CreateWindowList(GetComponent<IWindow>() , "Option")
            .AddEvents(_titleButtonImage, () => BaseUI.Instance.CallBack("Option", "Title"))
            .AddEvents(_backButtonImage, () => BaseUI.Instance.CallBack("Option", "Back"));
    }

    public void Open()
    {
        GameManager.Instance.SetOptionState(GameManager.Option.Open);
        Time.timeScale = 0;
        _rect.DOAnchorPosX(_centerPos.x, 0.2f)
            .SetEase(Ease.Linear)
            .SetUpdate(true);
    }

    public void Close()
    {
        _rect.DOAnchorPosX(_setPos.x, 0.2f)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    if (GameManager.GameState.Title != GameManager.Instance.CurrentGameState)
                    {
                        GameManager.Instance.SetOptionState(GameManager.Option.Close);
                    }

                    Time.timeScale = 1;
                });
    }

    public override void CallBack(object[] datas)
    {
        WindowManager.Instance.Request("Option");
    }
}

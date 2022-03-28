using UnityEngine;
using DG.Tweening;

/// <summary>
/// MunuPanel‚ÌŠÇ—ƒNƒ‰ƒX
/// </summary>

public class MenuPanel : ChildrenUI
{
    [SerializeField] Vector2 _setPos;

    RectTransform _rect;
    Vector2 _saveVec = Vector2.zero;

    bool _isActive = false;

    public override void SetUp()
    {
        _rect = gameObject.GetRect();
        _saveVec = _rect.anchoredPosition;
        _rect.anchoredPosition = _setPos;
        
        if (GameManager.GameState.Title != GameManager.Instance.CurrentGameState)
        {
            Inputter.Instance.Inputs.UI.Options.started += context => SetCanvas();
        }
    }

    void SetCanvas()
    {
        if (_isActive)
        {
            _rect.DOAnchorPosX(_setPos.x, 0.2f)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnComplete(() => 
                {
                    _isActive = false;

                    if (GameManager.GameState.Title != GameManager.Instance.CurrentGameState)
                    {
                        GameManager.Instance.SetOptionState(GameManager.Option.Close);
                    }

                    Time.timeScale = 1;
                });
        }
        else
        {
            GameManager.Instance.SetOptionState(GameManager.Option.Open);
            Time.timeScale = 0;
            _rect.DOAnchorPosX(_saveVec.x, 0.2f)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnComplete(() => _isActive = true);
        }
    }

    public override void CallBack(object[] data)
    {
        Debug.Log((int)data[0]);
        SetCanvas();
        GamePadButtonEvents.Instance.PickUpRequest((int)data[0]);
    }
}

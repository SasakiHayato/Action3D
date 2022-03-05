using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// MunuPanelÇÃä«óùÉNÉâÉX
/// </summary>

public class MenuPanel : UIWindowParent.UIWindowChild
{
    [SerializeField] string _name;
    [SerializeField] Vector2 _setPos;

    Image _panel;
    RectTransform _rect;
    Vector2 _saveVec = Vector2.zero;

    bool _isActive = false;

    public override void SetUp()
    {
        _panel = ParentPanel.gameObject.transform.Find(_name).GetComponent<Image>();
        _rect = _panel.GetComponent<RectTransform>();
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
            if (GameManager.GameState.Title != GameManager.Instance.CurrentGameState)
            {
                GameManager.Instance.SetOptionState(GameManager.Option.Close);
            }
            
            Time.timeScale = 1;
            _rect.DOAnchorPosX(_setPos.x, 0.2f)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnComplete(() => _isActive = false);
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

    public override void UpDate()
    {
     
    }

    public override void CallBack(object[] data)
    {
        Debug.Log((int)data[0]);
        SetCanvas();
        GamePadButtonEvents.Instance.PickUpRequest((int)data[0]);
    }
}

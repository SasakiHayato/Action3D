using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

        Inputter.Instance.Inputs.UI.Options.started += context => SetCanvas();
    }

    void SetCanvas()
    {
        if (_isActive)
        {
            _rect.DOAnchorPosX(_setPos.x, 0.2f)
                .SetEase(Ease.Linear)
                .OnComplete(() => _isActive = false);
        }
        else
        {
            _rect.DOAnchorPosX(_saveVec.x, 0.2f)
                .SetEase(Ease.Linear)
                .OnComplete(() => _isActive = true);
        }
    }

    public override void UpDate()
    {
        if (_isActive)
        {
            Select();
        }
    }

    void Select()
    {

    }

    public override void CallBack(object[] data)
    {
        
    }
}

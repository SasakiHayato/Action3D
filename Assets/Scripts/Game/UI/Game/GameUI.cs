using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameUI : UIWindowParent
{
    [SerializeField] Vector2 _setCanvasPos;
    [SerializeField] int _maxID;
    bool _canvasActive = true;
    RectTransform _rect;

    int _saveValue = 0;
    int _canvasID = 0;

    public override void SetUp()
    {
        _rect = GetPanel.GetComponent<RectTransform>();
        _rect.anchoredPosition = _setCanvasPos;

        Inputter.Instance.Inputs.UI.Options.started += context
            => SetCanvas();

        base.SetUp();
        SetCanvas();
    }

    public override void UpDate()
    {
        if (_canvasActive)
        {
            Select();
            base.UpDate();
        }
        else
        {
            CallBack(0, null);
        }
    }

    public override void CallBack(int id, object[] data)
    {
        base.CallBack(id, data);
    }

    void Select()
    {
        Vector2 get = (Vector2)Inputter.GetValue(InputType.Select);
        if ((int)get.y != _saveValue)
        {
            _saveValue = (int)get.y;
            _canvasID += _saveValue;
            if (_canvasID > _maxID) _canvasID = _maxID;
            else if (_canvasID < 0) _canvasID = 0;
        }
    }

    void SetCanvas()
    {
        Sounds.SoundMaster.Request(null, "Click", 4);

        if (!_canvasActive)
        {
            _rect.DOAnchorPosX(0, 0.2f)
                .SetEase(Ease.Linear)
                .OnComplete(() => _canvasActive = true);
        }
        else
        {
            _rect.DOAnchorPosX(_setCanvasPos.x, 0.2f)
                .SetEase(Ease.Linear)
                .OnComplete(() => _canvasActive = false);
        }
    }
}

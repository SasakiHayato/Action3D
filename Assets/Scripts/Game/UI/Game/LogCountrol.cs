using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LogCountrol : UIWindowParent.UIWindowChild
{
    [SerializeField] string _panelName;
    [SerializeField] float _displayTime;
    [SerializeField] TextDataBase _textDataBase;
    [SerializeField] Vector2 _offSetPos;
    RectTransform _panelRect;
    Text _txt;

    bool _isActive = false;
    Vector2 _savePos;
    float _timer;

    public override void SetUp()
    {
        _panelRect = GameObject.Find(_panelName).GetComponent<RectTransform>();
        _savePos = _panelRect.anchoredPosition;
        _panelRect.anchoredPosition = _offSetPos;
        _txt = _panelRect.GetComponentInChildren<Text>();
        _txt.text = "";
    }

    public override void UpDate()
    {
        if (!_isActive) return;

        _timer += Time.deltaTime;
        if (_timer > _displayTime)
        {
            Init();
            _panelRect.DOAnchorPosY(_offSetPos.y, 0.2f).SetEase(Ease.Linear);
        }
    }

    void Init()
    {
        _timer = 0;
        _isActive = false;
        _txt.text = "";
    }

    public override void CallBack(object[] data)
    {
        int id = (int)data[0];
        _txt.text = _textDataBase.GetDatas[id].Text;
        if (!_isActive)
        {
            _panelRect.DOAnchorPosY(_savePos.y, 0.2f)
                .SetEase(Ease.Linear)
                .OnComplete(() => _isActive = true);
        }
    }
}

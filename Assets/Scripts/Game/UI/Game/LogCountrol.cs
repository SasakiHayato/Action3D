using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using Sounds;

/// <summary>
/// ÉQÅ[ÉÄè„Ç…Ç»Ç…Ç©ÇµÇÁÇÃLogÇÃï\é¶
/// </summary>

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
    List<string> _textList = new List<string>();

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
            _textList.Remove(_textList.First());
            if (_textList.Count <= 0)
            {
                Init();
                _panelRect.DOAnchorPosY(_offSetPos.y, 0.2f).SetEase(Ease.Linear);
            }
            else
            {
                _txt.text = _textList.First();
                SoundMaster.Request(null, "TextDisplay", SEDataBase.DataType.UI);
                _timer = 0;
            }
        }
    }

    void Init()
    {
        _timer = 0;
        _isActive = false;
        _txt.text = "";
        _textList = new List<string>();
    }

    public override void CallBack(object[] data)
    {
        int id = (int)data[0];
        _textList.Add(_textDataBase.GetDatas[id].Text);
        if (!_isActive)
        {
            _panelRect.DOAnchorPosY(_savePos.y, 0.2f)
                .SetEase(Ease.Linear)
                .OnComplete(() => _isActive = true);
            _txt.text = _textList.First();
            SoundMaster.Request(null, "TextDisplay", SEDataBase.DataType.UI);
        }
    }
}

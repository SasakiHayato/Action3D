using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// BossèoåªéûÇÃTextÇÃï\é¶
/// </summary>

public class BossInform : ChildrenUI
{
    [SerializeField] RectTransform _backGroundRect;

    [SerializeField] Text _warningText;
    [SerializeField] Text _nameText;

    // const
    Vector2 OutLineOffSet = new Vector2(200, 1);
    Vector2 AspectRatio = new Vector2(1600, 1000);

    Vector2 _saveWarningRectPos;
    Vector2 _saveNameRactPos;

    bool _isSetting = false;

    public override void SetUp()
    {
        _backGroundRect.localScale = Vector2.right;
        _warningText.gameObject.GetRect().localScale = Vector2.right;
    }

    public override void CallBack(object[] data)
    {
        if (_isSetting) return;
        Sounds.SoundMaster.PlayRequest(null, "BossDisplay", Sounds.SEDataBase.DataType.Enemys);

        _isSetting = true;
        _nameText.text = (string)data[0];
        RectTransform warningTextRect = _warningText.gameObject.GetRect();
        RectTransform nameTxetRect = _nameText.gameObject.GetRect();
        _saveWarningRectPos = warningTextRect.position;
        _saveNameRactPos = nameTxetRect.position;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_backGroundRect.DOScaleY(1, 0.2f))
            .Append(warningTextRect.DOScaleY(1, 0.2f))
            .AppendInterval(0.25f)
            .Append(DOTween.To
            (
                () => warningTextRect.gameObject.GetComponent<Outline>().effectDistance,
                (x) => warningTextRect.gameObject.GetComponent<Outline>().effectDistance = x,
                OutLineOffSet,
                0.5f
            ).SetEase(Ease.OutCubic))
            .AppendInterval(0.2f)
            .Append(nameTxetRect.DOAnchorPosX(0, 0.2f))
            .Join(warningTextRect.DOAnchorPosX(AspectRatio.x, 0.2f))
            .AppendInterval(0.75f)
            .Append(nameTxetRect.DOScaleY(0, 0.2f))
            .Join(_backGroundRect.DOScaleY(0, 0.2f))
            .OnComplete(() => Init());
    }

    void Init()
    {
        _warningText.gameObject.GetRect().position = _saveWarningRectPos;
        _warningText.gameObject.GetRect().localScale = Vector2.right;
        _warningText.gameObject.GetComponent<Outline>().effectDistance = Vector2.one;

        _nameText.gameObject.GetRect().position = _saveNameRactPos;
        _nameText.gameObject.GetRect().localScale = Vector2.one;

        _isSetting = false;
    }
}

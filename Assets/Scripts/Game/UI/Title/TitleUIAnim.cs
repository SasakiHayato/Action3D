using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// TitleのUIをアニメーションさせる。
/// </summary>

public class TitleUIAnim : ChildrenUI
{
    public override void SetUp()
    {
        AnimTitleButtonText();
    }

    void AnimTitleButtonText()
    {
        RectTransform rect = gameObject.GetRect();
        rect.DOScale(Vector2.one * 1.2f, 0.5f)
            .SetEase(Ease.InOutQuad)
            .SetEase(Ease.OutBack)
            .SetLoops(-1);
    }

    public override void CallBack(object[] data)
    {
        
    }
}

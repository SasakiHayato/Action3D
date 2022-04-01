using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OptionSystem : ChildrenUI, IWindow
{
    [SerializeField] RectTransform _targetPanel;
    [SerializeField] Vector2 _setPostion;
   
    Vector2 _offSetPos;

    public override void SetUp()
    {
        WindowManager.Instance.CreateWindowList(GetComponent<IWindow>(), "OptionSystem");
        _offSetPos = _targetPanel.anchoredPosition;
    }

    public void Open()
    {
        _targetPanel.DOAnchorPosX(_setPostion.x, 0.2f)
            .SetEase(Ease.Linear)
            .SetUpdate(true);
    }

    public void Close()
    {
        _targetPanel.DOAnchorPosX(_offSetPos.x, 0.2f)
            .SetEase(Ease.Linear)
            .SetUpdate(true);
    }

    public override void CallBack(object[] datas = null)
    {
        WindowManager.Instance.OpenRequest("OptionSystem");
    }
}

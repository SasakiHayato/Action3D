using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Lockon : UIWindowParent.UIWindowChild
{
    [SerializeField] Sprite _sprite;
    [SerializeField] Color _color;
    
    GameObject _target;
    GameObject _setUI;

    RectTransform _rect;
    Vector3 _setVec = new Vector3(10, 10);

    public override void SetUp()
    {
        GameObject obj = new GameObject("LockonUI");

        Image image = obj.AddComponent<Image>();
        image.sprite = _sprite;
        image.color = _color;

        _setUI = obj;
        obj.transform.SetParent(ParentPanel.gameObject.transform);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector3.zero;
        rect.localScale = Vector3.one;
        _rect = rect;

        _setUI.SetActive(false);
    }

    public override void UpDate()
    {
        if (!GameManager.Instance.IsLockOn && _setUI.activeSelf)
        {
            _setUI.SetActive(false);
            return;
        }

        var pos = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.transform.position);
        _rect.position = pos;
    }

    public override void CallBack(object[] data)
    {
        if (GameManager.Instance.IsLockOn)
        {
            _target = (GameObject)data[0];
            _setUI.SetActive(true);
            _setUI.transform.localScale = _setVec;

            _setUI.transform
                .DOScale(Vector3.one, 0.1f)
                .SetEase(Ease.Linear)
                .OnComplete(() => { _setUI.transform.DOKill(); });
        }
    }
}

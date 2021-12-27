using UnityEngine;
using UnityEngine.UI;

public class Lockon : UIWindowParent.UIWindowChild
{
    [SerializeField] Sprite _sprite;
    [SerializeField] Color _color;
    bool _isLockOn = false;
    GameObject _target;
    GameObject _setUI;

    Canvas _canvas;
    RectTransform _rect;

    public override void SetUp()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject obj = new GameObject("LockonUI");
        Image image = obj.AddComponent<Image>();
        image.sprite = _sprite;
        image.color = _color;
        _setUI = obj;
        obj.transform.SetParent(_canvas.transform);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector3.zero;
        rect.localScale = Vector3.one;
        _rect = rect;
        _setUI.SetActive(false);
    }

    public override void UpDate()
    {
        if (!_isLockOn) return;

        var pos = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.transform.position);
        _rect.position = pos;
    }

    public override void CallBack(object[] data)
    {
        if (!_isLockOn)
        {
            _isLockOn = true;
            _target = (GameObject)data[0];
            _setUI.SetActive(true);
        }
        else
        {
            _isLockOn = false;
            _setUI.SetActive(false);
        }
    }
}

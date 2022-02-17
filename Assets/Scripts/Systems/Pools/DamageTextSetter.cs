using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// DamageText‚ÌPool
/// </summary>

public class DamageTextSetter : MonoBehaviour, IPool
{
    [SerializeField] float _offSetYPos;
    const float Duration = 1f;

    public bool IsUse { get; private set; } = false;

    RectTransform _rect;
    Text _text;

    public void SetUp(Transform parent)
    {
        _rect = GetComponent<RectTransform>();

        _text = GetComponent<Text>();
        _text.text = "";

        gameObject.SetActive(false);
    }

    public void Use(int damage, Transform target, Color color)
    {
        IsUse = true;
        gameObject.SetActive(true);

        _text.text = damage.ToString();
        _text.color = color;
        _rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);
        _rect.DOAnchorPosY(_rect.position.y + _offSetYPos, Duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => Delete());
    }

    public void Delete()
    {
        gameObject.SetActive(false);
        IsUse = false;
    }
}

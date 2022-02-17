using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextSetter : MonoBehaviour, IPool
{
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

    public void Use(int damage, Transform target)
    {
        gameObject.SetActive(true);

        _text.text = damage.ToString();
        _rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);
    }

    public void Delete()
    {
        gameObject.SetActive(false);
    }
}

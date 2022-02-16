using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextSetter : MonoBehaviour, IPool
{
    public bool IsUse { get; private set; } = false;

    Text _text;

    public void SetUp(Transform parent)
    {
        _text = GetComponent<Text>();
        _text.text = "";
    }

    public void Use(int damage)
    {

    }

    public void Delete()
    {

    }
}

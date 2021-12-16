using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FieldManager : MonoBehaviour
{
    [SerializeField] float _rate;
    [SerializeField] float _time;

    private static FieldManager _instance = null;
    public static FieldManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static void FieldTimeRate(Action<UIType, int, object[]> action, UIType type, int id)
    {
        Instance.StartCoroutine(Instance.SetRate(action, type, id));
    }

    IEnumerator SetRate(Action<UIType, int, object[]> action, UIType type, int id)
    {
        Time.timeScale = 1 / _rate;
        yield return new WaitForSecondsRealtime(_time);
        Time.timeScale = 1;
        action.Invoke(type, id, null);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    private static FieldManager _instance = null;
    public static FieldManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static void FieldTimeRate(float rate, float time)
    {
        Instance.StartCoroutine(Instance.SetRate(rate, time));
    }

    IEnumerator SetRate(float rate, float time)
    {
        Time.timeScale = 1 / rate;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
    }
}

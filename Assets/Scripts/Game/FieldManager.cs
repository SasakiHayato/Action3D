using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FieldManager : MonoBehaviour
{
    [SerializeField] PostProcessProfile _profile;
    Vignette _vignette;
    
    private static FieldManager _instance = null;
    public static FieldManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
        
        _vignette = _profile.GetSetting<Vignette>();
        _vignette.active = false;
    }

    public static void FieldTimeRate(float rate, float time)
    {
        Instance.StartCoroutine(Instance.SetRate(rate, time));
    }

    IEnumerator SetRate(float rate, float time)
    {
        Time.timeScale = 1 / rate;
        _vignette.active = true;
        yield return new WaitForSecondsRealtime(time);
        _vignette.active = false;
        Time.timeScale = 1;
    }
}

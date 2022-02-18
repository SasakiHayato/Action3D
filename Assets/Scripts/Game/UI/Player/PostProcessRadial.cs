using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// PostProcess, •úËóƒuƒ‰[‚Ì•\¦
/// </summary>

public class PostProcessRadial : UIWindowParent.UIWindowChild
{
    [SerializeField] PostProcessProfile _profile;
    [SerializeField] float _fadeSpeed = 1;
    [SerializeField] float _intencity;

    RadialBlur _radial;
    Vignette _vignette;
    float _time = 0;

    public override void SetUp()
    {
        _radial = Object.FindObjectOfType<RadialBlur>();
        _vignette = _profile.GetSetting<Vignette>();
        _vignette.active = false;
    }

    public override void UpDate()
    {
        float set = 0;
        _time += Time.unscaledDeltaTime * _fadeSpeed;
        
        if (_vignette.active)
        {
            set = Mathf.Lerp(0, _intencity, _time);
            _radial.SetStrength = set * 3;
        }
        else
        {
            set = Mathf.Lerp(_intencity, 0, _time);
            if (set <= 0) _vignette.active = false;
            _radial.SetStrength = 0;
        }

        _vignette.intensity.value = set;
    }

    public override void CallBack(object[] data)
    {
        _time = 0;
        if (!_vignette.active) _vignette.active = true;
        else _vignette.active = false;
    }
}

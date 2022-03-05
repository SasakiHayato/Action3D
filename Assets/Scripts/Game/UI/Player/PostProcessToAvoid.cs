using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// PostProcess, ‰ñ”ðŽž‚ÌEffect
/// </summary>

public class PostProcessToAvoid : ChildrenUI
{
    [SerializeField] PostProcessProfile _profile;
    [SerializeField] float _fadeSpeed = 1;
    [SerializeField] float _intencity;

    RadialBlur _radial;
    Vignette _vignette;
    ColorGrading _grading;

    bool _isEffect = false;
    float _time = 0;

    const float MaxEffectVal = -50;

    public override void SetUp()
    {
        _radial = FindObjectOfType<RadialBlur>();

        _vignette = _profile.GetSetting<Vignette>();
        _vignette.active = false;

        _grading = _profile.GetSetting<ColorGrading>();
        _grading.active = false;
    }

    private void Update()
    {
        float vignetteVal = 0;
        float gradingVal = 0;

        _time += Time.unscaledDeltaTime * _fadeSpeed;

        if (_isEffect)
        {
            vignetteVal = Mathf.Lerp(0, _intencity, _time);
            gradingVal = Mathf.Lerp(0, MaxEffectVal, _time);

            _radial.SetStrength = vignetteVal * 3;
        }
        else
        {
            vignetteVal = Mathf.Lerp(_intencity, 0, _time);
            gradingVal = Mathf.Lerp(MaxEffectVal, 0, _time);

            if (vignetteVal <= 0)
            {
                _isEffect = false;

                _vignette.active = false;
                _grading.active = false;
            }
            _radial.SetStrength = 0;
        }

        _vignette.intensity.value = vignetteVal;
        _grading.saturation.value = gradingVal;
    }

    public override void CallBack(object[] data)
    {
        _time = 0;
        if (!_isEffect)
        {
            _isEffect = true;

            _vignette.active = true;
            _grading.active = true;
        }
        else
        {
            _isEffect = false;

            _vignette.active = false;
            _grading.active = false;
        }
    }
}

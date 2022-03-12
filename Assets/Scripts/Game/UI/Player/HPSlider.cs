using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// åªç›ÇÃPlayerÇÃHPÇÃï\é¶
/// </summary>

public class HPSlider : ChildrenUI
{
    [SerializeField] float _slideSpeed = 1;
    Slider _slider;
    Player _player;

    int _saveHp;
    float _timer = 0;

    public override void SetUp()
    {
        _player = FindObjectOfType<Player>();
        _slider = GetComponent<Slider>();
        _slider.maxValue = _player.HP;
        _slider.value = _player.HP;
        _saveHp = _player.HP;
    }

    private void Update()
    {
        if (_saveHp != _player.HP)
        {
            _timer += Time.deltaTime * _slideSpeed;
            float rate = Mathf.Lerp(_saveHp, _player.HP, _timer);
            _slider.value = rate;

            if (_player.HP == rate)
            {
                _timer = 0;
                _saveHp = _player.HP;
            }
        }
    }

    public override void CallBack(object[] data)
    {
        _slider.maxValue = _player.HP;
        _slider.value = _player.HP;
        _saveHp = _player.HP;
    }
}

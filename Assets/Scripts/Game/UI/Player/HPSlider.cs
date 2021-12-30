using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSlider : UIWindowParent.UIWindowChild
{
    [SerializeField] string _sliderName;
    [SerializeField] float _slideSpeed = 1;
    Slider _slider;
    Player _player;

    int _saveHp;
    float _timer = 0;

    public override void SetUp()
    {
        _player = Object.FindObjectOfType<Player>();
        _slider = GameObject.Find(_sliderName).GetComponent<Slider>();
        _slider.maxValue = _player.GetHP;
        _slider.value = _player.GetHP;
        _saveHp = _player.GetHP;
    }

    public override void UpDate()
    {
        if (_saveHp != _player.GetHP)
        {
            _timer += Time.deltaTime * _slideSpeed;
            float rate = Mathf.Lerp(_saveHp, _player.GetHP, _timer);
            _slider.value = rate;

            if (_player.GetHP == rate)
            {
                _timer = 0;
                _saveHp = _player.GetHP;
            }
        }
    }

    public override void CallBack(object[] data)
    {
        
    }
}

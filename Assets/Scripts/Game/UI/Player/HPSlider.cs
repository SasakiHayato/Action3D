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
        _slider = ParentPanel.gameObject.transform.Find(_sliderName).GetComponent<Slider>();
        _slider.maxValue = _player.HP;
        _slider.value = _player.HP;
        _saveHp = _player.HP;
    }

    public override void UpDate()
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
        
    }
}

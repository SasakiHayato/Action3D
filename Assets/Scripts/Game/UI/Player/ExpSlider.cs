using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���݂̌o���l�̕\��
/// </summary>

public class ExpSlider : UIWindowParent.UIWindowChild
{
    [SerializeField] string _sliderName;
    Slider _slider;

    public override void SetUp()
    {
        _slider = GameObject.Find(_sliderName).GetComponent<Slider>();
        _slider.maxValue = GameManager.Instance.PlayerData.NextLevelExp;
        _slider.value = 0;
    }

    public override void UpDate()
    {
        _slider.value = GameManager.Instance.PlayerData.CurrentExp;
    }

    public override void CallBack(object[] data)
    {
        _slider.maxValue = GameManager.Instance.PlayerData.NextLevelExp;
    }
}

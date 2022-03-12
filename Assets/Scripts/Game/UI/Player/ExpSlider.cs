using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���݂̌o���l�̕\��
/// </summary>

public class ExpSlider : ChildrenUI
{
    Slider _slider;

    public override void SetUp()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = GameManager.Instance.PlayerData.NextLevelExp;
        _slider.value = 0;
    }

    private void Update()
    {
        _slider.value = GameManager.Instance.PlayerData.CurrentExp;
    }

    public override void CallBack(object[] data)
    {
        _slider.maxValue = GameManager.Instance.PlayerData.NextLevelExp;
    }
}

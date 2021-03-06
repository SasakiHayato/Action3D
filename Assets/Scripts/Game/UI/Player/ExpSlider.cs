using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 現在の経験値の表示
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

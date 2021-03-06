using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Playerの現在のレベルの表示
/// </summary>

public class LevelTextSetter : ChildrenUI
{
    [SerializeField] Text _txt;

    public override void SetUp()
    {
        _txt.text = "Level : 001";
    }

    public override void CallBack(object[] data)
    {
        int level = (int)data[0];
        _txt.text = $"Level : {level.ToString("000")}";
    }
}

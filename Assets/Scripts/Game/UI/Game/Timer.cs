using UnityEngine.UI;

/// <summary>
/// Œ»İ‚ÌƒQ[ƒ€‚ÌŒo‰ßŠÔ‚Ì•\¦
/// </summary>

public class Timer : ChildrenUI
{
    Text _txt;

    public override void SetUp()
    {
        _txt = GetComponentInChildren<Text>();
    }

    void Update()
    {
        float totaltime = GameManager.Instance.GetCurrentTime;
        int minutes = (int)totaltime / 60;
        float second = totaltime - minutes * 60;
        
        _txt.text = $"Time ; {minutes.ToString("00")}:{second.ToString($"00.00")}";
    }

    public override void CallBack(object[] data) { }
}

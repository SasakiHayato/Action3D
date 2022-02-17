using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���݂̃Q�[���̌o�ߎ��Ԃ̕\��
/// </summary>

public class Timer : UIWindowParent.UIWindowChild
{
    [SerializeField] string _name;
    Text _txt;

    public override void SetUp()
    {
        _txt = ParentPanel.gameObject.transform.Find(_name).GetComponentInChildren<Text>();
    }

    public override void UpDate()
    {
        float totaltime = GameManager.Instance.GetCurrentTime;
        int minutes = (int)totaltime / 60;
        float second = totaltime - minutes * 60;

        _txt.text = $"Time ; {minutes.ToString("00")}:{second.ToString($"00.00")}";
    }

    public override void CallBack(object[] data) { }
}

using UnityEngine;
using UnityEngine.UI;

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
        float time = GameManager.Instance.GetCurrentTime;
        _txt.text = $"Time ; {time.ToString("00:00.00")}";
    }

    public override void CallBack(object[] data)
    {
        UpDate();
    }
}

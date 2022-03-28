
public class OptionUI : ParentUI
{
    public override void SetUp()
    {
        base.SetUp();
    }

    public override void CallBack(object[] datas)
    {
        WindowManager.Instance.CreateWindowList(null, null, "")
            .AddEvents(null, null);
    }
}

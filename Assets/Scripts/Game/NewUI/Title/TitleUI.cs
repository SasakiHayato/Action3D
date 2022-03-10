/// <summary>
/// UIType.Title‚É‘Î‚·‚éPanel‚ÌŠÇ—ƒNƒ‰ƒX
/// </summary>

public class TitleUI : ParentUI
{
    public override void SetUp()
    {
        base.SetUp();
        if (GameManager.GameState.Title != GameManager.Instance.CurrentGameState) Active(false);
    }

    public override void CallBack(object[] datas)
    {
        
    }
}

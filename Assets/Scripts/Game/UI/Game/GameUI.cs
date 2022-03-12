/// <summary>
/// UIType.Game‚É‘Î‚·‚éPanel‚ÌŠÇ—ƒNƒ‰ƒX
/// </summary>

public class GameUI : ParentUI
{
    public override void SetUp()
    {
        base.SetUp();

        if (GameManager.GameState.InGame != GameManager.Instance.CurrentGameState)
        {
            Active(false);
            return;
        }
    }

    public override void CallBack(object[] datas)
    {
        
    }
}

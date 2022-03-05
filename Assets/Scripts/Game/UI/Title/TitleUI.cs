/// <summary>
/// UIType.Titleに対するPanelの管理クラス
/// </summary>

public class TitleUI : ParentUI
{
    public override void SetUp()
    {
        base.SetUp();
        if (GameManager.GameState.Title != GameManager.Instance.CurrentGameState) Active(false);
    }
}

/// <summary>
/// Enemy関連のUI情報の管理クラス
/// </summary>

public class EnemyConnectUI : ParentUI
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

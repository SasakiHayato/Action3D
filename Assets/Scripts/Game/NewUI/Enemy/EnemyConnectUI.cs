/// <summary>
/// EnemyŠÖ˜A‚ÌUIî•ñ‚ÌŠÇ—ƒNƒ‰ƒX
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
}

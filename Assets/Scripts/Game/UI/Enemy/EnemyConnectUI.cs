/// <summary>
/// Enemy�֘A��UI���̊Ǘ��N���X
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

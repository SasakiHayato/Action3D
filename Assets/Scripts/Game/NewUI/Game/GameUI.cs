/// <summary>
/// UIType.Game�ɑ΂���Panel�̊Ǘ��N���X
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

/// <summary>
/// UIType.Title�ɑ΂���Panel�̊Ǘ��N���X
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

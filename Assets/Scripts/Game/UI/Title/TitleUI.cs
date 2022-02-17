/// <summary>
/// UIType.Title‚É‘Î‚·‚éPanel‚ÌŠÇ—ƒNƒ‰ƒX
/// </summary>

public class TitleUI : UIWindowParent
{
    bool _isRun = false;

    public override void SetUp()
    {
        if (GameManager.GameState.Title != GameManager.Instance.CurrentGameState)
        {
            GetPanel.gameObject.SetActive(false);
            _isRun = false;
            return;
        }

        _isRun = true;
        base.SetUp();
    }

    public override void UpDate()
    {
        if (!_isRun) return;
        base.UpDate();
    }

    public override void CallBack(int id, object[] data)
    {
        if (!_isRun) return;
        base.CallBack(id, data);
    }
}

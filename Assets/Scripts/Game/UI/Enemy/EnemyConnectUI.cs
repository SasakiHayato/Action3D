/// <summary>
/// EnemyŠÖ˜A‚ÌUIî•ñ‚ÌŠÇ—ƒNƒ‰ƒX
/// </summary>

public class EnemyConnectUI : UIWindowParent
{
    bool _isRun = false;

    public override void SetUp()
    {
        if (GameManager.GameState.InGame != GameManager.Instance.CurrentGameState)
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

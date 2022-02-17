using UnityEngine;

/// <summary>
/// UIType.Player‚É‘Î‚·‚éPanel‚ÌŠÇ—ƒNƒ‰ƒX
/// </summary>

public class PlayerUI : UIWindowParent
{
    GameObject _player = null;
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
        _player = GameObject.FindGameObjectWithTag("Player");
        base.SetUp();
    }

    public override void UpDate()
    {
        if (_player == null || !_isRun) return;
        base.UpDate();
    }

    public override void CallBack(int id, object[] data)
    {
        if (_player == null || !_isRun) return;
        base.CallBack(id, data);
    }
}

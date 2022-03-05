using UnityEngine;

/// <summary>
/// UIType.Player‚É‘Î‚·‚éPanel‚ÌŠÇ—ƒNƒ‰ƒX
/// </summary>

public class PlayerUI : ParentUI
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

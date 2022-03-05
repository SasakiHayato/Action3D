using UnityEngine;

/// <summary>
/// UIType.Player�ɑ΂���Panel�̊Ǘ��N���X
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

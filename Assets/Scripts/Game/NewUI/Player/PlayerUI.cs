using UnityEngine;

/// <summary>
/// UIType.Playerに対するPanelの管理クラス
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

    public override void CallBack(object[] datas)
    {
        
    }
}

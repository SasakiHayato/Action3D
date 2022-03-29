using UnityEngine;

/// <summary>
/// InputSystemのデータ管理クラス
/// </summary>

public class Inputter : SingletonAttribute<Inputter>
{
    public InputData Inputs { get => _inputs; }
    InputData _inputs;

    public void Load()
    {
        _inputs = new InputData();
        _inputs.Enable();
        
        Inputs.UI.Check.started += context => IsSelectButton();
        Inputs.UI.Cancel.started += context => Cancel();

        if (GameManager.GameState.Title != GameManager.Instance.CurrentGameState)
        {
            Inputs.UI.Options.started += context => WindowManager.Instance.Request("Option");
        }
    }

    public static void Init()
    {
        Instance._inputs.Dispose();
    }

    public static object GetValue(InputType type)
    {
        object obj = null;
        switch (type)
        {
            case InputType.PlayerMove:
                obj = Instance._inputs.Player.Move.ReadValue<Vector2>();
                break;
            case InputType.CmMove:
                obj = Instance._inputs.Player.MoveCm.ReadValue<Vector2>();
                break;
            case InputType.Select:
                obj = Instance._inputs.UI.Select.ReadValue<Vector2>();
                break;
            case InputType.ShotVal:
                obj = Instance._inputs.Player.Shot.ReadValue<float>();
                break;
        }

        return obj;
    }

    void IsSelectButton()
    {
        if (GameManager.Option.Open != GameManager.Instance.OptionState) return;

        WindowManager.Instance.IsSelect();
    }

    void Cancel()
    {
        if (GameManager.Option.Open != GameManager.Instance.OptionState) return;

        WindowManager.Instance.CloseRequest();
    }
}

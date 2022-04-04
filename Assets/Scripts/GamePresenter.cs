using UnityEngine;

/// <summary>
/// シーン開始時のゲームのセットアップ
/// </summary>

public class GamePresenter : MonoBehaviour
{
    [SerializeField] GameManager.GameState _gameState;
    [SerializeField] GameManager.FieldType _inGamefieldType;

    void Awake()
    {
        GameManager gameManager = new GameManager();
        GameManager.SetInstance(gameManager, gameManager);
        GameManager.Instance.SetGameState = _gameState;

        Inputter inputter = new Inputter();
        Inputter.SetInstance(inputter, inputter);
        Inputter.Instance.Load();

        WindowManager windowManager = new WindowManager();
        WindowManager.SetInstance(windowManager, windowManager);
        WindowManager.Instance.SetUp();

        BaseUI baseUI = new BaseUI();
        BaseUI.SetInstance(baseUI, baseUI);
    }

    void Start()
    {
        GameManager.Instance.InGameFieldType = _inGamefieldType;
        GameManager.Instance.GameStateSetUpSystems(_gameState);
        GameManager.Instance.GameStateSetUpEvents(_gameState);
    }

    void Update()
    {
        if (GameManager.Instance.OptionState == GameManager.Option.Open)
        {
            WindowManager.Instance.Selecting();
        }

        GameManager.Instance.GameTime();
        Inputter.Instance.CheckConnectGamePad();
    }
}

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

        Inputter inputter = new Inputter();
        Inputter.SetInstance(inputter, inputter);
        Inputter.Instance.Load();

        GamePadButtonEvents gamePadButtonEvents = new GamePadButtonEvents();
        GamePadButtonEvents.SetInstance(gamePadButtonEvents, gamePadButtonEvents);

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
            GamePadButtonEvents.Instance.Select();
            GamePadButtonEvents.Instance.SelectChangeScale(Vector2.one, Vector2.one * 1.2f);
        }
    }
}

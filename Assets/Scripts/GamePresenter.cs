using UnityEngine;

/// <summary>
/// �V�[���J�n���̃Q�[���̃Z�b�g�A�b�v
/// </summary>

public class GamePresenter : MonoBehaviour
{
    [SerializeField] GameManager.GameState _gameState;
    [SerializeField] GameManager.FieldType _inGamefieldType;

    void Awake()
    {
        GameManager gameManager = new GameManager();
        GameManager.SetInstance(gameManager, gameManager);

        GamePadButtonEvents gamePadButtonEvents = new GamePadButtonEvents();
        GamePadButtonEvents.SetInstance(gamePadButtonEvents, gamePadButtonEvents);
    }

    void Start()
    {
        GameManager.Instance.InGameFieldType = _inGamefieldType;
        GameManager.Instance.GameStateSetUpSystems(_gameState);
        GameManager.Instance.GameStateSetUpEvents(_gameState);
    }
}

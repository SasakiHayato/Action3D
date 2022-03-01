using UnityEngine;

/// <summary>
/// シーン開始時のゲームのセットアップ
/// </summary>

public class GameStatePresenter : MonoBehaviour
{
    [SerializeField] GameManager.GameState _gameState;
    [SerializeField] GameManager.FieldType _inGamefieldType;
    
    void Start()
    {
        GameManager.Instance.InGameFieldType = _inGamefieldType;
        GameManager.Instance.GameStateSetUpSystems(_gameState);
        GameManager.Instance.GameStateSetUpEvents(_gameState);
    }
}

using UnityEngine;

/// <summary>
/// �V�[���J�n���̃Q�[���̃Z�b�g�A�b�v
/// </summary>

public class GameStatePresenter : MonoBehaviour
{
    [SerializeField] GameManager.GameState _gameState;

    void Start()
    {
        GameManager.Instance.GameStateSetUp(_gameState);
    }
}

using UnityEngine;

public class GameStatePresenter : MonoBehaviour
{
    [SerializeField] GameManager.GameState _gameState;

    void Start()
    {
        GameManager.Instance.GameStateSetUp(_gameState);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

/// <summary>
/// シーン遷移の管理クラス
/// </summary>

public class SceneSettings : MonoBehaviour
{
    private static SceneSettings _instance = null;
    public static SceneSettings Instance
    {
        get
        {
            object instance = FindObjectOfType(typeof(SceneSettings));

            if (instance == null)
            {
                GameObject obj = new GameObject("SceneSettings");
                _instance = obj.AddComponent<SceneSettings>();
                obj.hideFlags = HideFlags.HideInHierarchy;
            }
            else
            {
                _instance = (SceneSettings)instance;
            }

            return _instance;
        }
    }

    AsyncOperation _operation = null;
    
    public void LoadSync(string name, Action action = null)
    {
        DOTween.KillAll();
        action?.Invoke();
        GameManager.Instance.GameStateSetUpEvents(GameManager.GameState.Load);
        SceneManager.LoadScene(name);
    }

    public void LoadSync(int id, Action action = null)
    {
        DOTween.KillAll();
        action?.Invoke();
        GameManager.Instance.GameStateSetUpEvents(GameManager.GameState.Load);
        SceneManager.LoadScene(id);
    }

    public void LoadAsync(string name, float waitTime, Action action = null)
    {
        DOTween.KillAll();
        _operation = null;

        _operation = SceneManager.LoadSceneAsync(name);
        _operation.allowSceneActivation = false;
        StartCoroutine(nameof(Async), action);
    }

    public void LoadAsync(int id, float waitTime, Action action = null)
    {
        DOTween.KillAll();
        _operation = null;

        _operation = SceneManager.LoadSceneAsync(id);
        _operation.allowSceneActivation = false;

        StartCoroutine(Async(waitTime, action));
    }

    IEnumerator Async(float waitTime, Action action)
    {
        action?.Invoke();
        yield return new WaitForSeconds(waitTime);
        bool isDone = false;
        GameManager.Instance.GameStateSetUpEvents(GameManager.GameState.Load);

        while (!isDone)
        {
            if (_operation.progress >= 0.9f) isDone = true;
            yield return null;
        }
        
        _operation.allowSceneActivation = true;
    }
}

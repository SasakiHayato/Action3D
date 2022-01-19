using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    // Singleton
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null) _instance = new GameManager();
            return _instance;
        }
    }

    bool _islockOn = false;
    public bool IsLockOn
    {
        get
        {
            if (Instance.LockonTarget != null) return _islockOn;
            else return false;
        }
        set
        {
            _islockOn = value;
            if (!_islockOn) Instance.LockonTarget = null;
        }
    }

    public float GetCurrentTime { get; private set; } = 0;
    
    public GameObject LockonTarget { get; set; }
    public void End()
    {
        Instance.IsLockOn = false;
        Inputter.Init();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameTime()
    {
        Instance.GetCurrentTime += Time.deltaTime;
    }

    public static void Init()
    {
        Instance._islockOn = false;
        Instance.GetCurrentTime = 0;
    }

    public GameObject Player { get; set; }
    public void AddExp(int exp, int level)
    {
        float add = ((float)level / 10) - 0.1f;
        int set = exp + (int)(exp * add);
    }
}

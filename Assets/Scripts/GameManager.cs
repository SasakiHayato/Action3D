using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    bool _islockOn = false;
    public GameObject LockonTarget { get; set; }
    public float GetCurrentTime { get; private set; } = 0;

    public PlayerData PlayerData { get; private set; } = new PlayerData();

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

    /// <summary>
    /// ゲーム終了時の諸々の初期化
    /// </summary>
    public void End()
    {
        Init();
        Inputter.Init();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// ゲームの現在の経過時間
    /// </summary>
    public void GameTime()
    {
        Instance.GetCurrentTime += Time.deltaTime;
    }
   
    /// <summary>
    /// 受け取った経験値を計算させる
    /// </summary>
    /// <param name="exp">受け取った経験値</param>
    /// <param name="level">受け取った対象のレベル</param>
    public void GetExp(int exp, int level)
    {
        int set = exp;
        if (level > 1)
        {
            float add = ((float)level / 10) - 0.1f;
            set = exp + (int)(exp * add);
        }

        AddExp(PlayerData.CurrentExp += set);
    }

    void AddExp(int currentExp)
    {
        if (currentExp >= PlayerData.NextLevelExp)
        {
            // Log
            UIManager.CallBack(UIType.Game, 3, new object[] { 1 });

            int set = currentExp - PlayerData.NextLevelExp;
            PlayerData.NextLevelExp += 100;

            int hp = PlayerData.HP;
            int power = PlayerData.Power;
            float speed = PlayerData.Player.Speed;
            int level = PlayerData.Player.Level + 1;
            PlayerData.Player.SetParam(hp, power, speed, level);

            // ExpSlider
            UIManager.CallBack(UIType.Player, 5, null);

            AddExp(PlayerData.CurrentExp = set);
        }
        else
        {
            // HpSlider
            UIManager.CallBack(UIType.Player, 4, new object[] { PlayerData.Player.Level });
            // Leveltext
            UIManager.CallBack(UIType.Player, 3, new object[] { PlayerData.Player.HP });
            Sounds.SoundMaster.Request(null, "LevelUp", 0);
        }
    }

    void Init()
    {
        Instance.IsLockOn = false;
        Instance.GetCurrentTime = 0;
        Instance.PlayerData = new PlayerData();
    }
}

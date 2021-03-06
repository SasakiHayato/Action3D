using UnityEngine;
using System.Collections.Generic;
using Sounds;

/// <summary>
/// ゲーム全体の管理クラス
/// </summary>

public class GameManager : SingletonAttribute<GameManager>
{
    public enum GameState
    {
        InGame,
        Title,
        Dead,
        Load,
        End,
    }

    public enum FieldType
    {
        Warld,
        Arena,

        None,
    }

    public enum Option
    {
        Open,
        Close,
    }

    public GameState CurrentGameState { get; private set; }
    public GameState SetGameState { set { CurrentGameState = value; } }
    public Option OptionState { get; private set; } = Option.Close;

    bool _islockOn = false;
    public GameObject LockonTarget { get; set; }
    public float GetCurrentTime { get; private set; } = 0;

    public PlayerData PlayerData { get; private set; } = new PlayerData();

    public List<IAttackCollision> AttackCollisions { get; set; } = new List<IAttackCollision>();

    public FieldType InGameFieldType { get; set; }

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
    /// 対象となるステートの各マネージャーの生成
    /// </summary>
    /// <param name="state">ロードするステート</param>
    public void GameStateSetUpSystems(GameState state)
    {
        CurrentGameState = state;

        Object.Instantiate((GameObject)Resources.Load("Systems/SoundMaster"));

        switch (state)
        {
            case GameState.InGame:
                BaseUI.Instance.Load();
                Object.Instantiate((GameObject)Resources.Load("Systems/FieldSystems"));
                Object.Instantiate((GameObject)Resources.Load("Systems/ItemManager"));
                Object.Instantiate((GameObject)Resources.Load("Systems/BulletSettings"));

                if (FieldType.Warld == Instance.InGameFieldType)
                {
                    SoundMaster.PlayRequest(null, "FieldBGM", SEDataBase.DataType.BGM);
                }
                else
                {
                    SoundMaster.PlayRequest(null, "ArenaBGM", SEDataBase.DataType.BGM);
                }
               
                break;

            case GameState.Title:
                SoundMaster.PlayRequest(null, "TitleBGM", SEDataBase.DataType.BGM);
                BaseUI.Instance.Load();
                BaseUI.Instance.ParentActive("Title", true);
                break;

            case GameState.Dead:
                break;

            case GameState.Load:
                BaseUI.Instance.Load();
                break;

            case GameState.End:
                break;
        }
    }

    /// <summary>
    /// 対象となるステートの各Eventの発生管理
    /// </summary>
    /// <param name="state">SetUpをするState</param>
    public void GameStateSetUpEvents(GameState state)
    {
        switch (state)
        {
            case GameState.InGame:
                
                Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                
                if (player.AnimController == null)
                {
                    player.SetUp();
                }
                
                player.AnimController.RequestAnimCallBackEvent("Intro", () => { PlayerData.CanMove = true; });
                SetOptionState(Option.Close);
                
                break;

            case GameState.Title:
                
                Fader.Instance.Request(Fader.FadeType.In, 0.25f);
                SetOptionState(Option.Open);
                
                break;

            case GameState.Dead:
                PlayerData.CanMove = false;
                Fader.Instance.Request(Fader.FadeType.Out, 0.5f);
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                player.AnimController.RequestAnimCallBackEvent("Damage_Die", () => SceneSettings.Instance.LoadSync(0));
                break;

            case GameState.Load:
                End();
                break;

            case GameState.End:
                Fader.Instance.Request(Fader.FadeType.Out, 1f);
                SceneSettings.Instance.LoadAsync(0, 1);
                break;
        }
    }

    /// <summary>
    /// optionを開いた際のゲームシーンの管理
    /// </summary>
    /// <param name="option">OptionState</param>
    public void SetOptionState(Option option)
    {
        OptionState = option;

        switch (option)
        {
            case Option.Open:

                if (GameState.Title != CurrentGameState)
                {
                    Time.timeScale = 0;
                }
                
                break;

            case Option.Close:
                Time.timeScale = 1;
                
                break;
        }
    }

    /// <summary>
    /// ゲーム終了時の諸々の初期化
    /// </summary>
    public void End()
    {
        Init();
        Inputter.Instance.Init();
    }

    /// <summary>
    /// ゲームの現在の経過時間
    /// </summary>
    public void GameTime()
    {
        Instance.GetCurrentTime += Time.deltaTime;
    }

    public void AddIAttackCollision(IAttackCollision collision)
    {
        AttackCollisions.Add(collision);
    }

    public void RemoveAttackCollsion(IAttackCollision collision)
    {
        AttackCollisions.Remove(collision);
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
            BaseUI.Instance.CallBack("Game", "Log", new object[] { 1 });
            
            int set = currentExp - PlayerData.NextLevelExp;
            PlayerData.NextLevelExp += 100;

            int hp = PlayerData.HP;
            int power = PlayerData.Power;
            float speed = PlayerData.Player.Speed;
            int level = PlayerData.Player.Level + 1;
            PlayerData.Player.SetParam(hp, power, speed, level);

            // ExpSlider
            BaseUI.Instance.CallBack("Player", "Exp");
            
            AddExp(PlayerData.CurrentExp = set);
        }
        else
        {
            // HpSlider
            BaseUI.Instance.CallBack("Player", "HP");
            // Leveltext
            BaseUI.Instance.CallBack("Player", "Level", new object[] { PlayerData.Player.Level });
            SoundMaster.PlayRequest(null, "LevelUp", 0);
        }
    }

    void Init()
    {
        Instance.IsLockOn = false;
        Instance.GetCurrentTime = 0;
        Instance.PlayerData = new PlayerData();
        Instance.AttackCollisions = new List<IAttackCollision>();
    }
}

using UnityEngine;

/// <summary>
/// �Q�[���S�̂̊Ǘ��N���X
/// </summary>

public class GameManager
{
    public enum GameState
    {
        InGame,
        Title,
        Dead,
        Load,
    }

    public GameState CurrentGameState { get; private set; }

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
    /// �ΏۂƂȂ�X�e�[�g�̊e�}�l�[�W���[�̐���
    /// </summary>
    /// <param name="state">���[�h����X�e�[�g</param>
    public void GameStateSetUp(GameState state)
    {
        CurrentGameState = state;
        
        switch (state)
        {
            case GameState.InGame:
                Object.Instantiate((GameObject)Resources.Load("Systems/UIManager"));
                Object.Instantiate((GameObject)Resources.Load("Systems/FieldManager"));
                Object.Instantiate((GameObject)Resources.Load("Systems/ItemManager"));
                Object.Instantiate((GameObject)Resources.Load("Systems/SoundMaster"));
                Object.Instantiate((GameObject)Resources.Load("Systems/BulletSettings"));
                break;

            case GameState.Title:
                Object.Instantiate((GameObject)Resources.Load("Systems/SoundMaster"));
                Object.Instantiate((GameObject)Resources.Load("Systems/UIManager"));
                Object.Instantiate((GameObject)Resources.Load("Systems/TitleManager")); ;
                break;

            case GameState.Dead:
                break;

            case GameState.Load:
                UnityEngine.Object.Instantiate((GameObject)Resources.Load("Systems/UIManager"));
                break;
        }
    }

    /// <summary>
    /// �Q�[���I�����̏��X�̏�����
    /// </summary>
    public void End()
    {
        Init();
        Inputter.Init();
        SceneSettings.Instance.LoadSync(0);
    }

    /// <summary>
    /// �Q�[���̌��݂̌o�ߎ���
    /// </summary>
    public void GameTime()
    {
        Instance.GetCurrentTime += Time.deltaTime;
    }
   
    /// <summary>
    /// �󂯎�����o���l���v�Z������
    /// </summary>
    /// <param name="exp">�󂯎�����o���l</param>
    /// <param name="level">�󂯎�����Ώۂ̃��x��</param>
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

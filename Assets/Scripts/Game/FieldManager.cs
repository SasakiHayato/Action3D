using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnemysData;
using Sounds;

/// <summary>
/// Fieldの管理クラス
/// </summary>

public class FieldManager : MonoBehaviour
{
    [SerializeField] EnemyMasterData _enemyMasterData;
    [SerializeField] int _updateTime;
    [SerializeField] float _rate;
    [SerializeField] float _time;
    
    FieldData _fieldData = new FieldData();
    public FieldData FieldData => _fieldData;
    
    // どこからでも呼べるように
    private static FieldManager _instance = null;
    public static FieldManager Instance => _instance;

    [SerializeField] List<SpawnData> _worldSpownDatas;
    [SerializeField] List<SpawnData> _arenaSpawnDatas;

    [Serializable]
    public class SpawnData
    {
        public int ID;
        public int Level;
        public Transform Point;
        public float Range;
        public EnemyGroupTip GroupTip;
    }

    ObjectPool<ParticleUser> _hitParticlePool = new ObjectPool<ParticleUser>();
    public ObjectPool<ParticleUser> GetHitParticle => _hitParticlePool;

    ObjectPool<ParticleUser> _deadParticlePool = new ObjectPool<ParticleUser>();
    public ObjectPool<ParticleUser> GetDeadParticle => _deadParticlePool;

    ObjectPool<ParticleUser> _explosionParticlePool = new ObjectPool<ParticleUser>();

    ObjectPool<ParticleUser> _ripplesParticlePool = new ObjectPool<ParticleUser>();
    public ObjectPool<ParticleUser> GetRipplesParticle => _ripplesParticlePool;

    int _setUpdateTime;

    const float FirstSetUpArena = 10;
    bool _isSetUpArena = false;

    private void Awake()
    {
        _instance = this;

        GameObject hitParticle = (GameObject)Resources.Load("Particle/HitParticle");
        _hitParticlePool.SetUp(hitParticle.GetComponent<ParticleUser>(), transform, 10);

        GameObject deadParticle = (GameObject)Resources.Load("Particle/DeadParticle");
        _deadParticlePool.SetUp(deadParticle.GetComponent<ParticleUser>(), transform, 5);

        GameObject explosionParticle = (GameObject)Resources.Load("Particle/PlasmaExplosionEffect");
        _explosionParticlePool.SetUp(explosionParticle.GetComponent<ParticleUser>(), transform, 5);

        GameObject ripplesParticle = (GameObject)Resources.Load("Particle/RipplesParticle");
        _ripplesParticlePool.SetUp(ripplesParticle.GetComponent<ParticleUser>(), transform, 5);
    }

    void Start()
    {
        if (GameManager.Instance.InGameFieldType == GameManager.FieldType.Warld)
        {
            _fieldData = new FieldData(_worldSpownDatas, _enemyMasterData);
            _fieldData.UpdateEnemy();

            _setUpdateTime = _updateTime;
        }
        else
        {
            _fieldData = new FieldData(_arenaSpawnDatas, _enemyMasterData);
        }
    }

    void Update()
    {
        if (GameManager.Instance.InGameFieldType == GameManager.FieldType.Warld)
        {
            UpDateWorld();
        }
        else
        {
            UpDateArena();
        }
    }

    void UpDateWorld()
    {
        if (GameManager.Instance.GetCurrentTime > _updateTime)
        {
            _updateTime += _setUpdateTime;
            _fieldData.Update();
            BaseUI.Instance.CallBack("Game", "Log", new object[] { 0 });
        }
    }

    void UpDateArena()
    {
        if (GameManager.Instance.GetCurrentTime > FirstSetUpArena && !_isSetUpArena)
        {
            _isSetUpArena = true;
            _fieldData.UpdateEnemy();
        }
        else
        {
            if (_isSetUpArena) _fieldData.SetUpArena();
        }
    }

    /// <summary>
    /// スローモーションの設定
    /// </summary>
    /// <param name="action">スローモーションが終わった際に実行する関数</param>
    public static void FieldTimeRate(Action action)
    {
        SoundMaster.PlayRequest(null, "StartSlowMotion", SEDataBase.DataType.Field);
        Instance.StartCoroutine(Instance.SetRate(action));
    }

    IEnumerator SetRate(Action action)
    {
        Time.timeScale = 1 / _rate;
        yield return new WaitForSecondsRealtime(_time);
        Time.timeScale = 1;

        if (action != null) action.Invoke();
    }

    /// <summary>
    /// Objectの爆発を申請
    /// </summary>
    /// <param name="data">対象のEnemyData</param>
    /// <param name="position">爆発させるPosition</param>
    /// <param name="level">対象のLevel</param>
    public static void RequestExprosion(EnemyData data, Vector3 position, int level)
    {
        ItemBase itemBase = ItemManager.Instance.RequestItem("ItemExp");

        GameObject obj = Instantiate(data.DummyPrefab);
        obj.transform.position = position;

        object[] datas = new object[] { data.Exp, level };

        Instance.StartCoroutine(Instance.WaitExprosionAction(obj, itemBase, datas));
    }

    IEnumerator WaitExprosionAction(GameObject obj, ItemBase itemBase, object[] datas)
    {
        yield return null;
        yield return new WaitAnim(obj.GetComponent<Animator>());

        ParticleUser particle = Instance._explosionParticlePool.Respons();
        particle.Use(obj.transform);

        SoundMaster.PlayRequest(null, 1, SEDataBase.DataType.Field);
        if (GameManager.FieldType.Arena != GameManager.Instance.InGameFieldType)
        {
            for (int i = 0; i < 5; i++)
            {
                ItemExp itemExp = Instantiate(itemBase).GetComponent<ItemExp>();
                itemExp.SetOtherData(datas);
                itemExp.gameObject.transform.position = obj.transform.position;
                itemExp.Force(obj.transform.position);
            }
        }

        Destroy(obj);
    }
}
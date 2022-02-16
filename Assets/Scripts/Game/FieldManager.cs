using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnemysData;
using Sounds;

public class FieldManager : MonoBehaviour
{
    [SerializeField] EnemyMasterData _enemyMasterData;
    [SerializeField] int _updateTime;
    [SerializeField] float _rate;
    [SerializeField] float _time;
    
    FieldData _fieldData = new FieldData();
    public FieldData FieldData => _fieldData;
    
    // ‚Ç‚±‚©‚ç‚Å‚àŒÄ‚×‚é‚æ‚¤‚É
    private static FieldManager _instance = null;
    public static FieldManager Instance => _instance;

    [SerializeField] List<SpawnData> _spownDatas;

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

    int _setUpdateTime;

    private void Awake()
    {
        _instance = this;

        GameObject hitParticle = (GameObject)Resources.Load("HitParticle");
        _hitParticlePool.SetUp(hitParticle.GetComponent<ParticleUser>(), transform, 10);

        GameObject deadParticle = (GameObject)Resources.Load("DeadParticle");
        _deadParticlePool.SetUp(deadParticle.GetComponent<ParticleUser>(), transform, 5);

        GameObject explosionParticle = (GameObject)Resources.Load("PlasmaExplosionEffect");
        _explosionParticlePool.SetUp(explosionParticle.GetComponent<ParticleUser>(), transform, 5);
    }

    void Start()
    {
        _fieldData = new FieldData(_spownDatas, _enemyMasterData);
        _fieldData.UpdateEnemy();
        SoundMaster.Request(null, "FieldBGM", 3);

        _setUpdateTime = _updateTime;
    }

    void Update()
    {
        GameManager.Instance.GameTime();
        if (GameManager.Instance.GetCurrentTime > _updateTime)
        {
            _updateTime += _setUpdateTime;
            _fieldData.Update();
            UIManager.CallBack(UIType.Game, 3, new object[] { 0 });
        }
    }

    public static void FieldTimeRate(Action<UIType, int, object[]> action, UIType type, int id)
    {
        SoundMaster.Request(null, "StartSlowMotion", 1);
        Instance.StartCoroutine(Instance.SetRate(action, type, id));
    }

    IEnumerator SetRate(Action<UIType, int, object[]> action, UIType type, int id)
    {
        Time.timeScale = 1 / _rate;
        yield return new WaitForSecondsRealtime(_time);
        Time.timeScale = 1;

        if (action != null) action.Invoke(type, id, null);
    }

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

        for (int i = 0; i < 5; i++)
        {
            ItemExp itemExp = Instantiate(itemBase).GetComponent<ItemExp>();
            itemExp.SetOtherData(datas);
            itemExp.gameObject.transform.position = obj.transform.position;
            itemExp.Force(obj.transform.position);
        }

        Destroy(obj);
    }
}
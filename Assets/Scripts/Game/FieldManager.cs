using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnemysData;
using Sounds;

public class FieldManager : MonoBehaviour
{
    [SerializeField] EnemyMasterData _enemyMasterData;
    [SerializeField] float _rate;
    [SerializeField] float _time;

    FieldEnemyData _fieldData;

    // ‚Ç‚±‚©‚ç‚Å‚àŒÄ‚×‚é‚æ‚¤‚É
    private static FieldManager _instance = null;
    public static FieldManager Instance => _instance;

    [SerializeField] List<SpawnData> _spowns;

    [Serializable]
    public class SpawnData
    {
        public int ID;
        public int Level;
        public Transform Point;
        public float Range;
        public EnemyType[] Enemys;
    }

    ObjectPool<ParticleUser> _hitParticlePool = new ObjectPool<ParticleUser>();
    public ObjectPool<ParticleUser> GetHitParticle => _hitParticlePool;

    ObjectPool<ParticleUser> _deadParticlePool = new ObjectPool<ParticleUser>();
    public ObjectPool<ParticleUser> GetDeadParticle => _deadParticlePool;

    private void Awake()
    {
        _instance = this;

        GameObject hitParticle = (GameObject)Resources.Load("HitParticle");
        _hitParticlePool.SetUp(hitParticle.GetComponent<ParticleUser>(), transform, 10);

        GameObject deadParticle = (GameObject)Resources.Load("DeadParticle");
        _deadParticlePool.SetUp(deadParticle.GetComponent<ParticleUser>(), transform, 10);

        _fieldData = new FieldEnemyData(_spowns, _enemyMasterData);
        _fieldData.SetEnemy();
    }

    void Start()
    {
        SoundMaster.Request(null, "FieldBGM", 3);
    }

    void Update()
    {
        GameManager.GameTime();
        if (GameManager.Instance.GetCurrentTime > 4)
        {
            _fieldData.Update();
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
        action.Invoke(type, id, null);
    }
}
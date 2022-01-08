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

    // ‚Ç‚±‚©‚ç‚Å‚àŒÄ‚×‚é‚æ‚¤‚É
    private static FieldManager _instance = null;
    public static FieldManager Instance => _instance;

    [SerializeField] List<SpownData> _spowns;

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

        SetEnemy();
    }

    void Start()
    {
        SoundMaster.Request(null, "FieldBGM", 3);
    }

    void SetEnemy()
    {
        foreach (SpownData data in _spowns)
        {
            GameObject spownPoint = new GameObject("Point");
            spownPoint.transform.position = data.Point.position;
            foreach (EnemyData enemyData in _enemyMasterData.GetData)
            {
                foreach (EnemyType enemyType in data.Enemys)
                {
                    if (enemyData.Name == enemyType.ToString())
                    {
                        GameObject get = Instantiate(enemyData.Prefab);
                        get.transform.position = data.Point.position;
                        get.transform.SetParent(spownPoint.transform);

                        get.GetComponent<CharaBase>()
                            .SetParam(enemyData.HP, enemyData.Power, enemyData.Speed);
                    }
                }
            }
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

[Serializable]
class SpownData
{
    public int ID;
    public int Level;
    public Transform Point;
    public float Range;
    public EnemyType[] Enemys;
}
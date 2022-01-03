using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FieldManager : MonoBehaviour
{
    [SerializeField] float _rate;
    [SerializeField] float _time;

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
    }

    void Start()
    {
        Sounds.SoundMaster.Request(null, "FieldBGM", 3);
    }

    public static void FieldTimeRate(Action<UIType, int, object[]> action, UIType type, int id)
    {
        Sounds.SoundMaster.Request(null, "StartSlowMotion", 1);
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
    public int Level;
    public Vector3 Point;
    public float Range;
    public EnemyDataBase EnemyDatas;
}

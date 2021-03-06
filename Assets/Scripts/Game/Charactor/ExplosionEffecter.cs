using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objectを爆発させる際のEffect管理
/// </summary>

public class ExplosionEffecter :MonoBehaviour
{
    ObjectPool<ParticleUser> _explosionParticlePool = new ObjectPool<ParticleUser>();

    // 爆発EffectのPoolを作成
    public void SetUp(Transform parent)
    {
        GameObject explosionParticle = (GameObject)Resources.Load("SmallExplosion");
        _explosionParticlePool.SetUp(explosionParticle.GetComponent<ParticleUser>(), parent, 10);
    }

    /// <summary>
    /// 爆発させるObjectのダミーを生成
    /// </summary>
    /// <param name="t">対象のObject</param>
    /// <param name="delayTime">何秒後に爆発させるか</param>
    public void SetDummy(GameObject t, float delayTime)
    {
        GameObject obj = Instantiate(t);
        obj.transform.position = t.transform.position;
        obj.RemoveComponentAll();

        StartCoroutine(Explosion(delayTime, t));
    }

    IEnumerator Explosion(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);
        ParticleUser particle = _explosionParticlePool.Respons();
        particle.Use(obj.transform);
    }
}

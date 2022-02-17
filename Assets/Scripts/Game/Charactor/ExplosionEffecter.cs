using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object‚ğ”š”­‚³‚¹‚éÛ‚ÌEffectŠÇ—
/// </summary>

public class ExplosionEffecter :MonoBehaviour
{
    ObjectPool<ParticleUser> _explosionParticlePool = new ObjectPool<ParticleUser>();

    // ”š”­Effect‚ÌPool‚ğì¬
    public void SetUp(Transform parent)
    {
        GameObject explosionParticle = (GameObject)Resources.Load("SmallExplosion");
        _explosionParticlePool.SetUp(explosionParticle.GetComponent<ParticleUser>(), parent, 10);
    }

    /// <summary>
    /// ”š”­‚³‚¹‚éObject‚Ìƒ_ƒ~[‚ğ¶¬
    /// </summary>
    /// <param name="t">‘ÎÛ‚ÌObject</param>
    /// <param name="delayTime">‰½•bŒã‚É”š”­‚³‚¹‚é‚©</param>
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

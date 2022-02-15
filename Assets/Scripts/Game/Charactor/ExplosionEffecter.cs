using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffecter :MonoBehaviour
{
    ObjectPool<ParticleUser> _explosionParticlePool = new ObjectPool<ParticleUser>();

    public void SetUp(Transform parent)
    {
        GameObject explosionParticle = (GameObject)Resources.Load("PlasmaExplosionEffect");
        _explosionParticlePool.SetUp(explosionParticle.GetComponent<ParticleUser>(), parent, 10);
    }

    public void SetDummy(GameObject t, float delayTime)
    {
        GameObject obj = Object.Instantiate(t);
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

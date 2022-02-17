using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object�𔚔�������ۂ�Effect�Ǘ�
/// </summary>

public class ExplosionEffecter :MonoBehaviour
{
    ObjectPool<ParticleUser> _explosionParticlePool = new ObjectPool<ParticleUser>();

    // ����Effect��Pool���쐬
    public void SetUp(Transform parent)
    {
        GameObject explosionParticle = (GameObject)Resources.Load("SmallExplosion");
        _explosionParticlePool.SetUp(explosionParticle.GetComponent<ParticleUser>(), parent, 10);
    }

    /// <summary>
    /// ����������Object�̃_�~�[�𐶐�
    /// </summary>
    /// <param name="t">�Ώۂ�Object</param>
    /// <param name="delayTime">���b��ɔ��������邩</param>
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

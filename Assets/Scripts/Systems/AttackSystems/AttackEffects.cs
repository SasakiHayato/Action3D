using System.Collections;
using Cinemachine;
using UnityEngine;

namespace NewAttacks
{
    public class AttackEffects : MonoBehaviour
    {
        private static AttackEffects s_instance = null;
        public static AttackEffects Instance
        {
            get
            {
                var instance = FindObjectOfType(typeof(AttackEffects));
                if (instance != null)
                {
                    s_instance = (AttackEffects)instance;
                }
                else
                {
                    GameObject obj = new GameObject("AttackEffect");
                    s_instance = obj.AddComponent<AttackEffects>();
                    obj.hideFlags = HideFlags.HideInHierarchy;
                }

                return s_instance;
            }
        }

        public void HitParticle(Transform target)
        {
            float rotateX = Random.Range(-180, 180);
            Vector3 rotate = new Vector3(rotateX, 90, 0);

            ParticleUser particle = FieldManager.Instance.GetHitParticle.Respons();
            particle.Use(target, Quaternion.Euler(rotate));
        }

        public void HitStop(Animator anim)
        {
            StartCoroutine(Instance.WaitTime(anim));
        }

        IEnumerator WaitTime(Animator anim)
        {
            anim.speed = 1 / 48;
            yield return new WaitForSeconds(0.15f);
            anim.speed = 1f;
        }

        public void ShakeCm()
        {
            GameObject obj = GameObject.Find("3drParsonCm");
            CinemachineImpulseSource source = obj.GetComponent<CinemachineImpulseSource>();
            source.GenerateImpulse();
        }
    }
}
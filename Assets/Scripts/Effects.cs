using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSetting
{
    public enum EffectType
    {
        HitStop,
        ShakeCm,
        HitParticle,

        None,
    }

    public class Effects : MonoBehaviour
    {
        private static Effects _instance = null;
        public static Effects Instance
        {
            get
            {
                object instance = FindObjectOfType(typeof(Effects));
                if (instance != null) _instance = (Effects)instance;
                else
                {
                    GameObject obj = new GameObject("Effects");
                    _instance = obj.AddComponent<Effects>();
                    obj.hideFlags = HideFlags.HideInHierarchy;
                }

                return _instance;
            }
        }

        public static void HitStop(Animator anim)
        {
            Debug.Log("EffctType HitStop");
            Instance.StartCoroutine(Instance.WaitTime(anim));
        }
        IEnumerator WaitTime(Animator anim)
        {
            anim.speed = 0.1f;
            yield return new WaitForSeconds(0.1f);
            anim.speed = 1f;
        }

        public static void ShakeCm()
        {
            Debug.Log("EffctType ShakeCm");

        }

        public static void HitParticle(GameObject target)
        {
            Debug.Log("EffctType HitParticle");
            GameObject obj = Instantiate((GameObject)Resources.Load("HitParticle"));
            obj.transform.position = target.transform.position;
        }

        public static void None() => Debug.Log("EffctType None");
    }
}


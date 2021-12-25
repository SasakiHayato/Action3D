using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace AttackSetting
{
    public enum EffectType
    {
        HitStop,
        ShakeCm,
        HitParticle,
        KnonkBack,


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
            Instance.StartCoroutine(Instance.WaitTime(anim));
        }

        IEnumerator WaitTime(Animator anim)
        {
            anim.speed = 1 / 48;
            yield return new WaitForSeconds(0.15f);
            anim.speed = 1f;
        }

        public static void ShakeCm()
        {
            GameObject obj = GameObject.Find("3drParsonCm");
            CinemachineImpulseSource source = obj.GetComponent<CinemachineImpulseSource>();
            source.GenerateImpulse();
        }

        public static void HitParticle(GameObject target)
        {
            GameObject obj = Instantiate((GameObject)Resources.Load("HitParticle"));
            float rotateX = Random.Range(-180, 180);
            Vector3 rotate = new Vector3(rotateX, 90, 0);
            obj.transform.position = target.transform.position;
            obj.transform.rotation = Quaternion.Euler(rotate);
        }

        public static void KnockBack(GameObject target, AttackCollision.Parent parent, Transform parentT)
        {
            if (parent == AttackCollision.Parent.Player)
            {
                EnemyBase enemyBase = target.GetComponent<EnemyBase>();
                if (enemyBase != null) enemyBase.KnockBack(parentT.forward);
            }
            else
            {
                Player player = target.GetComponent<Player>();
                if (player != null) player.KnockBack(parentT.forward);
            }
        }

        public static void None() => Debug.Log("EffctType None");
    }
}


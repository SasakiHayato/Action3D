using System.Collections;
using UnityEngine;
using Cinemachine;

/// <summary>
/// çUåÇéûÇÃEffectä«óùÉNÉâÉX
/// </summary>

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
                }

                return _instance;
            }
        }

        const float HitStopTime = 0.2f;

        public static void HitStop(Animator anim)
        {
            Instance.StartCoroutine(Instance.WaitTime(anim));
        }

        IEnumerator WaitTime(Animator anim)
        {
            anim.speed = 1 / 48;
            yield return new WaitForSeconds(HitStopTime);
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
            float rotateX = Random.Range(-180, 180);
            Vector3 rotate = new Vector3(rotateX, 90, 0);

            ParticleUser particle = FieldManager.Instance.GetHitParticle.Respons();
            particle.Use(target.transform, Quaternion.Euler(rotate));
        }

        public static void KnockBack(GameObject target, AttackCollision.Parent parent, Transform parentT, AttackSettings.KnockBackData knock)
        {
            Vector3 setVec = Vector3.zero;
            switch (knock.Type)
            {
                case AttackSettings.KnockType.Forward:
                    setVec = parentT.forward;
                    break;
                case AttackSettings.KnockType.Up:
                    setVec = parentT.up;
                    break;
                case AttackSettings.KnockType.Down:
                    setVec = parentT.up * -1;
                    break;
                default:
                    break;
            }
            if (parent == AttackCollision.Parent.Player)
            {
                EnemyBase enemyBase = target?.GetComponent<EnemyBase>();
                if (enemyBase != null) enemyBase.KnockBack(setVec * knock.Power);
            }
            else
            {
                Player player = target?.GetComponent<Player>();
                if (player != null) player.KnockBack(setVec * knock.Power);
            }
        }

        public static void None() => Debug.Log("EffctType None");
    }
}


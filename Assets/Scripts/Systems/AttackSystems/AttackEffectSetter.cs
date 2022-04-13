using UnityEngine;
using System;

/// <summary>
/// çUåÇÇµÇΩç€ÇÃEffectê∂ê¨ÉNÉâÉX
/// </summary>

namespace NewAttacks
{
    public partial class AttackSettings : MonoBehaviour
    {
        public class AttackEffectSetter
        {
            GameObject _user;
            AttackCollider _weapon;

            public void SetUpUserData(GameObject user, AttackCollider collider)
            {
                _user = user;
                _weapon = collider;
            }

            public void Set(ref Action action, Effect[] effects)
            {
                foreach (Effect e in effects)
                {
                    action += () => Request(e);
                }
            }

            void Request(Effect effect)
            {
                switch (effect)
                {
                    case Effect.HitParticle:
                        AttackEffects.Instance.HitParticle(_weapon.transform);
                        break;
                    case Effect.HitStop:
                        AttackEffects.Instance.HitStop(_user.GetComponent<Animator>());
                        break;
                    case Effect.ShakeCm:
                        AttackEffects.Instance.ShakeCm();
                        break;
                }
            }
        }
    }
}
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace NewAttacks
{
    public class NewAttackCollider : MonoBehaviour, IAttackCollision
    {
        public enum Parent
        {
            Player,
            Enemy,
        }

        GameObject _parent;
        Transform _target;
        NewAttackSettings _settings;

        bool _isHit = false;

        public Parent ParentID { get; private set; }

        // IAttackCollision
        public string ParentTagName { get; private set; }
        public GameObject Target { get; private set; }
        public Collider Collider { get; private set; }

        const float EffectDist = 2f;
        const float SetSize = 2;

        public void SetUp(GameObject target, NewAttackSettings settings)
        {
            _parent = target;
            _target = target.transform;
            _settings = settings;

            if (transform.CompareTag("Player")) ParentID = Parent.Player;
            else ParentID = Parent.Enemy;
        }

        public void RipllesRequest()
        {
            var collisions = GameManager.Instance.AttackCollisions
                .Where(a =>
                {
                    if (ParentTagName != a.ParentTagName) return true;
                    else return false;
                })
                .Where(a =>
                {
                    float dist = Vector3.Distance(a.Target.transform.position, _target.position);
                    if (dist < EffectDist && a.Collider.enabled) return true;
                    else return false;
                });

            SetParticle(new List<IAttackCollision>(collisions));
        }

        void SetParticle(List<IAttackCollision> attacks)
        {
            attacks.ForEach(a =>
            {
                Vector3 midPos = (a.Target.transform.position + Target.transform.position) / 2;
                ParticleUser user = FieldManager.Instance.GetRipplesParticle.Respons();

                float rotateX = Random.Range(-180, 180);
                Vector3 rotate = new Vector3(rotateX, 90, 0);

                user.Use(midPos, Quaternion.Euler(rotate));

                Vector3 scale = user.transform.localScale;
                user.transform.DOScale(scale * SetSize, 0.4f)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => user.Delete());
            });
        }

        void OnTriggerEnter(Collider other)
        {
            if (_parent == null || _isHit) return;

            if (other.GetComponent<CharacterController>() != null)
            {
                if (_parent.CompareTag("Player") == other.CompareTag("Enemy"))
                    _settings.IsHitCallBack(other);
                else if (_parent.CompareTag("Enemy") == other.CompareTag("Player"))
                    _settings.IsHitCallBack(other);
            }
        }
    }

}
using Animancer;
using Animancer.Units;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerGame
{
    public sealed class Projectile : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D _Rigidbody;
        public Rigidbody2D Rigidbody => _Rigidbody;

        [SerializeField]
        private SpriteRenderer _Renderer;
        public SpriteRenderer Renderer => _Renderer;

        [SerializeField]
        private SoloAnimation _ImpactAnimation;
        public SoloAnimation ImpactAnimation => _ImpactAnimation;

        [SerializeField, Seconds]
        private float _Lifetime = 3;
        public float Lifetime => _Lifetime;

#if UNITY_EDITOR
        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Rigidbody);
            gameObject.GetComponentInParentOrChildren(ref _Renderer);
            gameObject.GetComponentInParentOrChildren(ref _ImpactAnimation);

            if (TryGetComponent<Collider2D>(out var collider) &&
                !collider.isTrigger)
                collider.isTrigger = true;
        }
#endif

        public Team Team { get; set; }
        public int Damage { get; set; }
        public float Force { get; set; }
        public Vector2 Direction { get; set; }
        public HashSet<Hit.ITarget> Ignore { get; set; }

        public void Fire(
            Vector2 velocity,
            Team team,
            int damage,
            float force,
            HashSet<Hit.ITarget> ignore)
            => Fire(velocity, team, damage, force, velocity.normalized, ignore);

        public void Fire(
            Vector2 velocity,
            Team team,
            int damage,
            float force,
            Vector2 direction,
            HashSet<Hit.ITarget> ignore)
        {
            _Rigidbody.velocity = velocity;

            Team = team;
            Damage = damage;
            Force = Force;
            Direction = direction;
            Ignore = ignore;

            Destroy(gameObject, _Lifetime);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var target = Hit.GetTarget(collider);
            if (target != null)
            {
                var hit = new Hit(transform, Team, Damage, Force, Direction, Ignore);

                if (!hit.TryHit(target))
                    return;
            }


            if (_ImpactAnimation == null)
            {
                Destroy(gameObject);
            }
            else
            {
                _Rigidbody.simulated = false;
                _ImpactAnimation.enabled = true;
                Destroy(gameObject, _ImpactAnimation.Clip.length / _ImpactAnimation.Speed);
            }
        }

        private void OnDisable()
        {
            if (Ignore != null)
                ObjectPool.Release(Ignore);
        }

    }
}

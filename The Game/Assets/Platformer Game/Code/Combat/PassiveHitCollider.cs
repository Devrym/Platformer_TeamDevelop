using Animancer;
using Animancer.Units;
using PlatformerGame.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerGame
{
    public sealed class PassiveHitCollider : MonoBehaviour
    {
        [SerializeField]
        private Team _Team;
        public ref Team Team => ref _Team;

        [SerializeField]
        private int _Damage = 10;
        public ref int Damage => ref _Damage;

        [SerializeField, Degrees, Tooltip(Strings.Tooltips.HitAngle)]
        private float _HitAngle = 90;
        public ref float HitAngle => ref _HitAngle;

        [SerializeField]
        private float _HitForce = 10;
        public ref float HitForce => ref _HitForce;

        [SerializeField, Seconds]
        private float _ReHitDelay = 0.2f;
        public ref float ReHitDelay => ref _ReHitDelay;

        private static readonly Dictionary<Hit.ITarget, float>
            LastHitTimes = new Dictionary<Hit.ITarget, float>();

        private static readonly List<ContactPoint2D> Contacts = new List<ContactPoint2D>(16);

        private void OnCollisionEnter2D(Collision2D collision)
            => OnCollisionStay2D(collision);

        private void OnCollisionStay2D(Collision2D collision)
        {
            var direction = HitData.AngleToDirection(_HitAngle, transform.rotation, false);

            var hit = new Hit(transform, _Team, _Damage, _HitForce, direction);

            var time = Time.timeSinceLevelLoad;

            Contacts.Clear();
            var count = collision.GetContacts(Contacts);
            for (int i = 0; i < count; i++)
            {
                var target = Hit.GetTarget(Contacts[i].collider);

                if (LastHitTimes.TryGetValue(target, out var lastHitTime) &&
                    lastHitTime + _ReHitDelay > time)
                    continue;

                LastHitTimes[target] = time;

                hit.TryHit(target);
            }
        }

    }
}

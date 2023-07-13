using Animancer;
using Animancer.Units;
using PlatformerGame.Characters;
using System;
using UnityEngine;

namespace PlatformerGame
{
    [Serializable]
    public sealed partial class HitData
    {
        [SerializeField]
        [Tooltip("The time when this hit becomes active.\n• " + AnimationTimeAttribute.Tooltip)]
        [AnimationTime(AnimationTimeAttribute.Units.Seconds)]
        private float _StartTime;

        public ref float StartTime => ref _StartTime;


        [SerializeField]
        [Tooltip("The time when this hit becomes inactive.\n• " + AnimationTimeAttribute.Tooltip)]
        [AnimationTime(AnimationTimeAttribute.Units.Seconds)]
        private float _EndTime;

        public ref float EndTime => ref _EndTime;

        [SerializeField]
        [Tooltip("The amount of damage this hit deals")]
        private int _Damage;

        public ref int Damage => ref _Damage;

        [SerializeField]
        [Tooltip("The knockback force applied to objects by this hit")]
        private float _Force;

        public ref float Force => ref _Force;

        [SerializeField, Degrees, Tooltip(Strings.Tooltips.HitAngle)]
        private float _Angle;

        public ref float Angle => ref _Angle;

        [SerializeField]
        [Tooltip("The outline of the area affected by this hit")]
        private Vector2[] _Area;

        public ref Vector2[] Area => ref _Area;

        public static void InitializeEvents(HitData[] hits, AnimancerEvent.Sequence events, float length)
        {
            if (hits == null)
                return;


            var inverseAnimationLength = 1f / length;

            var count = hits.Length;
            events.Capacity = Math.Max(events.Capacity, events.Count + count);

            var normalizedEndTime = float.IsNaN(events.NormalizedEndTime) ? 1 : events.NormalizedEndTime;

            var previousIndex = -1;
            for (int i = 0; i < count; i++)
            {
                var hit = hits[i];
                var start = hit._StartTime * inverseAnimationLength;
                var end = hit._EndTime * inverseAnimationLength;

                Debug.Assert(start < end, $"{nameof(HitData)}.{nameof(StartTime)} must be less than its {nameof(EndTime)}.");

                previousIndex = events.Add(previousIndex + 1, start, () =>
                {
                    var attacker = CharacterAnimancerComponent.GetCurrent();
                    attacker.AddHitBox(hit);
                });

                if (end < normalizedEndTime)
                {
                    previousIndex = events.Add(previousIndex + 1, end, () =>
                    {
                        var attacker = CharacterAnimancerComponent.GetCurrent();
                        attacker.RemoveHitBox(hit);
                    });
                }
            }

            events.OnEnd += () =>
            {
                var attacker = CharacterAnimancerComponent.GetCurrent();
                attacker.ClearHitBoxes();

            };
        }

        public bool IsActiveAt(float time) =>
            time >= _StartTime &&
            time < _EndTime;

        public override string ToString()
            => $"{nameof(HitData)}({GetDescription(", ")})";

        public string GetDescription(string delimiter) =>
            $"{nameof(StartTime)}={StartTime}" +
            $"{delimiter}{nameof(EndTime)}={EndTime}" +
            $"{delimiter}{nameof(Damage)}={Damage}" +
            $"{delimiter}{nameof(Force)}={Force}";

        public static Vector2 AngleToDirection(float angle, Quaternion rotation, bool flipX = false)
        {
            var direction = AngleToDirection(angle, flipX);
            return rotation * direction;
        }

        public static Vector2 AngleToDirection(float angle, bool flipX = false)
        {
            angle *= Mathf.Deg2Rad;

            var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            if (flipX)
                direction.x = -direction.x;

            return direction;
        }

        public static float DirectionToAngle(Vector2 direction, bool flipX = false)
        {
            if (flipX)
                direction.x = -direction.x;

            return Mathf.Atan2(direction.y, direction.x);
        }

    }
}

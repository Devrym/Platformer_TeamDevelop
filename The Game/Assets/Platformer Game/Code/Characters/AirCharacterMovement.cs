using Animancer.Units;
using UnityEngine;

namespace PlatformerGame.Characters
{
    public sealed class AirCharacterMovement : CharacterMovement
    {
        [SerializeField, MetersPerSecond] private float _HorizontalSpeed = 8;
        [SerializeField, MetersPerSecond] private float _AscentSpeed = 4;
        [SerializeField, MetersPerSecond] private float _DescentSpeed = 6;
        [SerializeField, Seconds] private float _Smoothing = 0.1f;

        private Vector2 _SmoothingVelocity;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.NotNegative(ref _HorizontalSpeed);
            PlatformerUtilities.NotNegative(ref _AscentSpeed);
            PlatformerUtilities.NotNegative(ref _DescentSpeed);
            PlatformerUtilities.NotNegative(ref _Smoothing);
        }
#endif

        protected override Vector2 UpdateVelocity(Vector2 velocity)
        {
            var brainMovement = Character.MovementDirection;
            var speedMultiplier = 1;

            Vector2 targetVelocity;
            if (speedMultiplier == 0)
            {
                targetVelocity = default;
            }
            else
            {
                targetVelocity.x = _HorizontalSpeed;
                targetVelocity.y = brainMovement.y > 0 ? _AscentSpeed : _DescentSpeed;
                targetVelocity = Vector2.Scale(targetVelocity, brainMovement) * speedMultiplier;
            }

            return PlatformerUtilities.SmoothDamp(velocity, targetVelocity, ref _SmoothingVelocity, _Smoothing);
        }

    }
}

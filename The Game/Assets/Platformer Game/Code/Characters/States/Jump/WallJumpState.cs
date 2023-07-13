using Animancer;
using Animancer.Units;
using UnityEngine;

namespace PlatformerGame.Characters.States
{
    public class WallJumpState : HoldJumpState
    {
        [SerializeField, Meters]
        [Tooltip("The wall detection range")]
        private float _DetectionDistance = 0.2f;
        public float DetectionDistance => _DetectionDistance;

        [SerializeField, Multiplier]
        [Tooltip("The amount of horizontal force applied (relative to the vertical force)")]
        private float _HorizontalMultiplier = 1;
        public float HorizontalMultiplier => _HorizontalMultiplier;

        [SerializeField]
        [Tooltip("The physics layers which count as walls")]
        private LayerMask _WallLayers = Physics2D.DefaultRaycastLayers;
        public LayerMask WallLayers => _WallLayers;

        private Vector2 _WallNormal;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.NotNegative(ref _DetectionDistance);
            PlatformerUtilities.NotNegative(ref _HorizontalMultiplier);
        }
#endif

        public override bool CanEnterState
        {
            get
            {
                if (Character.Body.IsGrounded ||
                    _DetectionDistance <= 0)
                    return false;


                var direction = new Vector2(Character.Animancer.FacingX, 0);
                if (CheckForWallJump(direction))
                    return true;


                if (CheckForWallJump(-direction))
                    return true;

                return false;
            }
        }

        private bool CheckForWallJump(Vector2 direction)
        {
            var bounds = Character.Body.Collider.bounds;

            var queriesHitTriggers = Physics2D.queriesHitTriggers;
            Physics2D.queriesHitTriggers = false;

            var count = Physics2D.BoxCastNonAlloc(
                bounds.center, bounds.size, Character.Body.Rotation, direction,
                PlatformerUtilities.OneRaycastHit, _DetectionDistance, _WallLayers);

            Physics2D.queriesHitTriggers = queriesHitTriggers;

            if (count > 0)
            {
                _WallNormal = -direction;
                return true;
            }
            else return false;
        }

        public override void OnEnterState()
        {
            Character.Body.IsGrounded = true;
            base.OnEnterState();
            Character.Body.IsGrounded = false;
            _WallNormal = default;
        }

        public override bool CanExitState => Animation.State.IsPlaying;

        public override Vector2 CalculateJumpVelocity()
        {
            AnimancerUtilities.Assert(_HorizontalMultiplier == 0 || _WallNormal != default,
                $"{nameof(WallJumpState)} can't calculate the jump velocity without the wall normal." +
                $" This likely means it was forced to enter without checking {nameof(CanEnterState)}");

            if (Character.MovementDirection.x == 0)
                Character.Animancer.Facing = _WallNormal;

            var speed = CalculateJumpSpeed(Height);

            var velocity = Character.Body.Velocity;
            velocity.x = 0;
            velocity.y *= Inertia;
            velocity.y += speed;
            velocity += _WallNormal * (speed * _HorizontalMultiplier);
            return velocity;
        }

    }
}

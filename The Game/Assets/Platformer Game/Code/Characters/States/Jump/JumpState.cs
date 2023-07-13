using Animancer;
using Animancer.Units;
using UnityEngine;

namespace PlatformerGame.Characters.States
{
    public class JumpState : CharacterState
    {
        [SerializeField]
        private ClipTransition _Animation;
        public ClipTransition Animation => _Animation;

        [SerializeField, Range(0, 1)]
        [Tooltip("Before the jump force is applied, the previous vertical velocity is multiplied by this value")]
        private float _Inertia = 0.25f;
        public float Inertia => _Inertia;

        [SerializeField, Meters]
        [Tooltip("The peak height of the jump arc")]
        private float _Height = 3;
        public float Height => _Height;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.Clamp(ref _Inertia, 0, 1);
            PlatformerUtilities.NotNegative(ref _Height);
        }
#endif

        protected virtual void Awake()
        {
            _Animation.Events.OnEnd += Character.StateMachine.ForceSetDefaultState;
        }

        public override bool CanEnterState => Character.Body.IsGrounded;

        public override void OnEnterState()
        {
            base.OnEnterState();

            Character.Body.Velocity = CalculateJumpVelocity();
            Character.Body.enabled = false;

            Character.Animancer.Play(_Animation);
        }

        public override float MovementSpeedMultiplier => 1;

        public override void OnExitState()
        {
            base.OnExitState();
            Character.Body.enabled = true;
        }

        public virtual Vector2 CalculateJumpVelocity() => CalculateJumpVelocity(_Inertia, _Height);

        public Vector2 CalculateJumpVelocity(float inertia, float height)
        {
            var velocity = Character.Body.Velocity;
            velocity.y *= inertia;
            velocity.y += CalculateJumpSpeed(height);
            return velocity;
        }

        public float CalculateJumpSpeed(float height)
        {
            var gravity = Character.Body.Gravity.y;
            AnimancerUtilities.Assert(gravity < 0, "Gravity is not negative");
            return Mathf.Sqrt(-2 * gravity * height);

        }

    }
}

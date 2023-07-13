using Animancer;
using Animancer.Units;
using UnityEngine;

namespace PlatformerGame.Characters.States
{
    public class LandState : CharacterState
    {
        [SerializeField]
        private ClipTransition _Animation;

        [SerializeField, MetersPerSecond]
        [Tooltip("This state will only activate if the character is moving at least this fast downwards when they land")]
        private float _RequiredDownSpeed = 7;

        [SerializeField, Range(0, 1)]
        [Tooltip("The character's speed is multiplied by this value while in this state")]
        private float _MovementSpeedMultiplier = 1;

        public override float MovementSpeedMultiplier => _MovementSpeedMultiplier;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.NotNegative(ref _RequiredDownSpeed);
            PlatformerUtilities.Clamp(ref _MovementSpeedMultiplier, 0, 1);
        }
#endif

        protected virtual void Awake()
        {
            _Animation.Events.OnEnd += Character.StateMachine.ForceSetDefaultState;

            Character.Body.OnGroundedChanged += OnGroundedChanged;
        }

        private void OnGroundedChanged(bool isGrounded)
        {
            if (isGrounded &&
                Context<ContactPoint2D>.Current.relativeVelocity.y >= _RequiredDownSpeed)
                Character.StateMachine.TrySetState(this);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            Character.Animancer.Play(_Animation);
        }

    }
}

using Animancer.Units;
using UnityEngine;
using static Animancer.Validate;

namespace PlatformerGame.Characters.States
{
    public class HoldJumpState : JumpState
    {
        [SerializeField, MetersPerSecondPerSecond(Rule = Value.IsNotNegative)]
        [Tooltip("The continuous acceleration applied while holding the jump button")]
        private float _HoldAcceleration = 40;
        public float HoldAcceleration => _HoldAcceleration;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.NotNegative(ref _HoldAcceleration);
        }
#endif

        protected virtual void FixedUpdate()
        {
            Character.Body.Velocity += new Vector2(0, _HoldAcceleration * Time.deltaTime);
        }

    }
}

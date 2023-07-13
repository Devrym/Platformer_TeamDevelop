using UnityEngine;

namespace PlatformerGame.Characters.States
{
    public class AirJumpState : HoldJumpState
    {
        [SerializeField]
        [Tooltip("The number of times the character can jump in the air without touching the ground")]
        private int _AirJumps;
        public int AirJumps => _AirJumps;

        private int _AirJumpsUsed;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.NotNegative(ref _AirJumps);
        }
#endif

        protected override void Awake()
        {
            base.Awake();

            Character.Body.OnGroundedChanged += (isGrounded) =>
            {
                if (isGrounded)
                    _AirJumpsUsed = 0;
            };
        }

        public override bool CanEnterState => !Character.Body.IsGrounded && _AirJumpsUsed < _AirJumps;

        public override void OnEnterState()
        {
            base.OnEnterState();
            _AirJumpsUsed++;
        }

    }
}

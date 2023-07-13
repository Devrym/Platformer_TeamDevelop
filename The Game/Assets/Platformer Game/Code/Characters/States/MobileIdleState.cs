using UnityEngine;

namespace PlatformerGame.Characters.States
{
    public class MobileIdleState : IdleState
    {
        [SerializeField, Range(0, 1)]
        [Tooltip("The character's speed is multiplied by this value while in this state")]
        private float _MovementSpeedMultiplier = 1;

        public override float MovementSpeedMultiplier => _MovementSpeedMultiplier;

    }
}

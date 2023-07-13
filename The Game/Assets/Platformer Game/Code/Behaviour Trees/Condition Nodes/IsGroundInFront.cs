
using Animancer.Units;
using PlatformerGame.Characters;
using System;
using UnityEngine;
using static Animancer.Validate;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public sealed class IsGroundInFront : ConditionNode
    {
        [SerializeField]
        [Meters(Rule = Value.IsNotNegative)]
        [Tooltip("The maximum distance within which to check (in meters)")]
        private float _Range = 1;

        public ref float Range => ref _Range;

        [SerializeField]
        [Tooltip("The layers which count as ground")]
        private LayerMask _Layers = Physics2D.DefaultRaycastLayers;

        public ref LayerMask Layers => ref _Layers;

#if UNITY_EDITOR
        [SerializeField]
        [Seconds(Rule = Value.IsNotNegative)]
        private float _DebugLineDuration;

        public ref float DebugLineDuration => ref _DebugLineDuration;
#endif

        public override bool Condition
        {
            get
            {
                var character = Context<Character>.Current;
                var body = character.Body;
                var stepHeight = Mathf.Max(body.StepHeight, Physics2D.defaultContactOffset * 2);

                var origin = body.Position;
                origin.x += character.MovementDirectionX * _Range;
                origin.y += stepHeight;

                var velocity = body.Velocity;
                if (Vector2.Dot(velocity, character.MovementDirection) > 0)
                    origin += velocity * Time.deltaTime;

                var distance = stepHeight * 2;

                if (Physics2D.Raycast(origin, Vector2.down, distance, _Layers))
                {
#if UNITY_EDITOR
                    PlatformerUtilities.DrawRay(origin, Vector2.down * distance, Color.blue, _DebugLineDuration);
#endif
                    return true;
                }
                else
                {
#if UNITY_EDITOR
                    PlatformerUtilities.DrawRay(origin, Vector2.down * distance, Color.red, _DebugLineDuration);
#endif
                    return false;
                }
            }
        }

    }
}

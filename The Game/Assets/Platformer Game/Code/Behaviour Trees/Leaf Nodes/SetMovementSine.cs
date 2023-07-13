using Animancer.Units;
using PlatformerGame.Characters;
using System;
using UnityEngine;
using static Animancer.Validate;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public sealed class SetMovementSine : LeafNode
    {
        [SerializeField]
        [Meters(Rule = Value.IsFinite)]
        [Tooltip("The height of the sine wave")]
        private float _Amplitude = 1;

        public ref float Amplitude => ref _Amplitude;

        [SerializeField]
        [Meters(Rule = Value.IsFinite)]
        [Tooltip("The spacing between peaks of the sine wave")]
        private float _Frequency = 1;

        public ref float Frequency => ref _Frequency;

        private float _BaseAltitude = float.NaN;

        public override Result Execute()
        {
            var character = Context<Character>.Current;
            if (float.IsNaN(_BaseAltitude))
                _BaseAltitude = character.Body.Position.y;

            var wave = Mathf.Sin(Time.timeSinceLevelLoad * _Frequency * Mathf.PI * 2) * _Amplitude;
            var targetAltitude = _BaseAltitude + wave;
            var currentAltitude = character.Body.Position.y;

            character.MovementDirectionY = Mathf.Clamp(targetAltitude - currentAltitude, -1, 1);

            return Result.Pass;
        }

    }
}

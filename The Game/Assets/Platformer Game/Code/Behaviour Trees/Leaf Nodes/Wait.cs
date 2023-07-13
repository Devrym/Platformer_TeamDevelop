using Animancer.Units;
using System;
using UnityEngine;
using static Animancer.Validate;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public sealed class Wait : LeafNode
    {
        [SerializeField]
        [Seconds(Rule = Value.IsNotNegative)]
        [Tooltip("The number of seconds to wait")]
        private float _Duration;

        public ref float Duration => ref _Duration;

        private float _ElapsedTime;

        public override Result Execute()
        {
            _ElapsedTime += Time.deltaTime;
            if (_ElapsedTime < _Duration)
                return Result.Pending;

            _ElapsedTime = 0;
            return Result.Pass;
        }

    }
}

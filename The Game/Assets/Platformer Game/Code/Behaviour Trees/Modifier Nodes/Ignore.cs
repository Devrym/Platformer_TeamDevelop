using System;
using UnityEngine;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public sealed class Ignore : ModifierNode
    {
        [SerializeField]
        [Tooltip("The result which this node returns after executing its child")]
        private Result _Result = Result.Pass;

        public ref Result Result => ref _Result;

        public override Result Execute()
        {
            Child?.Execute();
            return _Result;
        }

    }
}

using System;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public sealed class Invert : ModifierNode
    {
        public override Result Execute()
            => Child != null ? Child.Execute().Invert() : Result.Pass;

    }
}

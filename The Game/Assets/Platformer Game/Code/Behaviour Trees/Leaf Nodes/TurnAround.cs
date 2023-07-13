using PlatformerGame.Characters;
using System;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public sealed class TurnAround : LeafNode
    {
        public override Result Execute()
        {
            var character = Context<Character>.Current;
            character.MovementDirectionX = -character.MovementDirectionX;
            return Result.Pass;
        }

    }
}

using PlatformerGame.Characters;
using System;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public sealed class SetMovementForward : LeafNode
    {
        public override Result Execute()
        {
            var character = Context<Character>.Current;
            character.MovementDirection = character.Animancer.Facing;
            return Result.Pass;
        }
        
    }
}

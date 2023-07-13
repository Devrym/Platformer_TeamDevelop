using PlatformerGame.Characters;
using System;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public sealed class FaceAttacker : LeafNode
    {
        private Character _Character;

        public override Result Execute()
        {
            _Character = Context<Character>.Current;
            _Character.Health.OnHitReceived += TurnToFaceAttacker;
            return Result.Pass;
        }

        private void TurnToFaceAttacker(Hit hit)
        {
            if (hit.source == null)
                return;

            var direction = hit.source.position.x - _Character.Body.Position.x;
            if (direction == 0)
                return;

            _Character.MovementDirectionX = direction > 0 ? 1 : -1;
        }

    }
}

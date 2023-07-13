using System;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public sealed class Sequence : GroupNode
    {
        public override Result Execute()
        {
            for (int i = 0; i < Children.Length; i++)
            {
                var behaviour = Children[i];
                if (behaviour == null)
                    continue;

                var result = behaviour.Execute();
                if (result != Result.Pass)
                    return result;
            }

            return Result.Pass;
        }

    }
}

using System;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public sealed class Selector : GroupNode
    {
        public override Result Execute()
        {
            for (int i = 0; i < Children.Length; i++)
            {
                var behaviour = Children[i];
                if (behaviour == null)
                    continue;

                var result = behaviour.Execute();
                if (result != Result.Fail)
                    return result;
            }

            return Result.Fail;
        }

    }
}

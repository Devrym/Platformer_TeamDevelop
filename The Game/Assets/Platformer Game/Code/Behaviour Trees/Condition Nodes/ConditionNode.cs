using System;

namespace PlatformerGame.BehaviourTrees
{

    [Serializable]
    public abstract class ConditionNode : IBehaviourNode
    {

        public Result Execute() => Condition.ToResult();

        public abstract bool Condition { get; }

        int IBehaviourNode.ChildCount => 0;

        IBehaviourNode IBehaviourNode.GetChild(int index)
            => throw new NotSupportedException($"A {nameof(ConditionNode)} doesn't have any children.");

    }
}

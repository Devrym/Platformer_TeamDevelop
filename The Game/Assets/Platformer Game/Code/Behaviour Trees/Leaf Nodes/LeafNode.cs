using System;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public abstract class LeafNode : IBehaviourNode
    {
        public abstract Result Execute();

        int IBehaviourNode.ChildCount => 0;

        IBehaviourNode IBehaviourNode.GetChild(int index)
            => throw new NotSupportedException($"A {nameof(LeafNode)} doesn't have any children.");

    }
}

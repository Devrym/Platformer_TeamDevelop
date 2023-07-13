using System;

namespace PlatformerGame.BehaviourTrees
{
    public sealed class ActionNode : LeafNode
    {
        public Action Action { get; set; }

        public ActionNode() { }

        public ActionNode(Action action)
        {
            Action = action;
        }

        public override Result Execute()
        {
            try
            {
                Action?.Invoke();
                return Result.Pass;
            }
            catch
            {
                return Result.Fail;
            }
        }

    }
}

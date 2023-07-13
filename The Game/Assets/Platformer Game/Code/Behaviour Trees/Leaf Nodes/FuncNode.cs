using System;

namespace PlatformerGame.BehaviourTrees
{
    public sealed class FuncNode : LeafNode
    {
        public Func<Result> Func { get; set; }

        public FuncNode() { }

        public FuncNode(Func<Result> func)
        {
            Func = func;
        }

        public override Result Execute()
        {
            return Func.Invoke();
        }

    }
}

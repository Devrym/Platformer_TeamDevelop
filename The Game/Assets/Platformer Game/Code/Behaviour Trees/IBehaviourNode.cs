using Animancer;
using System.Collections.Generic;

namespace PlatformerGame.BehaviourTrees
{
   
    public interface IBehaviourNode : IPolymorphic
    {
       
        Result Execute();

        
        int ChildCount { get; }

        IBehaviourNode GetChild(int index);

    }

  
    public static partial class BehaviourTreeUtilities
    {

        public static List<IBehaviourNode> GetChildren(this IBehaviourNode node)
        {
            var list = ObjectPool.AcquireList<IBehaviourNode>();
            node.GetChildren(list);
            return list;
        }


        public static void GetChildren(this IBehaviourNode node, List<IBehaviourNode> list)
        {
            if (node == null)
                return;

            list.Add(node);

            var childCount = node.ChildCount;
            for (int i = 0; i < childCount; i++)
                node.GetChild(i).GetChildren(list);
        }

    }
}

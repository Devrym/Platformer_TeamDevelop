using Animancer;
using System;
using UnityEngine;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public abstract class GroupNode : IBehaviourNode, IPolymorphicReset
    {
        [SerializeReference]
        [Tooltip("The other nodes on which " + nameof(Execute) + " is called to determine the " + nameof(Result) + " of this node")]
        private IBehaviourNode[] _Children;

        public ref IBehaviourNode[] Children => ref _Children;

        void IPolymorphicReset.Reset()
        {
            _Children = new IBehaviourNode[2];
        }

        public abstract Result Execute();

        public int ChildCount => _Children.Length;

        public IBehaviourNode GetChild(int index) => _Children[index];

    }
}

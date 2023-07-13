using System;
using UnityEngine;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public abstract class ModifierNode : IBehaviourNode
    {
        [SerializeReference]
        [Tooltip("The other node on which " + nameof(Execute) + " is called to determine the " + nameof(Result) + " of this node")]
        private IBehaviourNode _Child;

        public ref IBehaviourNode Child => ref _Child;

        public virtual Result Execute()
        {
            if (_Child != null)
                return _Child.Execute();
            else
                return Result.Fail;
        }

        public int ChildCount => 1;

        public IBehaviourNode GetChild(int index) => _Child;

    }
}

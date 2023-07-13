using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public sealed class UnityEventNode : LeafNode
    {
        [SerializeField]
        [Tooltip("The delegate which will be invoked by " + nameof(Execute))]
        private UnityEvent _Action;

        public ref UnityEvent Action => ref _Action;

        public override Result Execute()
        {
            try
            {
                _Action?.Invoke();
                return Result.Pass;
            }
            catch
            {
                return Result.Fail;
            }
        }

    }
}

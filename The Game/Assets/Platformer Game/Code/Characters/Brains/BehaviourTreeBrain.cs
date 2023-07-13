using Animancer;
using PlatformerGame.BehaviourTrees;
using UnityEngine;

namespace PlatformerGame.Characters.Brains
{
    public class BehaviourTreeBrain : CharacterBrain
    {
        [SerializeReference] private IBehaviourNode _OnAwake;
        [SerializeReference] private IBehaviourNode _OnFixedUpdate;

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            _OnAwake = new SetMovementForward();
            _OnFixedUpdate = new Selector();
            ((IPolymorphicReset)_OnFixedUpdate).Reset();
        }
#endif

        protected virtual void Awake()
        {
            using (new Context<Character>(Character))
                _OnAwake?.Execute();
        }


        protected virtual void FixedUpdate()
        {
            using (new Context<Character>(Character))
                _OnFixedUpdate?.Execute();
        }

    }
}

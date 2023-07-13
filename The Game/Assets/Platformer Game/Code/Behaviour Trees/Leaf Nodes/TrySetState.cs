using PlatformerGame.Characters;
using PlatformerGame.Characters.States;
using System;
using UnityEngine;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public sealed class TrySetState : LeafNode
    {
        [SerializeField]
        [Tooltip("The state for the character to attempt to enter")]
        private CharacterState _State;

        public ref CharacterState State => ref _State;

        public override Result Execute()
        {
            var character = Context<Character>.Current;
            return character.StateMachine.TrySetState(_State).ToResult();
        }

    }
}

using Animancer;
using Animancer.FSM;
using UnityEngine;

namespace PlatformerGame.Characters.States
{
    public abstract class CharacterState : StateBehaviour, IOwnedState<CharacterState>
    {
        public const string MenuPrefix = Character.MenuPrefix + "States/";



        [SerializeField]
        private Character _Character;

        public Character Character => _Character;

        public void SetCharacter(Character character) => _Character = character;


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            gameObject.GetComponentInParentOrChildren(ref _Character);
        }
#endif


        public virtual float MovementSpeedMultiplier => 0;

        public virtual bool CanTurn => MovementSpeedMultiplier != 0;


        public StateMachine<CharacterState> OwnerStateMachine => _Character.StateMachine;

    }
}

using UnityEngine;

namespace PlatformerGame.Characters.States
{
    public sealed class SideAttackState : AttackState
    {
        [SerializeField] private AttackTransition _LeftAnimation;
        [SerializeField] private AttackTransition _RightAnimation;

        private void Awake()
        {
            _LeftAnimation.Events.OnEnd += Character.StateMachine.ForceSetDefaultState;
            _RightAnimation.Events.OnEnd += Character.StateMachine.ForceSetDefaultState;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            var animation = Character.MovementDirectionX < 0 ? _LeftAnimation : _RightAnimation;
            Character.Animancer.Play(animation);
        }

    }
}

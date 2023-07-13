using Animancer;
using UnityEngine;

namespace PlatformerGame.Characters.States
{
    public sealed class IntroductionState : CharacterState
    {
        [SerializeField] private PlayableAssetTransition _Animation;

        private void Awake()
        {
            if (!_Animation.IsValid)
                return;

            _Animation.Events.OnEnd = Character.StateMachine.ForceSetDefaultState;
            Character.StateMachine.TrySetState(this);
        }

        public override void OnEnterState()
        {
            Character.Animancer.Play(_Animation);
        }

        public override bool CanExitState => false;

        public override void OnExitState()
        {
            Character.Animancer.States.Destroy(_Animation);
        }

    }
}

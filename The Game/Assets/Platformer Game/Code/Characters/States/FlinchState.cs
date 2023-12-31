using Animancer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlatformerGame.Characters.States
{
    public sealed class FlinchState : CharacterState
    {
        [SerializeField, Range(0, 1)]
        [Tooltip("The character's speed is multiplied by this value while flinching")]
        private float _FlinchMovementSpeedMultiplier;

        public override float MovementSpeedMultiplier
            => Character.Health.CurrentHealth > 0 ? _FlinchMovementSpeedMultiplier : 0;

        public override bool CanTurn => false;

        [SerializeField]
        [Tooltip("The animation to play when the character gets hit by an attack")]
        private ClipTransition _FlinchAnimation;
        public ClipTransition FlinchAnimation => _FlinchAnimation;

        [SerializeField]
        [Tooltip("The animation to play when the character's health reaches 0")]
        private ClipTransition _DieAnimation;
        public ClipTransition DieAnimation => _DieAnimation;

        private void Awake()
        {
            _FlinchAnimation.Events.OnEnd += Character.StateMachine.ForceSetDefaultState;
            _DieAnimation.Events.OnEnd += () => Destroy(Character.gameObject);

            Character.Health.OnHitReceived += (hit) =>
            {
                if (hit.force > 0 && Character.Body != null)
                {
                    Character.StateMachine.ForceSetState(this);
                    Character.Body.Velocity += hit.direction * hit.force / Character.Body.Mass;
                }
                else if (hit.damage > 0)
                {
                    Character.StateMachine.ForceSetState(this);
                }
            };

            Character.Health.OnCurrentHealthChanged += (oldValue, newValue) =>
            {
                if (newValue <= 0)
                    Character.StateMachine.ForceSetState(this);
            };
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            var animation = Character.Health.CurrentHealth > 0 ? _FlinchAnimation : _DieAnimation;
            Character.Animancer.Play(animation);

            if (Character.Body != null)
                Character.Body.enabled = false;
        }

        public override bool CanExitState => false;

        public override void OnExitState()
        {
            base.OnExitState();

            if (Character.Body != null)
                Character.Body.enabled = true;
        }

        public void ReloadCurrentScene()
        {
            var scene = SceneManager.GetActiveScene();
#if UNITY_EDITOR
            UnityEditor.SceneManagement.EditorSceneManager.LoadSceneInPlayMode(scene.path, default);
#else
            SceneManager.LoadScene(scene.buildIndex);
#endif
        }

    }
}

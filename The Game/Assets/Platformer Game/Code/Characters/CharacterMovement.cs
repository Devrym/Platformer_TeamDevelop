using Animancer;
using PlatformerGame.Characters.Brains;
using UnityEngine;

namespace PlatformerGame.Characters
{
    public abstract class CharacterMovement : MonoBehaviour
    {
        public const int DefaultExecutionOrder = (CharacterBrain.DefaultExecutionOrder + CharacterBody2D.DefaultExecutionOrder) / 2;

        public const float MinimumSpeed = 0.01f;

        [SerializeField]
        private Character _Character;
        public ref Character Character => ref _Character;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Character);
        }
#endif

        protected virtual void FixedUpdate()
        {
            var previousVelocity = _Character.Body.Velocity;
            var velocity = UpdateVelocity(previousVelocity);

            if (velocity.sqrMagnitude < MinimumSpeed * MinimumSpeed)
                velocity = default;

            if (previousVelocity.x != velocity.x ||
                previousVelocity.y != velocity.y)
                _Character.Body.Velocity = velocity;
        }


        protected abstract Vector2 UpdateVelocity(Vector2 velocity);

    }
}

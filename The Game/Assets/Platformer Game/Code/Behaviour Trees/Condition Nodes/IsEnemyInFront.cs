using Animancer;
using Animancer.Units;
using PlatformerGame.Characters;
using System;
using UnityEngine;
using static Animancer.Validate;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public sealed class IsEnemyInFront : ConditionNode
    {
        [SerializeField]
        [Meters(Rule = Value.IsNotNegative)]
        [Tooltip("The maximum distance within which to check (in meters)")]
        private float _Range = 1;

        public ref float Range => ref _Range;

        public override bool Condition
        {
            get
            {
                var character = Context<Character>.Current;
                var rigidbody = character.Body.Rigidbody;

                var bounds = character.Body.Collider.bounds;
                var center = (Vector2)bounds.center + character.MovementDirection * _Range;

                var filter = new ContactFilter2D
                {
                    useLayerMask = true,
                };

                var colliders = ObjectPool.AcquireList<Collider2D>();
                Physics2D.OverlapBox(center, bounds.size, rigidbody.rotation, filter, colliders);
               
                ObjectPool.Release(colliders);
                return false;
            }
        }

    }
}

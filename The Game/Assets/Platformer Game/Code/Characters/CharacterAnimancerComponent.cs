using Animancer;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerGame.Characters
{
    public sealed class CharacterAnimancerComponent : AnimancerComponent
    {
        [SerializeField]
        private SpriteRenderer _Renderer;
        public SpriteRenderer Renderer => _Renderer;

        [SerializeField]
        private Character _Character;
        public Character Character => _Character;

#if UNITY_EDITOR
        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Renderer);
            gameObject.GetComponentInParentOrChildren(ref _Character);
        }
#endif


#if UNITY_ASSERTIONS
        private void Awake()
        {
            DontAllowFade.Assert(this);
        }
#endif

        public bool FacingLeft
        {
            get => _Renderer.flipX;
            set => _Renderer.flipX = value;
        }

        public float FacingX
        {
            get => _Renderer.flipX ? -1f : 1f;
            set
            {
                if (value != 0)
                    _Renderer.flipX = value < 0;
            }
        }

        public Vector2 Facing
        {
            get => new Vector2(FacingX, 0);
            set => FacingX = value.x;
        }

        private void Update()
        {
        }

        public static CharacterAnimancerComponent GetCurrent() => Get(AnimancerEvent.CurrentState);

        public static CharacterAnimancerComponent Get(AnimancerNode node) => Get(node.Root);

        public static CharacterAnimancerComponent Get(AnimancerPlayable animancer) => animancer.Component as CharacterAnimancerComponent;

        #region Hit Boxes



        #endregion
    }
}

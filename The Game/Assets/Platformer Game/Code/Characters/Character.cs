using Animancer;
using Animancer.FSM;
using UnityEngine;

namespace PlatformerGame.Characters
{
    public class Character : MonoBehaviour
    {
        public const string MenuPrefix = Strings.MenuPrefix + "Characters/";



        [SerializeField]
        private CharacterAnimancerComponent _Animancer;
        public CharacterAnimancerComponent Animancer => _Animancer;

        [SerializeField]
        private CharacterBody2D _Body;
        public CharacterBody2D Body => _Body;




#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Animancer);
            gameObject.GetComponentInParentOrChildren(ref _Body);
        }
#endif


        private Vector2 _MovementDirection;

        public Vector2 MovementDirection
        {
            get => _MovementDirection;
            set
            {
                _MovementDirection.x = Mathf.Clamp(value.x, -1, 1);
                _MovementDirection.y = Mathf.Clamp(value.y, -1, 1);
            }
        }

        public float MovementDirectionX
        {
            get => _MovementDirection.x;
            set => _MovementDirection.x = Mathf.Clamp(value, -1, 1);
        }

        public float MovementDirectionY
        {
            get => _MovementDirection.y;
            set => _MovementDirection.y = Mathf.Clamp(value, -1, 1);
        }


        public bool Run { get; set; }


        protected virtual void Awake()
        {
        }

#if UNITY_EDITOR

        [UnityEditor.CustomEditor(typeof(Character), true)]
        public class Editor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (!UnityEditor.EditorApplication.isPlaying)
                    return;

                var target = (Character)this.target;

                target.MovementDirection = UnityEditor.EditorGUILayout.Vector2Field("Movement Direction", target.MovementDirection);

                target.Run = UnityEditor.EditorGUILayout.Toggle("Run", target.Run);

            }

        }

#endif
    }
}

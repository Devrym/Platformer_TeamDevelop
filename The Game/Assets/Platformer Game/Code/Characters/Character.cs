using Animancer;
using Animancer.FSM;
using PlatformerGame.Characters.States;
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

        [SerializeField]
        private Health _Health;
        public Health Health => _Health;

        [SerializeField]
        private CharacterState _Idle;
        public CharacterState Idle => _Idle;


#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Animancer);
            gameObject.GetComponentInParentOrChildren(ref _Body);
            gameObject.GetComponentInParentOrChildren(ref _Health);
            gameObject.GetComponentInParentOrChildren(ref _Idle);
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

        public readonly StateMachine<CharacterState>.WithDefault
            StateMachine = new StateMachine<CharacterState>.WithDefault();

        protected virtual void Awake()
        {
            StateMachine.DefaultState = _Idle;

#if UNITY_ASSERTIONS
            foreach (Transform child in transform)
                child.name = $"{child.name} ({name})";
#endif
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

                UnityEditor.EditorGUI.BeginChangeCheck();
                var state = UnityEditor.EditorGUILayout.ObjectField(
                    "Current State", target.StateMachine.CurrentState, typeof(CharacterState), true);
                if (UnityEditor.EditorGUI.EndChangeCheck())
                    target.StateMachine.TrySetState((CharacterState)state);
            }

        }

#endif
    }
}

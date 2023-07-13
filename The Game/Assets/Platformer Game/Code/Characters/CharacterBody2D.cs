using Animancer;
using PlatformerGame.Characters.Brains;
using System;
using UnityEngine;

namespace PlatformerGame.Characters
{
    public class CharacterBody2D : MonoBehaviour
    {
        public const int DefaultExecutionOrder = CharacterBrain.DefaultExecutionOrder + 1000;

        [SerializeField]
        private Collider2D _Collider;

        public Collider2D Collider => _Collider;

        [SerializeField]
        private Rigidbody2D _Rigidbody;

        public Rigidbody2D Rigidbody => _Rigidbody;


#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Collider);
            gameObject.GetComponentInParentOrChildren(ref _Rigidbody);

            if (_Rigidbody != null && enabled)
            {
                if (_Rigidbody.bodyType != RigidbodyType2D.Dynamic)
                    _Rigidbody.bodyType = RigidbodyType2D.Dynamic;

                if (!_Rigidbody.simulated)
                    _Rigidbody.simulated = true;
            }
        }
#endif

        public Vector2 Position
        {
            get => _Rigidbody.position;
            set => _Rigidbody.position = value;
        }

        public Vector2 Velocity
        {
            get => _Rigidbody.velocity;
            set => _Rigidbody.velocity = value;
        }

        public float Mass
        {
            get => _Rigidbody.mass;
            set => _Rigidbody.mass = value;
        }

        public float Rotation
        {
            get => 0;// _Rigidbody.rotation;
            set => throw new NotSupportedException("Rotation is not supported.");// _Rigidbody.rotation = value;
        }

        public virtual Vector2 Gravity
            => Physics2D.gravity * _Rigidbody.gravityScale;


        private bool _IsGrounded;

        public bool IsGrounded
        {
            get => _IsGrounded;
            set
            {
                if (_IsGrounded == value)
                    return;

                _IsGrounded = value;
                OnGroundedChanged?.Invoke(value);
            }
        }

        public event Action<bool> OnGroundedChanged;


        private ContactFilter2D _TerrainFilter;

        public ContactFilter2D TerrainFilter => _TerrainFilter;

        public virtual float GripAngle
        {
            get => 0;
            set => throw new NotSupportedException($"Can't set {GetType().FullName}.{nameof(GripAngle)}.");
        }

        public virtual float StepHeight
        {
            get => 0;
            set => throw new NotSupportedException($"Can't set {GetType().FullName}.{nameof(StepHeight)}.");
        }

        public virtual PlatformContact2D GroundContact => default;

        protected virtual void Awake()
        {
            _TerrainFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        }

        protected virtual void OnDisable()
        {
            IsGrounded = false;
        }

#if UNITY_EDITOR
        [UnityEditor.CustomEditor(typeof(CharacterBody2D), true)]
        public class Editor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (!UnityEditor.EditorApplication.isPlaying)
                    return;

                using (new UnityEditor.EditorGUI.DisabledScope(true))
                {
                    var target = (CharacterBody2D)this.target;
                    UnityEditor.EditorGUILayout.Toggle("Is Grounded", target.IsGrounded);
                }
            }

          }

#endif
    }
}

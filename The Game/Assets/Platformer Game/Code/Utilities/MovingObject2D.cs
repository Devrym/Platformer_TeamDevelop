using Animancer;
using Animancer.Units;
using UnityEngine;

namespace PlatformerGame
{
    public sealed class MovingObject2D : MonoBehaviour
    {
        public const int DefaultExecutionOrder = -10000;

        [SerializeField]
        [Tooltip("The object to move")]
        private Rigidbody2D _Rigidbody;

        [SerializeField]
        [Tooltip("The destination to move to (relative to the starting position)")]
        private Vector2 _Movement;

        [SerializeField, MetersPerSecond]
        [Tooltip("The speed at which the object moves")]
        private float _Speed = 3;

        [SerializeField]
        [Tooltip("Should it keep moving back and forth between the start and end?")]
        private bool _Loop = true;

        private Vector2 _StartingPosition;
        private bool _Return;


#if UNITY_EDITOR
        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Rigidbody);
            PlatformerUtilities.NotNegative(ref _Speed);
        }
#endif


        private void Awake()
        {
            _StartingPosition = _Rigidbody.position;
        }


        private void FixedUpdate()
        {
            var movement = _Speed * Time.deltaTime;
            var startingPosition = _Rigidbody.position;
            var position = startingPosition;

            Move:

            var destination = _StartingPosition;
            if (!_Return)
                destination += _Movement;

            var offset = destination - position;
            var distance = offset.magnitude;
            if (distance > movement)// Still going.
            {
                position += offset * movement / distance;

                _Rigidbody.velocity = (position - startingPosition) / Time.deltaTime;
                _Rigidbody.MovePosition(position);
            }
            else if (_Loop)
            {
                position = destination;
                movement -= distance;
                _Return = !_Return;
                goto Move;
            }
            else
            {
                _Rigidbody.velocity = default;
                _Rigidbody.MovePosition(destination);
                enabled = false;
                return;
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var collider = GetComponent<Collider2D>();
            if (collider == null)
            {
                var start = UnityEditor.EditorApplication.isPlaying ? (Vector3)_StartingPosition : transform.position;

                Gizmos.DrawLine(start, start + (Vector3)_Movement);
                return;
            }

            var bounds = collider.bounds;
            var center = UnityEditor.EditorApplication.isPlaying ? (Vector3)_StartingPosition : bounds.center;
            var extents = bounds.extents;

            var corner = extents;
            Gizmos.DrawLine(center + corner, center + corner + (Vector3)_Movement);

            corner.y = -corner.y;
            Gizmos.DrawLine(center + corner, center + corner + (Vector3)_Movement);

            corner.x = -corner.x;
            Gizmos.DrawLine(center + corner, center + corner + (Vector3)_Movement);

            corner.y = -corner.y;
            Gizmos.DrawLine(center + corner, center + corner + (Vector3)_Movement);
        }
#endif

    }
}
